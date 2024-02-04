using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.EssentialKit.NetworkServicesCore
{
    public delegate void InternetConnectivityChangeInternalCallback(bool isConnected);

    public delegate void HostReachabilityChangeInternalCallback(bool isReachable);
}