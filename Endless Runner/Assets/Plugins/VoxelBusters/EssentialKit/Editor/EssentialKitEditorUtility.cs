using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins;
using VoxelBusters.CoreLibrary.Editor;

namespace VoxelBusters.EssentialKit.Editor
{
    public class EssentialKitEditorUtility
    {
        #region Constants

        private     const   string      kLiteVersionProductPage                 = "http://bit.ly/essentialkitliteunity";
        
        private     const   string      kFullVersionProductPage                 = "http://bit.ly/essentialkitunity";

        private     const   string      kPublisherPage                          = "http://bit.ly/ustoreVB";

        private     const   string      kDocumentationPage                      = "http://bit.ly/essentialkitdocumentation";

        private     const   string      kForumPage                              = "http://bit.ly/essentialkitforum";

        private     const   string      kTutorialPage                           = "http://bit.ly/essentialkittutorials";

        private     const   string      kDiscordPage                            = "https://discord.gg/y4kQAefbJ8";

        private     const   string      kDefaultSettingsAssetOldPath            = "Assets/Plugins/VoxelBusters/EssentialKit/Resources/EssentialKitSettings.asset";

        #endregion

        #region Public methods

        public static void ImportEssentialResources()
        {
            var     targetPackages  = new string[]
            {
                $"{CoreLibrarySettings.Package.GetPackageResourcesPath()}/PackageResources/Essentials.unitypackage",
                $"{EssentialKitSettings.Package.GetPackageResourcesPath()}/PackageResources/Essentials.unitypackage",
            };
            ImportPackages(targetPackages);
        }

        /*
        public static void ImportExtraResources()
        {
            var     targetPackages  = new string[]
            {
                $"{EssentialKitSettings.Package.GetPackageResourcesPath()}/PackageResources/Extras.unitypackage",
            };
            ImportPackages(targetPackages);
        }
        */

        #endregion

        #region Web action methods

        public static void OpenAssetStorePage(bool fullVersion)
        {
            Application.OpenURL(fullVersion ? kFullVersionProductPage : kLiteVersionProductPage);
        }

        public static void OpenPublisherPage()
        {
            Application.OpenURL(kPublisherPage);
        }

        public static void OpenDocumentation()
        {
            Application.OpenURL(kDocumentationPage);
        }

        public static void OpenForum()
        {
            Application.OpenURL(kForumPage);
        }

        public static void OpenTutorials()
        {
            Application.OpenURL(kTutorialPage);
        }

        public static void OpenSupport()
        {
            Application.OpenURL(kDiscordPage);
        }

        #endregion

        #region Private methods

        private static void RegisterForImportPackageCallbacks()
        {
            AssetDatabase.importPackageStarted     += OnImportPackageStarted;
            AssetDatabase.importPackageCompleted   += OnImportPackageCompleted;
            AssetDatabase.importPackageCancelled   += OnImportPackageCancelled;
            AssetDatabase.importPackageFailed      += OnImportPackageFailed;
        }

        private static void UnregisterFromImportPackageCallbacks()
        {
            AssetDatabase.importPackageStarted     -= OnImportPackageStarted;
            AssetDatabase.importPackageCompleted   -= OnImportPackageCompleted;
            AssetDatabase.importPackageCancelled   -= OnImportPackageCancelled;
            AssetDatabase.importPackageFailed      -= OnImportPackageFailed;
        }

        private static void ImportPackages(string[] packages)
        {
            RegisterForImportPackageCallbacks();
            foreach (var package in packages)
            {
                AssetDatabase.ImportPackage(package, false);
            }
            UnregisterFromImportPackageCallbacks();
        }

        private static bool IsFileStructureOutdated()
        {
            return IOServices.FileExists(kDefaultSettingsAssetOldPath);
        }

        private static void MigrateToNewFileStructure()
        {
            if (!AssetDatabase.IsValidFolder("Assets/Resources"))
            {
                AssetDatabase.CreateFolder("Assets", "Resources");
            }
            AssetDatabase.MoveAsset(kDefaultSettingsAssetOldPath, EssentialKitSettings.DefaultSettingsAssetPath);
        }

        #endregion

        #region Callback methods

        [InitializeOnLoadMethod]
        private static void PostProcessPackage()
        {
            EditorApplication.delayCall += () =>
            {
                if (IsFileStructureOutdated())
                {
                    MigrateToNewFileStructure();
                }
            };
        }

        private static void OnImportPackageStarted(string packageName)
        {
            Debug.Log($"[UnityPackageUtility] Started importing package: {packageName}");
        }

        private static void OnImportPackageCompleted(string packageName)
        {
            Debug.Log($"[UnityPackageUtility] Imported package: {packageName}");
        }

        private static void OnImportPackageCancelled(string packageName)
        {
            Debug.Log($"[UnityPackageUtility] Cancelled the import of package: {packageName}");
        }

        private static void OnImportPackageFailed(string packageName, string errorMessage)
        {
            Debug.Log($"[UnityPackageUtility] Failed importing package: {packageName} with error: {errorMessage}");
        }

        #endregion
    }
}