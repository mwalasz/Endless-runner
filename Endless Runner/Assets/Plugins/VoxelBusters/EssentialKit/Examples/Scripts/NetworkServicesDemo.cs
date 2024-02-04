using System.Text;
using UnityEngine;
using UnityEngine.UI;
// key namespaces
using VoxelBusters.CoreLibrary.NativePlugins;
using VoxelBusters.EssentialKit;
// internal namespace
using VoxelBusters.CoreLibrary.NativePlugins.DemoKit;

namespace VoxelBusters.EssentialKit.Demo
{
    public class NetworkServicesDemo : DemoActionPanelBase<NetworkServicesDemoAction, NetworkServicesDemoActionType>
    {
        #region Base class methods

        protected override void OnEnable()
        {
            base.OnEnable();

            // register for events
            NetworkServices.OnHostReachabilityChange        += OnHostReachabilityChange;
            NetworkServices.OnInternetConnectivityChange    += OnInternetConnectivityChange;
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            // unregister from events
            NetworkServices.OnHostReachabilityChange        -= OnHostReachabilityChange;
            NetworkServices.OnInternetConnectivityChange    -= OnInternetConnectivityChange;
        }

        protected override void OnActionSelectInternal(NetworkServicesDemoAction selectedAction)
        {
            switch (selectedAction.ActionType)
            {
                case NetworkServicesDemoActionType.IsInternetActive:
                    Log("Internet connectivity status: " + NetworkServices.IsInternetActive);
                    break;

                case NetworkServicesDemoActionType.IsHostReachable:
                    Log("Host reachability status: " + NetworkServices.IsHostReachable);
                    break;

                case NetworkServicesDemoActionType.IsNotifierActive:
                    Log("Is notifier active: " + NetworkServices.IsNotifierActive);
                    break;

                case NetworkServicesDemoActionType.StartNotifier:
                    NetworkServices.StartNotifier();
                    Log("Notifier started successfully.");
                    break;

                case NetworkServicesDemoActionType.StopNotifier:
                    NetworkServices.StopNotifier();
                    Log("Notifier stopped.");
                    break;

                case NetworkServicesDemoActionType.ResourcePage:
                    ProductResources.OpenResourcePage(NativeFeatureType.kNetworkServices);
                    break;
            }
        }

        #endregion

        #region Plugin event callback methods
        
        private void OnInternetConnectivityChange(NetworkServicesInternetConnectivityStatusChangeResult result)
        {
            Log("Received internet connectivity changed event.");
            Log("Internet connectivity status: " + result.IsConnected);
        }

        private void OnHostReachabilityChange(NetworkServicesHostReachabilityStatusChangeResult result)
        {
            Log("Received host reachability changed event.");
            Log("Host reachability status: " + result.IsReachable);
        }

        #endregion
    }
}
