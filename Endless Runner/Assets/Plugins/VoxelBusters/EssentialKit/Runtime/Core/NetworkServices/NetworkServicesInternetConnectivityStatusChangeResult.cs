using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.EssentialKit
{
    /// <summary>
    /// This interface contains the information retrieved when <see cref="NetworkServices.OnInternetConnectivityChange"/> event occurs.
    /// </summary>
    public class NetworkServicesInternetConnectivityStatusChangeResult
    {
        #region Properties

        /// <summary>
        /// This boolean value is used to determine whether internet connection is available.
        /// </summary>
        /// <value><c>true</c> if is connected; otherwise, <c>false</c>.</value>
        public bool IsConnected { get; private set; }

        #endregion

        #region Constructors

        internal NetworkServicesInternetConnectivityStatusChangeResult(bool isConnected)
        {
            // Set properties
            IsConnected    = isConnected;
        }

        #endregion
    }
}