#if UNITY_EDITOR && UNITY_ANDROID
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Xml;
using VoxelBusters.CoreLibrary;
using VoxelBusters.EssentialKit;
using System.Collections.Generic;
using System.IO;

namespace VoxelBusters.EssentialKit.Editor.Build.Android
{
    [InitializeOnLoad]
    public class FirebaseSettingsGenerator
    {

        internal static string kGoogleServicesJsonPath              = "Assets/google-services.json";
        private static string kGoogleServicesGeneratedXmlFilePath   = EssentialKitPackageLayout.AndroidProjectResValuesPath + "/google-services.xml";
        private static string kGoogleServicesXmlTemplatePath        = EssentialKitPackageLayout.AndroidEditorSourcePath + "/google-services.template";

        private static bool IsGoogleServicesJsonFileAvailable()
        {
            return IOServices.FileExists(kGoogleServicesJsonPath);
        }

        public static void Execute()
        {
            // Settings
            XmlWriterSettings _settings = new XmlWriterSettings();
            _settings.Encoding = new System.Text.UTF8Encoding(true);
            _settings.ConformanceLevel = ConformanceLevel.Document;
            _settings.Indent = true;


            string targetPath   = kGoogleServicesGeneratedXmlFilePath;
            string templatePath = kGoogleServicesXmlTemplatePath;

            IOServices.CreateDirectory(IOServices.GetDirectoryName(targetPath));
            
            // Replace strings in Template
            string templateContent = IOServices.ReadFile(templatePath);

            if (IsGoogleServicesJsonFileAvailable())
            {
                string googleServicesJsonContent = IOServices.ReadFile(kGoogleServicesJsonPath);

                IJsonServiceProvider serializer             = ExternalServiceProvider.JsonServiceProvider;
                IDictionary googleServicesJsonContentMap    = null;

                try
                {
                    googleServicesJsonContentMap = (IDictionary)serializer.FromJson(googleServicesJsonContent);
                }
                catch(System.Exception exception)
                {
                    Debug.LogError(exception);
                    Debug.LogError("Failed deserializing google services json content. Cross check serializer set to ExternalServiceProvider.JsonServiceProvider returns IDictionary and can parse dictionary data");
                    return;
                }

                IDictionary projectInfo                     = (IDictionary)googleServicesJsonContentMap["project_info"];

                if (projectInfo != null)
                {
                    string firebaseDatabaseURL = projectInfo["firebase_url"] as string;
                    string projectNumber = projectInfo["project_number"] as string;
                    string storageBucket = projectInfo["storage_bucket"] as string;
                    string projectID = projectInfo["project_id"] as string;

                    templateContent = templateContent.Replace("FIREBASE_DATABASE_URL", firebaseDatabaseURL);
                    templateContent = templateContent.Replace("GCM_DEFAULT_SENDER_ID", projectNumber);
                    templateContent = templateContent.Replace("GOOGLE_STORAGE_BUCKET", storageBucket);
                    templateContent = templateContent.Replace("PROJECT_ID", projectID);

                    IList clientList = (IList)googleServicesJsonContentMap["client"];

                    if (clientList != null && clientList.Count > 0)
                    {
                        bool foundMatchingPackageNameClient = false;
                        foreach (IDictionary eachClient in clientList)
                        {
                            IDictionary clientInfo = (IDictionary)eachClient["client_info"];
                            IDictionary androidClientInfo = (IDictionary)clientInfo["android_client_info"];
                            
                            if (androidClientInfo != null)
                            {
                                string clientPackageName = (string)androidClientInfo["package_name"];
                                if (clientPackageName.Equals(UnityEngine.Application.identifier))
                                {
                                    IList oauthClientList = (IList)eachClient["oauth_client"];
                                    string oauthClientId = "";

                                    if (oauthClientList != null && oauthClientList.Count > 0)
                                    {
                                        IDictionary dict = (IDictionary)oauthClientList[0];
                                        oauthClientId = dict["client_id"] as string;
                                    }

                                    IDictionary apiKey = (IDictionary)(((IList)eachClient["api_key"])[0]);

                                    templateContent = templateContent.Replace("GOOGLE_APP_ID", clientInfo["mobilesdk_app_id"] as string);
                                    templateContent = templateContent.Replace("GOOGLE_API_KEY", apiKey["current_key"] as string);
                                    templateContent = templateContent.Replace("GOOGLE_CRASH_REPORTING_API_KEY", apiKey["current_key"] as string);
                                    templateContent = templateContent.Replace("DEFAULT_WEB_CLIENT_ID", oauthClientId);

                                    foundMatchingPackageNameClient = true;
                                    break;
                                }
                            }
                        }

                        if(!foundMatchingPackageNameClient)
                        {
                            DebugLogger.LogWarning("Unable to find GOOGLE_APP_ID as no matching package name exists in google-services.json");
                        }
                    }
                    IOServices.CreateFile(targetPath, templateContent);
                }
                else
                {
                    Debug.LogError("Invalid google-services.json file. Please get a valid file. You can get one from Firebase Console -> Project Settings(gear icon) -> General -> Your Apps section");
                }
            }
        }
    }
}

#endif
