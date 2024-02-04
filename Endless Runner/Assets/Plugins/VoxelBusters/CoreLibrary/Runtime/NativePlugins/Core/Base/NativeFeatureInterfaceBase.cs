using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.CoreLibrary.NativePlugins
{
    public abstract class NativeFeatureInterfaceBase : NativeObjectBase, INativeFeatureInterface
    {
        #region Constructors

        protected NativeFeatureInterfaceBase(bool isAvailable, NativeObjectRef nativeObjectRef = null)
            : base(nativeObjectRef)
        {
            // set properties
            IsAvailable     = isAvailable;
        }

        ~NativeFeatureInterfaceBase()
        {
            Dispose(false);
        }

        #endregion

        #region INativeFeatureInterface implementation

        public bool IsAvailable
        {
            get;
            private set;
        }

        #endregion
    }
}