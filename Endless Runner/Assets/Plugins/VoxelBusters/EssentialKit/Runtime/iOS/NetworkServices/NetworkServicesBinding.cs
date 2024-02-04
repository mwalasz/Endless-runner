#if UNITY_IOS || UNITY_TVOS
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace VoxelBusters.EssentialKit.NetworkServicesCore.iOS
{
    internal static class NetworkServicesBinding
    {
        [DllImport("__Internal")]
        public static extern void NPNetworkServicesRegisterCallbacks(ReachabilityChangeNativeCallback internetReachabilityChangeCallback, ReachabilityChangeNativeCallback hostReachabilityChangeCallback);

        [DllImport("__Internal")]   
        public static extern void NPNetworkServicesInit(string hostAddress);

        [DllImport("__Internal")]
        public static extern void NPNetworkServicesStartReachabilityNotifier();

        [DllImport("__Internal")]
        public static extern void NPNetworkServicesStopReachabilityNotifier();

        [DllImport("__Internal")]
        public static extern NetworkStatus NPNetworkServicesGetInternetReachabilityStatus();

        [DllImport("__Internal")]
        public static extern NetworkStatus NPNetworkServicesGetHostReachabilityStatus();
    }
}
#endif