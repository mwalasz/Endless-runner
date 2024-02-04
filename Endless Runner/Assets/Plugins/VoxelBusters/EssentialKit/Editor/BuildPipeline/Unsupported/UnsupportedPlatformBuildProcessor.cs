using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.Editor;
using VoxelBusters.CoreLibrary.Editor.NativePlugins.Build;

namespace VoxelBusters.EssentialKit.Editor.Build
{
    public class UnsupportedPlatformNativePluginsProcessor : CoreLibrary.Editor.NativePlugins.Build.UnsupportedPlatformNativePluginsProcessor
    {
        #region Properties

        private EssentialKitSettings Settings { get; set; }

        #endregion

        #region Base class methods

        public override void OnUpdateLinkXml(LinkXmlWriter writer)
        {
            // Check whether plugin is configured
            if (!EnsureInitialized()) return;

            // Add active configurations
            var     usedFeaturesMap = ImplementationSchema.GetAllRuntimeConfigurations(settings: Settings);
            var     platform        = EditorApplicationUtility.ConvertBuildTargetToRuntimePlatform(BuildTarget);
            foreach (var current in usedFeaturesMap)
            {
                string  name        = current.Key;
                var     config      = current.Value;
                writer.AddConfiguration(name, config, platform, useFallbackPackage: !Settings.IsFeatureUsed(name));
            }
        }

        #endregion

        #region Private methods

        private bool EnsureInitialized()
        {
            if (Settings != null) return true;

            if (EssentialKitSettingsEditorUtility.TryGetDefaultSettings(out EssentialKitSettings settings))
            {
                Settings    = settings;
                return true;
            }

            return false;
        }

        #endregion
    }
}