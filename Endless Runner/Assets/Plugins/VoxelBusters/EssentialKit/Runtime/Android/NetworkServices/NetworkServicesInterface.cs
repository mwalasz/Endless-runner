#if UNITY_ANDROID

using System;
using VoxelBusters.CoreLibrary.NativePlugins.Android;

namespace VoxelBusters.EssentialKit.NetworkServicesCore.Android
{
    public sealed class NetworkServicesInterface : NativeNetworkServicesInterfaceBase, INativeNetworkServicesInterface
    {
        #region Static fields

        private NativeNetworkConnectivity m_instance;

        #endregion

        #region Constructors

        public NetworkServicesInterface() 
            : base(isAvailable: true)
        {
            m_instance = new NativeNetworkConnectivity(NativeUnityPluginUtility.GetContext());
            m_instance.SetListener(new NativeNetworkChangeListener()
            {
                onChangeCallback = (bool isConnected) =>
                {
                    SendInternetConnectivityChangeEvent(isConnected);
                    SendHostReachabilityChangeEvent(isConnected);
                    //Check : Provide separate internet and host reachabilities
                }
            });
        }

        #endregion

        #region Base class methods

        public override void StartNotifier()
        {
            NetworkServicesUnitySettings settings = NetworkServices.UnitySettings;
            NativeNetworkPollSettings nativeNetworkPollSettings = new NativeNetworkPollSettings();
            nativeNetworkPollSettings.SetIpAddress(settings.HostAddress.IpV4);
            nativeNetworkPollSettings.SetMaxRetryCount(settings.PingSettings.MaxRetryCount);
            nativeNetworkPollSettings.SetPortNumber(settings.PingSettings.Port);//53 for DNS, 80 for normal
            nativeNetworkPollSettings.SetTimeGapBetweenPolls(settings.PingSettings.TimeGapBetweenPolling);
            nativeNetworkPollSettings.SetConnectionTimeOutPeriod((long)Math.Round(settings.PingSettings.TimeOutPeriod));

            m_instance.Start(nativeNetworkPollSettings);
        }

        public override void StopNotifier()
        {
            m_instance.Stop();
        }

        #endregion
    }
}
#endif