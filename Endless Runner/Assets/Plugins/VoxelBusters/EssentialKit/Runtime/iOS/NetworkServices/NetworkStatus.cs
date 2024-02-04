#if UNITY_IOS || UNITY_TVOS
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.EssentialKit.NetworkServicesCore.iOS
{
    internal enum NetworkStatus : long
    {
        NotReachable = 0,

        ReachableViaWiFi,

        ReachableViaWWAN,
    }
}
#endif