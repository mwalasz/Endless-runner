using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using VoxelBusters.CoreLibrary.NativePlugins;
using VoxelBusters.CoreLibrary.Editor;
using VoxelBusters.CoreLibrary.Editor.NativePlugins.UnityUI;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.EssentialKit.Editor
{
    [InitializeOnLoad]
    public static class EssentialKitSettingsEditorUtility
    {
        #region Constants

        private     const       string                      kLocalPathInProjectSettings     = "Project/Voxel Busters/Essential Kit";

        #endregion

        #region Static fields

        private     static      EssentialKitSettings        s_defaultSettings;

        #endregion

        #region Static properties

        public static EssentialKitSettings DefaultSettings
        {
            get
            {
                if (s_defaultSettings == null)
                {
                    var     instance    = LoadDefaultSettingsObject(throwError: false);
                    if (null == instance)
                    {
                        instance        = CreateDefaultSettingsObject();
                    }
                    s_defaultSettings   = instance;
                }
                return s_defaultSettings;
            }
            set
            {
                Assert.IsPropertyNotNull(value, nameof(value));

                // set new value
                s_defaultSettings       = value;
            }
        }

        public static bool SettingsExists
        {
            get
            {
                if (s_defaultSettings == null)
                {
                    s_defaultSettings   = LoadDefaultSettingsObject(throwError: false);
                }
                return (s_defaultSettings != null);
            }
        }

        #endregion

        #region Constructors

        static EssentialKitSettingsEditorUtility()
        {
            AddGlobalDefines();
        }

        #endregion

        #region Static methods

        public static void ShowSettingsNotFoundErrorDialog()
        {
            EditorUtility.DisplayDialog(
                title: "Error",
                message: "Native plugins is not configured. Please select plugin settings file from menu and configure it according to your preference.",
                ok: "Ok");
        }

        public static bool TryGetDefaultSettings(out EssentialKitSettings defaultSettings)
        {
            // Set default value
            defaultSettings     = null;

            // Set reference if the object exists
            if (SettingsExists)
            {
                defaultSettings = DefaultSettings;
                return true;
            }
            return false;
        }

        public static void AddGlobalDefines()
        {
            ScriptingDefinesManager.AddDefine("ENABLE_VOXELBUSTERS_ESSENTIAL_KIT");
        }
        
        public static void RemoveGlobalDefines()
        {
            ScriptingDefinesManager.RemoveDefine("ENABLE_VOXELBUSTERS_ESSENTIAL_KIT");
        }

        public static void OpenInProjectSettings()
        {
            if (!SettingsExists)
            {
                CreateDefaultSettingsObject();
            }
            Selection.activeObject  = null;
            SettingsService.OpenProjectSettings(kLocalPathInProjectSettings);
        }

        [SettingsProvider]
        private static SettingsProvider CreateSettingsProvider()
        {
            return SettingsProviderZ.Create(
                settingsObject: DefaultSettings,
                path: kLocalPathInProjectSettings,
                scopes: SettingsScope.Project);
        }

        #endregion

        #region Private static methods

        private static EssentialKitSettings CreateDefaultSettingsObject()
        {
            return AssetDatabaseUtility.CreateScriptableObject<EssentialKitSettings>(
                assetPath: EssentialKitSettings.DefaultSettingsAssetPath,
                onInit: (instance)  => SetDefaultProperties(instance));
        }

        private static EssentialKitSettings LoadDefaultSettingsObject(bool throwError = true)
        {
            var     throwErrorFunc      = throwError ? () => Diagnostics.PluginNotConfiguredException() : (System.Func<System.Exception>)null;
            return AssetDatabaseUtility.LoadScriptableObject<EssentialKitSettings>(
                assetPath: EssentialKitSettings.DefaultSettingsAssetPath,
                onLoad: (instance)  => SetDefaultProperties(instance),
                throwErrorFunc: throwErrorFunc);
        }

        private static void SetDefaultProperties(EssentialKitSettings settings)
        {
            // set properties
            var     uiCollection        = settings.NativeUISettings.CustomUICollection;
            if (uiCollection.RendererPrefab == null)
            {
                uiCollection.RendererPrefab     = UnityUIEditorUtility.LoadRendererPrefab();
            }
            if (uiCollection.AlertDialogPrefab == null)
            {
                uiCollection.AlertDialogPrefab  = UnityUIEditorUtility.LoadAlertDialogPrefab();
            }
        }

        #endregion
    }
}