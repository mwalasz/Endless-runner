using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit.ExtrasCore
{
    public abstract class NativeUtilityInterfaceBase : NativeFeatureInterfaceBase, INativeUtilityInterface
    {
        #region Constructors

        protected NativeUtilityInterfaceBase(bool isAvailable)
            : base(isAvailable)
        { }

        #endregion

        #region INativeUtilityInterface implementation

        public abstract void RequestStoreReview();

        public abstract void OpenAppStorePage(string applicationId); 

        public abstract void OpenApplicationSettings();
        
        #endregion
    }
}