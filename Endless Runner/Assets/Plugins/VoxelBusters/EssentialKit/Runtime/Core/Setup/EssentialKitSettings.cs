using System;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit
{
    public class EssentialKitDomain
    {
        public static string Default => "VoxelBusters.CoreLibrary.EssentialKit";
    }

    public class EssentialKitSettings : SettingsObject
    {
        #region Static fields

        [ClearOnReload]
        private     static      EssentialKitSettings    s_sharedInstance;

        [ClearOnReload]
        private     static      UnityPackageDefinition  s_package;

        #endregion

        #region Fields

        [SerializeField]
        private     ApplicationSettings                 m_applicationSettings           = new ApplicationSettings();

        [SerializeField]
        private     AddressBookUnitySettings            m_addressBookSettings           = new AddressBookUnitySettings();

        [SerializeField]
        private     NativeUIUnitySettings               m_nativeUISettings              = new NativeUIUnitySettings();

        [SerializeField]
        private     SharingServicesUnitySettings        m_sharingServicesSettings       = new SharingServicesUnitySettings();

        
        [SerializeField]
        private     NetworkServicesUnitySettings        m_networkServicesSettings       = new NetworkServicesUnitySettings();

        [SerializeField]
        private     UtilityUnitySettings                m_utilitySettings               = new UtilityUnitySettings();

        #endregion

        #region Static properties

        internal static UnityPackageDefinition Package => ObjectHelper.CreateInstanceIfNull(
            ref s_package,
            () =>
            {
                return new UnityPackageDefinition(name: "com.voxelbusters.essentialkit",
                                                  displayName: "Essential Kit - Free Version",
                                                  version: "2.7.1",
                                                  defaultInstallPath: $"Assets/Plugins/VoxelBusters/EssentialKit",
                                                  dependencies: CoreLibrarySettings.Package);
            });

        public static string PackageName => Package.Name;

        public static string DisplayName => Package.DisplayName;

        public static string Version => Package.Version;

        public static string DefaultSettingsAssetName => "EssentialKitSettings";

        public static string DefaultSettingsAssetPath => $"{Package.GetMutableResourcesPath()}/{DefaultSettingsAssetName}.asset";

        public static string PersistentDataPath => Package.PersistentDataPath;

        public static EssentialKitSettings Instance => GetSharedInstanceInternal();

        #endregion

        #region Properties

        public ApplicationSettings ApplicationSettings
        {
            get => m_applicationSettings;
            set => m_applicationSettings = value;
        }

        public AddressBookUnitySettings AddressBookSettings
        {
            get => m_addressBookSettings;
            set => m_addressBookSettings = value;
        }

        public NativeUIUnitySettings NativeUISettings
        {
            get => m_nativeUISettings;
            set => m_nativeUISettings = value;
        }

        public SharingServicesUnitySettings SharingServicesSettings
        {
            get => m_sharingServicesSettings;
            set => m_sharingServicesSettings = value;
        }

        
        public NetworkServicesUnitySettings NetworkServicesSettings
        {
            get => m_networkServicesSettings;
            set => m_networkServicesSettings = value;
        }

        

        public UtilityUnitySettings UtilitySettings
        {
            get => m_utilitySettings;
            set => m_utilitySettings = value;
        }

        #endregion

        #region Static methods

        public static void SetSettings(EssentialKitSettings settings)
        {
            Assert.IsArgNotNull(settings, nameof(settings));

            // set properties
            s_sharedInstance    = settings;
        }

        private static EssentialKitSettings GetSharedInstanceInternal(bool throwError = true)
        {
            if (null == s_sharedInstance)
            {
                // check whether we are accessing in edit or play mode
                var     assetPath   = DefaultSettingsAssetName;
                var     settings    = Resources.Load<EssentialKitSettings>(assetPath);
                if (throwError && (null == settings))
                {
                    throw Diagnostics.PluginNotConfiguredException("Essential Kit");
                }

                // store reference
                s_sharedInstance = settings;
            }

            return s_sharedInstance;
        }

        #endregion

        #region Base class methods

        protected override void UpdateLoggerSettings()
        {
            DebugLogger.SetLogLevel(ApplicationSettings.LogLevel, EssentialKitDomain.Default);
        }

        #endregion

        #region Private methods

        public string[] GetAvailableFeatureNames()
        {
            return new string[]
            {
                NativeFeatureType.kAddressBook,
                NativeFeatureType.kNativeUI,
                NativeFeatureType.kNetworkServices,
                NativeFeatureType.kSharingServices,
                NativeFeatureType.kExtras
            };
        }

        public string[] GetUsedFeatureNames()
        {
            var     usedFeatures    = new List<string>();
            if (m_addressBookSettings.IsEnabled)
            {
                usedFeatures.Add(NativeFeatureType.kAddressBook);
            }
            if (m_nativeUISettings.IsEnabled)
            {
                usedFeatures.Add(NativeFeatureType.kNativeUI);
            }
            if (m_networkServicesSettings.IsEnabled)
            {
                usedFeatures.Add(NativeFeatureType.kNetworkServices);
            }
            if (m_sharingServicesSettings.IsEnabled)
            {
                usedFeatures.Add(NativeFeatureType.kSharingServices);
            }
            if ((usedFeatures.Count > 0) || (m_applicationSettings.RateMyAppSettings.IsEnabled))
            {
                usedFeatures.Add(NativeFeatureType.kNativeUI);//Required for showing confirmation dialog
            }
            if (m_utilitySettings.IsEnabled || (m_applicationSettings.RateMyAppSettings.IsEnabled))
            {
                usedFeatures.Add(NativeFeatureType.kExtras);
            }

            return usedFeatures.ToArray();
        }

        public bool IsFeatureUsed(string name)
        {
            return Array.Exists(GetUsedFeatureNames(), (item) => string.Equals(item, name));
        }

        #endregion
    }
}