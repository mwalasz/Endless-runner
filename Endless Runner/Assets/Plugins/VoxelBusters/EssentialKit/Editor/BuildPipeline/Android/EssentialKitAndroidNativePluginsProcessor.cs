#if UNITY_EDITOR && UNITY_ANDROID
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Text;
using System.Linq;
using UnityEngine;
using UnityEditor;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.Editor;
using VoxelBusters.CoreLibrary.Editor.NativePlugins.Build;


namespace VoxelBusters.EssentialKit.Editor.Build.Android
{
    public class EssentialKitAndroidNativePluginsProcessor : VoxelBusters.CoreLibrary.Editor.NativePlugins.Build.Android.AndroidNativePluginsProcessor
    {
#region Properties

        private EssentialKitSettings Settings { get; set; }

#endregion

#region Overriden methods

        public override void OnCheckConfiguration()
        {
            if (!EnsureInitialized())
                return;

            StringBuilder builder = new StringBuilder();

            if (builder.Length != 0)
            {
                string error = string.Format("[VoxelBusters : {0}] \n{1}", EssentialKitSettings.DisplayName, builder.ToString());
                if (SettingsPropertyGroup.CanToggleFeatureUsageState())
                {
                    Debug.LogError(error, Settings);
                }
                else
                {
                    Debug.LogWarning(error, Settings);
                }
            }
        }

        public override void OnUpdateLinkXml(LinkXmlWriter writer)
        {
            if (!EnsureInitialized())
                return;

            // Add active configurations
            var usedFeaturesMap = ImplementationSchema.GetAllRuntimeConfigurations(settings: Settings);
            var platform = EditorApplicationUtility.ConvertBuildTargetToRuntimePlatform(BuildTarget);
            foreach (var current in usedFeaturesMap)
            {
                string name = current.Key;
                var config = current.Value;
                writer.AddConfiguration(name, config, platform, useFallbackPackage: !Settings.IsFeatureUsed(name));
            }
        }

        public override void OnAddFiles()
        {
            if (!EnsureInitialized())
                return;


            // generate manifest
            AndroidManifestGenerator.GenerateManifest(Settings);

            // generate dependencies
            AndroidLibraryDependenciesGenerator.CreateLibraryDependencies();

        }



        public override void OnAddFolders()
        {
            if (!EnsureInitialized())
                return;
        }

        public override void OnAddResources()
        {
            if (!EnsureInitialized())
                return;

            UpdateStringsXml();
        }

        public override void OnUpdateConfiguration()
        {
            if (!EnsureInitialized())
                return;
        }

#endregion

#region Helper methods

        private bool EnsureInitialized()
        {
            if (Settings != null) return true;

            if (EssentialKitSettingsEditorUtility.TryGetDefaultSettings(out EssentialKitSettings settings))
            {
                Settings = settings;
                return true;
            }

            return false;
        }


        private void UpdateStringsXml()
        {
            Dictionary<string, string> config = new Dictionary<string, string>();

            if (Settings.AddressBookSettings.IsEnabled)
            {
                config.Add("ADDRESS_BOOK_PERMISSON_REASON", Settings.ApplicationSettings.UsagePermissionSettings.AddressBookUsagePermission.GetDescriptionForActivePlatform());
            }
            
            XmlDocument xml = new XmlDocument();
            xml.Load(EssentialKitPackageLayout.AndroidProjectResValuesStringsPath);
            XmlNodeList nodes = xml.SelectNodes("/resources/string");

            foreach (XmlNode node in nodes)
            {
                XmlAttribute xmlAttribute = node.Attributes["name"];
                string key = xmlAttribute.Value;

                if (config.ContainsKey(key))
                {
                    node.InnerText = config[key];
                }
            }

            xml.Save(EssentialKitPackageLayout.AndroidProjectResValuesStringsPath);
        }

#endregion
    }
}
#endif