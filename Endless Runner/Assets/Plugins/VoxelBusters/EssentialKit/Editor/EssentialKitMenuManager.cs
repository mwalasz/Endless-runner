using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.Editor;

namespace VoxelBusters.EssentialKit.Editor
{
    public static class EssentialKitMenuManager
    {
        #region Constants

        private const string kMenuItemPath = "Window/Voxel Busters/Essential Kit";

        #endregion

        #region Menu items

        [MenuItem(kMenuItemPath + "/Open Settings")]
        public static void OpenSettings()
        {
            EssentialKitSettingsEditorUtility.OpenInProjectSettings();
        }

        /*[MenuItem(kMenuItemPath + "/Import Settings")]
        public static void ImportSettings()
        {
            var     settings        = UpgradeUtility.ImportSettings();

            // save
            EditorUtility.SetDirty(settings);
            AssetDatabase.SaveAssets();

            // ping this object
            Selection.activeObject  = settings;
            EditorGUIUtility.PingObject(settings);
        }*/

#if NATIVE_PLUGINS_SHOW_UPM_MIGRATION
        [MenuItem(kMenuItemPath + "/Migrate To UPM")]
        public static void MigrateToUpm()
        {
            EssentialKitSettings.Package.MigrateToUpm();
        }

        [MenuItem(kMenuItemPath + "/Migrate To UPM", isValidateFunction: true)]
        private static bool ValidateMigrateToUpm()
        {
            return EssentialKitSettings.Package.IsInstalledWithinAssets();
        }
#endif

        [MenuItem(kMenuItemPath + "/Uninstall")]
        public static void Uninstall()
        {
            UninstallPlugin.Uninstall();
        }

#endregion
    }
}