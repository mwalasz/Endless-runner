#if UNITY_ANDROID && UNITY_2018_4_OR_NEWER
using System;
using System.Linq;
using System.Text;
using UnityEditor.Android;
using System.IO;
using UnityEngine;
using System.Collections.Generic;

namespace VoxelBusters.EssentialKit.Editor.Build.Android
{
    class GradleFileProperties
    {
        public string AndroidGradlePluginVersion;
        public string CompileSdkVersion;
        public string TargetSdkVersion;
        public string BuildToolsVersion;

        public override string ToString()
        {
            return string.Format("Gradle Version : {0} \n" +
                "Compile Sdk Version : {1} \n" +
                "Target Sdk Version : {2} \n" +
                "BuildToolsVersion : {3}", AndroidGradlePluginVersion, CompileSdkVersion, TargetSdkVersion, BuildToolsVersion);
        }
    }

    class GradlePostProcessor : IPostGenerateGradleAndroidProject
    {
        public int callbackOrder { get { return 0; } }
        public void OnPostGenerateGradleAndroidProject(string path)
        {
            GradleFileProperties gradleFileProperties = new GradleFileProperties();
            UpdateGradleFileProperties(path, gradleFileProperties);

#if UNITY_2019_3_OR_NEWER
            gradleFileProperties.AndroidGradlePluginVersion = GetGradleVersion(path + "/..");
            PatchGradleForPackingOptions(path + "/../launcher");
            //EnableJetifierIfRequired(path + "/..");
#else
            PatchGradleForPackingOptions(path);
            //EnableJetifierIfRequired(path);
#endif
            //MatchUnityBuildGradleVersions(path);
            Debug.Log("Gradle file properties : " + gradleFileProperties);
            RegenerateManifestFileIfRequired(path, gradleFileProperties);
        }

        //This maps only android gradle version if exists only (From 2019.3 it won't update android gradle version)
        private void MatchUnityBuildGradleVersions(string path)
        {
            string rootGradleVersion = null;
            string rootCompileSdkVersion = null;
            string rootBuildToolsVersion = null;
            string rootTargetSDKVersion = null;

            string[] targetProjectPaths =
            {
                "/com.voxelbusters.essentialkit.androidlib"
            };

            // First read the main build.gradle file
            string[] lines = File.ReadAllLines(path + "/build.gradle");

            foreach (string eachLine in lines)
            {
                // Detect gradle version
                if (HasText(eachLine, "classpath", "tools.build:gradle"))
                {
                    rootGradleVersion = eachLine;
                }
                // Detect compileSdkVersion version
                else if (HasText(eachLine, "compileSdkVersion"))
                {
                    rootCompileSdkVersion = eachLine;
                }
                // Detect buildToolsVersion version
                else if (HasText(eachLine, "buildToolsVersion"))
                {
                    rootBuildToolsVersion = eachLine;
                }
                // Detect targetSdkVersion version
                else if (HasText(eachLine, "targetSdkVersion"))
                {
                    rootTargetSDKVersion = eachLine;
                }
            }


            foreach (string eachProject in targetProjectPaths)
            {
                UpdateGradleFile(path + eachProject, rootGradleVersion, rootCompileSdkVersion, rootBuildToolsVersion, rootTargetSDKVersion);
            }
        }

        private void UpdateGradleFile(string projectPath, string rootGradleVersion, string rootCompileSdkVersion, string rootBuildToolsVersion, string rootTargetSDKVersion)
        {
            string filePath = projectPath + "/build.gradle";

            if (File.Exists(filePath))
            {
                string[] lines = File.ReadAllLines(filePath);
                string[] updatedLines = new string[lines.Length];

                for (int i = 0; i < lines.Length; i++)
                {
                    string eachLine = lines[i];
                    string updatedText = eachLine;

                    // Detect gradle version
                    if (HasText(eachLine, "classpath", "tools.build:gradle"))
                    {
                        updatedText = rootGradleVersion;
                    }
                    // Detect compileSdkVersion version
                    else if (HasText(eachLine, "compileSdkVersion"))
                    {
                        updatedText = rootCompileSdkVersion;
                    }
                    // Detect buildToolsVersion version
                    else if (HasText(eachLine, "buildToolsVersion"))
                    {
                        updatedText = rootBuildToolsVersion;
                    }
                    // Detect targetSdkVersion version
                    else if (HasText(eachLine, "targetSdkVersion"))
                    {
                        updatedText = rootTargetSDKVersion;
                    }

                    updatedLines[i] = updatedText;
                }

                File.WriteAllLines(filePath, updatedLines);
            }
        }

        private bool HasText(string inputString, string startSearchString, string additionalStringToSearch = null)
        {
            string trimmedText = inputString.Trim();

            if (trimmedText.StartsWith(startSearchString, System.StringComparison.InvariantCulture))
            {
                if (additionalStringToSearch != null)
                {
                    return trimmedText.Contains(additionalStringToSearch);
                }

                return true;
            }

            return false;
        }

        private GradleFileProperties UpdateGradleFileProperties(string path, GradleFileProperties properties)
        {
            string[] lines = File.ReadAllLines(path + "/build.gradle");

            foreach (string eachLine in lines)
            {
                // Detect gradle version
                if (HasText(eachLine, "classpath", "tools.build:gradle"))
                {
                    string[] components = eachLine.Trim().Split(':');
                    if (components.Length > 2)
                    {
                        properties.AndroidGradlePluginVersion = components[2].Replace("'", "").Replace("\"", "");
                    }
                }
                // Detect compileSdkVersion version
                else if (HasText(eachLine, "compileSdkVersion"))
                {
                    string[] components = eachLine.Trim().Split(' ');
                    if (components.Length > 1)
                    {
                        properties.CompileSdkVersion = components[components.Length - 1];
                    }

                }
                // Detect buildToolsVersion version
                else if (HasText(eachLine, "buildToolsVersion"))
                {
                    string[] components = eachLine.Trim().Split(' ');
                    if (components.Length > 1)
                    {
                        properties.BuildToolsVersion = components[components.Length - 1].Replace("'", "").Replace("\"", "");
                    }
                }
                // Detect targetSdkVersion version
                else if (HasText(eachLine, "targetSdkVersion"))
                {
                    string[] components = eachLine.Trim().Split(' ');
                    if (components.Length > 1)
                    {
                        properties.TargetSdkVersion = components[components.Length - 1];
                    }
                }
            }

            return properties;
        }

        private string GetGradleVersion(string path)
        {
            string[] lines = File.ReadAllLines(path + "/build.gradle");

            foreach (string eachLine in lines)
            {
                // Detect gradle version
                if (HasText(eachLine, "classpath", "tools.build:gradle"))
                {
                    string[] components = eachLine.Trim().Split(':');
                    if (components.Length > 2)
                    {
                        return components[2].Replace("'", "").Replace("\"", "");
                    }
                }
            }

            return "";
        }

        private void RegenerateManifestFileIfRequired(string path, GradleFileProperties properties)
        {
            if (!string.IsNullOrEmpty(properties.TargetSdkVersion))
            {
                int versionNumber;

                if (int.TryParse(properties.TargetSdkVersion, out versionNumber))
                {
                    if (versionNumber >= 30) //Starting from Android API 30, we need queries entry to query extenral packages.
                    {
                        EssentialKitSettings settings = EssentialKitSettingsEditorUtility.DefaultSettings;
                        if (settings == null)
                        {
                            return;
                        }

                        string manifestPath = string.Format("{0}/{1}/{2}", path, EssentialKitPackageLayout.AndroidProjectFolderName, "AndroidManifest.xml");
                        AndroidManifestGenerator.GenerateManifest(settings, manifestPath);

                        CheckAndroidGradlePluginVersionCompatibility(properties);
                    }
                }

            }
        }


        private void CheckAndroidGradlePluginVersionCompatibility(GradleFileProperties properties)
        {
            if (string.IsNullOrEmpty(properties.AndroidGradlePluginVersion))
            {
                Debug.LogWarning("[Harmless] Unable to find the gradle plugin version.");
                return;
            }    
            
            //https://android-developers.googleblog.com/2020/07/preparing-your-build-for-package-visibility-in-android-11.html
            Version unityPluginVersion = new Version(properties.AndroidGradlePluginVersion);

            Version[] minimumSupportedVersions = new Version[] {    new Version("3.3.3"),
                                                                    new Version("3.4.3"),
                                                                    new Version("3.5.4"),
                                                                    new Version("3.6.4"),
                                                                    new Version("4.0.1")
                                                                };

            //First check if major.minor versions are compatible. 4.1+ is always compatible, so we can ignore those.
            Version lastMinSupportedVersion = minimumSupportedVersions[minimumSupportedVersions.Length - 1];

            if((unityPluginVersion.Major >= lastMinSupportedVersion.Major) && (unityPluginVersion.Minor > lastMinSupportedVersion.Minor))
            {
                Debug.Log("Android gradle plugin version is compatible with queries tag. The build won't fail for queries tag in manifest...");
                return;
            }
            else
            {
                //Visit : https://assetstore.essentialkit.voxelbusters.com/notes/resolving-android-gradle-build-errors
                //Ref : https://docs.unity3d.com/Manual/android-gradle-overview.html
#if !UNITY_2020_1_OR_NEWER
                Debug.LogWarning(string.Format("For supporting queries tag(which is required for Android 11 support), Gradle Version needs to be 5.6.4+. To assure this, your unity's Android Gradle Version needs to be atleast nearest of 3.3.3/3.4.3/3.5.4/3.6.4/4.0.1 versions (current : {0}). For more details visit https://assetstore.essentialkit.voxelbusters.com/notes/resolving-android-gradle-build-errors", unityPluginVersion));
                //Patch if required here? => Next version based on feedback!
#endif
            }
        }


        private void EnableJetifierIfRequired(string path)
        {
            string[] files = Directory.GetFiles(UnityEngine.Application.dataPath + "/Plugins/Android" , "androidx.*.aar");

            if(files.Length > 0)
            {
                string gradlePropertiesPath = path + "/gradle.properties";

                string[] lines = File.ReadAllLines(gradlePropertiesPath);

                // Need jetifier patch process
                bool hasAndroidXProperty = lines.Any(text => text.Contains("android.useAndroidX"));
                bool hasJetifierProperty = lines.Any(text => text.Contains("android.enableJetifier"));

                StringBuilder builder = new StringBuilder();

                foreach(string each in lines)
                {
                    builder.AppendLine(each);
                }

                if (!hasAndroidXProperty)
                {
                    builder.AppendLine("android.useAndroidX=true");
                }

                if (!hasJetifierProperty)
                {
                    builder.AppendLine("android.enableJetifier=true");
                }

                File.WriteAllText(gradlePropertiesPath, builder.ToString());
            }
        }

        private void PatchGradleForPackingOptions(string path)
        {
            Debug.Log("Patching gradle for packing options at path : " + path);

            // First read the main build.gradle file
            string gradleFilePath = path + Path.DirectorySeparatorChar + "build.gradle";
            List<string> lines = File.ReadAllLines(gradleFilePath).ToList();

            int index = -1;
            for(int i=0; i<lines.Count; i++)
            {
                if (HasText(lines[i], "apply plugin: 'com.android.application'"))
                {
                    index = i;
                    break;
                }
            }

            if(index != -1)
            {
                string textToInsert = @"android {
     packagingOptions {
         exclude 'META-INF/proguard/androidx-annotations.pro'
     }
 }";
                
                lines.Insert(index + 1, textToInsert);

                File.WriteAllLines(gradleFilePath, lines);
            }
        }
    }
}
#endif