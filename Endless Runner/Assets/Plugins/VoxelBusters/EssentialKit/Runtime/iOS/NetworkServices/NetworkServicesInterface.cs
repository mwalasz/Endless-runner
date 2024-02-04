#if UNITY_IOS || UNITY_TVOS
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AOT;
using VoxelBusters.EssentialKit;

namespace VoxelBusters.EssentialKit.NetworkServicesCore.iOS
{
    public sealed class NetworkServicesInterface : NativeNetworkServicesInterfaceBase, INativeNetworkServicesInterface
    {
        #region Static fields

        private static  bool                        s_initialised   = false;

        private static  NetworkServicesInterface    s_instance      = null;

        #endregion

        #region Constructors

        public NetworkServicesInterface() 
            : base(isAvailable: true)
        {
            if (!s_initialised)
            {
                s_initialised       = true;

                NetworkServicesBinding.NPNetworkServicesRegisterCallbacks(HandleInternetReachabilityChangeNativeCallback, HandleHostReachabilityChangeNativeCallback);
                            
                string  hostAddress = NetworkServices.UnitySettings.HostAddress.IpV6;
                NetworkServicesBinding.NPNetworkServicesInit(hostAddress);
            }

            // save reference
            s_instance  = this;
        }

        #endregion

        #region Base class methods

        public override void StartNotifier()
        { 
            NetworkServicesBinding.NPNetworkServicesStartReachabilityNotifier();
        }

        public override void StopNotifier()
        { 
            NetworkServicesBinding.NPNetworkServicesStopReachabilityNotifier();
        }

        #endregion

        #region Native callback methods

        [MonoPInvokeCallback(typeof(ReachabilityChangeNativeCallback))]
        private static void HandleInternetReachabilityChangeNativeCallback(bool isReachable, NetworkStatus networkStatus)
        {
            s_instance.SendInternetConnectivityChangeEvent(isReachable);
        }

        [MonoPInvokeCallback(typeof(ReachabilityChangeNativeCallback))]
        private static void HandleHostReachabilityChangeNativeCallback(bool isReachable, NetworkStatus networkStatus)
        {
            s_instance.SendHostReachabilityChangeEvent(isReachable);
        }

        #endregion
    }
}
#endif