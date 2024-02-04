#if UNITY_IOS || UNITY_TVOS
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.EssentialKit;

namespace VoxelBusters.EssentialKit.NetworkServicesCore.iOS
{
    internal delegate void ReachabilityChangeNativeCallback(bool isReachable, NetworkStatus networkStatus);
}
#endif