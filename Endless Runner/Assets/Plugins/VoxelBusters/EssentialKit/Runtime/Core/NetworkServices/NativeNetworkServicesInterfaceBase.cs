using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit.NetworkServicesCore
{
    public abstract class NativeNetworkServicesInterfaceBase : NativeFeatureInterfaceBase, INativeNetworkServicesInterface
    {
        #region Constructors

        protected NativeNetworkServicesInterfaceBase(bool isAvailable)
            : base(isAvailable)
        { }

        #endregion

        #region INativeNetworkServicesInterface implementation

        public event InternetConnectivityChangeInternalCallback OnInternetConnectivityChange;

        public event HostReachabilityChangeInternalCallback OnHostReachabilityChange;

        public abstract void StartNotifier();

        public abstract void StopNotifier();

        #endregion

        #region Private methods

        protected void SendInternetConnectivityChangeEvent(bool isConnected)
        {
            CallbackDispatcher.InvokeOnMainThread(() => OnInternetConnectivityChange(isConnected));
        }

        protected void SendHostReachabilityChangeEvent(bool isReachable)
        {
            CallbackDispatcher.InvokeOnMainThread(() => OnHostReachabilityChange(isReachable));
        }

        #endregion
    }
}