using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace VoxelBusters.CoreLibrary.Editor.Experimental
{
    public class ProxySettingsProvider : SettingsProvider
    {
        #region Constants

        private     const       string      kDefaultInstallMessageFormat        = "In order to use {0} system you need to install the {0} package. Clicking the button below will install {0} package and allow you to configure.";

        private     const       string      kDefaultProjectSettingsPathFormat   = "Project/Voxel Busters/{0}";            

        #endregion

        #region Fields

        private     string      m_installMessage;

        private     string      m_installButtonLabel;

        private     string      m_installUrl;

        #endregion

        #region Constructors

        public ProxySettingsProvider(string name,
                                     string installUrl,
                                     string path = null,
                                     SettingsScope scopes = SettingsScope.Project)
            : base(path ?? string.Format(kDefaultProjectSettingsPathFormat, name), scopes)
        {
            // set properties
            m_installMessage        = string.Format(kDefaultInstallMessageFormat, name);
            m_installButtonLabel    = $"Install {name}";
            m_installUrl            = installUrl;
        }

        #endregion

        #region Create methods

#if !ENABLE_VOXELBUSTERS_ESSENTIAL_KIT
        [SettingsProvider]
        private static SettingsProvider CreateEssentialKitSettingsProvider()
        {
            return new ProxySettingsProvider(name: "Essential Kit",
                                             installUrl: InstallPath.EssentialKit);
        }
#endif

#if !ENABLE_VOXELBUSTERS_SCREEN_RECORDER_KIT
        [SettingsProvider]
        private static SettingsProvider CreateScreenRecorderKitSettingsProvider()
        {
            return new ProxySettingsProvider(name: "Screen Recorder Kit",
                                             installUrl: InstallPath.ScreenRecorderKit);
        }
#endif

#if !ENABLE_VOXELBUSTERS_SOCIAL_KIT
        [SettingsProvider]
        private static SettingsProvider CreateSocialKitSettingsProvider()
        {
            return new ProxySettingsProvider(name: "Social Kit",
                                             installUrl: InstallPath.SocialKit);
        }
#endif

#if !ENABLE_VOXELBUSTERS_ML_KIT
        [SettingsProvider]
        private static SettingsProvider CreateMLKitSettingsProvider()
        {
            return new ProxySettingsProvider(name: "ML Kit",
                                             installUrl: InstallPath.MLKit);
        }
#endif

#if !ENABLE_VOXELBUSTERS_REPORTING_KIT
        [SettingsProvider]
        private static SettingsProvider CreateReportingKitSettingsProvider()
        {
            return new ProxySettingsProvider(name: "Reporting Kit",
                                             installUrl: InstallPath.ReportingKit);
        }
#endif

#if !ENABLE_VOXELBUSTERS_ADS_KIT
        [SettingsProvider]
        private static SettingsProvider CreateAdsKitSettingsProvider()
        {
            return new ProxySettingsProvider(name: "Ads Kit",
                                             installUrl: InstallPath.AdsKit);
        }
#endif

        #endregion

        #region Base class methods

        public override void OnGUI(string searchContext)
        {
            EditorGUILayout.HelpBox(m_installMessage, MessageType.Info);
            if (GUILayout.Button(m_installButtonLabel))
            {
                Application.OpenURL(m_installUrl);
            }
        }

        #endregion
    }
}