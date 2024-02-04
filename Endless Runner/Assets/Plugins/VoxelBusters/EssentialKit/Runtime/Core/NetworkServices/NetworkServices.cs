using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.EssentialKit.NetworkServicesCore;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit
{
    /// <summary>
    /// Provides cross-platform interface to check network connectivity status.
    /// </summary>
    /// <example>
    /// The following example illustrates how to use network service related events.
    /// <code>
    /// using UnityEngine;
    /// using System.Collections;
    /// using VoxelBusters.EssentialKit;
    /// 
    /// public class ExampleClass : MonoBehaviour 
    /// {
    ///     private void OnEnable()
    ///     {
    ///         // registering for event
    ///         NetworkServices.OnInternetConnectivityChange    += OnInternetConnectivityChange;
    ///         NetworkServices.OnHostReachabilityChange        += OnHostReachabilityChange;
    ///     }
    /// 
    ///     private void OnDisable()
    ///     {
    ///         // unregistering event
    ///         NetworkServices.OnInternetConnectivityChange    -= OnInternetConnectivityChange;
    ///         NetworkServices.OnHostReachabilityChange        -= OnHostReachabilityChange;
    ///     }
    /// 
    ///     private void OnInternetConnectivityChange(NetworkServicesInternetConnectivityStatus data)
    ///     {
    ///         if (data.IsConnected)
    ///         {
    ///             // notify user that he/she is online
    ///         }
    ///         else
    ///         {
    ///             // notify user that he/she is offline
    ///         }
    ///     }
    /// 
    ///     private void OnHostReachabilityChange(NetworkServicesHostReachabilityStatus data)
    ///     {
    ///         Debug.Log("Host connectivity status: " + data.IsReachable);
    ///     }
    /// }
    /// </code>
    /// </example>
    public static class NetworkServices
    {
        #region Static fields

        [ClearOnReload]
        private     static  INativeNetworkServicesInterface     s_nativeInterface    = null;

        #endregion

        #region Static properties

        public static NetworkServicesUnitySettings UnitySettings { get; private set; }

        /// <summary>
        /// A boolean value that is used to determine internet connectivity status.
        /// </summary>
        /// <value><c>true</c> if connected to network; otherwise, <c>false</c>.</value>
        public static bool IsInternetActive { get; private set; }

        /// <summary>
        /// A boolean value that is used to determine whether host is reachable or not.
        /// </summary>
        /// <value><c>true</c> if is host reachable; otherwise, <c>false</c>.</value>
        public static bool IsHostReachable { get; private set; }

        /// <summary>
        /// A boolean value that is used to determine whether notifier is running or not.
        /// </summary>
        /// <value><c>true</c> if notifier is active; otherwise, <c>false</c>.</value>
        public static bool IsNotifierActive { get; private set; }

        #endregion
        
        #region Static events

        /// <summary>
        /// Event that will be called whenever network state changes.
        /// </summary>
        public static event Callback<NetworkServicesInternetConnectivityStatusChangeResult> OnInternetConnectivityChange;

        /// <summary>
        /// Event that will be called whenever host reachability state changes.
        /// </summary>
        public static event Callback<NetworkServicesHostReachabilityStatusChangeResult> OnHostReachabilityChange;
        
        #endregion

        #region Public methods

        public static bool IsAvailable()
        {
            return (s_nativeInterface != null) && s_nativeInterface.IsAvailable;
        }

        public static void Initialize(NetworkServicesUnitySettings settings)
        {
            Assert.IsArgNotNull(settings, nameof(settings));

            // Reset event properties
            OnInternetConnectivityChange    = null;
            OnHostReachabilityChange        = null;

            // Set default properties
            UnitySettings           = settings;
            IsInternetActive        = true;
            IsHostReachable         = true;

            // Configure interface
            s_nativeInterface       = NativeFeatureActivator.CreateInterface<INativeNetworkServicesInterface>(ImplementationSchema.NetworkServices, UnitySettings.IsEnabled);

            RegisterForEvents();
            if (UnitySettings.IsEnabled && UnitySettings.AutoStartNotifier)
            {
                SurrogateCoroutine.WaitUntilAndInvoke(new WaitForFixedUpdate(), StartNotifier);
            }
        }

        /// <summary>
        /// Starts the notifier.
        /// </summary>
        public static void StartNotifier()
        {
            try
            {
                // check current status and stop the existing notifier instance if required
                if (IsNotifierActive)
                {
                    StopNotifier();
                }

                // make request
                IsNotifierActive  = true;
                s_nativeInterface.StartNotifier();
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(EssentialKitDomain.Default, exception);
            }
        }

        /// <summary>
        /// Stops the notifier.
        /// </summary>
        public static void StopNotifier()
        {
            try
            {
                // check whether notifier is running
                if (!IsNotifierActive)
                {
                    return;
                }

                // make request
                IsNotifierActive = false;
                s_nativeInterface.StopNotifier();
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(EssentialKitDomain.Default, exception);
            }
        }

        #endregion

        #region Private methods

        private static void RegisterForEvents()
        {
            s_nativeInterface.OnInternetConnectivityChange  += HandleInternetConnectivityChangeInternalCallback;
            s_nativeInterface.OnHostReachabilityChange      += HandleHostReachabilityChangeInternalCallback;
        }

        private static void UnregisterFromEvents()
        {
            s_nativeInterface.OnInternetConnectivityChange  -= HandleInternetConnectivityChangeInternalCallback;
            s_nativeInterface.OnHostReachabilityChange      -= HandleHostReachabilityChangeInternalCallback;
        }

        #endregion

        #region Event callback methods

        private static void HandleInternetConnectivityChangeInternalCallback(bool isConnected)
        {
            // Update value
            IsInternetActive    = isConnected;

            // Notify listeners
            var    result       = new NetworkServicesInternetConnectivityStatusChangeResult(isConnected);
            CallbackDispatcher.InvokeOnMainThread(OnInternetConnectivityChange, result);
        }

        private static void HandleHostReachabilityChangeInternalCallback(bool isReachable)
        {
            // Update value
            IsHostReachable     = isReachable;

            // Notify listeners
            var     result      = new NetworkServicesHostReachabilityStatusChangeResult(isReachable);
            CallbackDispatcher.InvokeOnMainThread(OnHostReachabilityChange, result);
        }

        #endregion
    }
}