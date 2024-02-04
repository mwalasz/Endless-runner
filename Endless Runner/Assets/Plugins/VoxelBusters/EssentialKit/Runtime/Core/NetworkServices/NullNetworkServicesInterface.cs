using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit.NetworkServicesCore
{
    internal sealed class NullNetworkServicesInterface : NativeNetworkServicesInterfaceBase, INativeNetworkServicesInterface
    {
        #region Constructors

        public NullNetworkServicesInterface() 
            : base(isAvailable: false)
        { }

        #endregion

        #region Private static methods

        private static void LogNotSupported()
        {
            Diagnostics.LogNotSupported("NetworkServices");
        }

        #endregion

        #region Base class methods

        public override void StartNotifier()
        {
            LogNotSupported();
        }

        public override void StopNotifier()
        { 
            LogNotSupported();
        }

        #endregion
    }
}