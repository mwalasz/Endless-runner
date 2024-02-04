#if UNITY_IOS || UNITY_TVOS
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.CoreLibrary.NativePlugins.iOS
{
    public class IosNativeObjectRef : NativeObjectRef
    {
        #region Constructors

        public IosNativeObjectRef(IntPtr ptr, bool retain = true)
            : base(ptr, retain)
        { }

        #endregion

        #region Base class methods

        protected override void RetainInternal(IntPtr ptr)
        {
            IosNativePluginsUtility.RetainNativeObject(ptr);
        }

        protected override void ReleaseInternal(IntPtr ptr)
        {
            IosNativePluginsUtility.ReleaseNativeObject(ptr);
        }

        #endregion
    }
}
#endif