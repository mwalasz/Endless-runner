using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.EssentialKit
{
    [Serializable]
    public partial class NetworkServicesUnitySettings : SettingsPropertyGroup
    {
        #region Fields

        [SerializeField]
        [Tooltip("Host address.")]
        private     Address             m_hostAddress;

        [SerializeField]
        [Tooltip("If enabled, rechability trackers are activated on launch.")]
        private     bool                m_autoStartNotifier;

        [SerializeField]
        [Tooltip("Ping test configuration.")]
        private     PingTestSettings    m_pingSettings;

        #endregion

        #region Properties

        public Address HostAddress => m_hostAddress;

        public bool AutoStartNotifier => m_autoStartNotifier;

        public PingTestSettings PingSettings => m_pingSettings;

        #endregion

        #region Constructors

        public NetworkServicesUnitySettings(bool isEnabled = true, Address hostAddress = null,
            bool autoStartNotifier = true, PingTestSettings pingSettings = null)
            : base(isEnabled: isEnabled, name: NativeFeatureType.kNetworkServices)
        {
            // set properties
            m_hostAddress       = hostAddress ?? new Address();
            m_autoStartNotifier = autoStartNotifier;
            m_pingSettings      = pingSettings ?? new PingTestSettings();
        }

        #endregion
    }
}