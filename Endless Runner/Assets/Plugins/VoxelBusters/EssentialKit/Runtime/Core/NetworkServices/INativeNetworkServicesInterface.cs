using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit.NetworkServicesCore
{
    public interface INativeNetworkServicesInterface : INativeFeatureInterface
    {
        #region Events

        event InternetConnectivityChangeInternalCallback OnInternetConnectivityChange;

        event HostReachabilityChangeInternalCallback OnHostReachabilityChange;

        #endregion

        #region Methods

        void StartNotifier();

        void StopNotifier();

        #endregion
    }
}