using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.CoreLibrary.NativePlugins
{
    public abstract class NativeObjectBase : INativeObject
    {
        #region Properties

        protected bool IsDisposed { get; private set; }

        #endregion

        #region Constructors

        protected NativeObjectBase(NativeObjectRef nativeObjectRef = null)
        {
            // set properties
            NativeObjectRef     = nativeObjectRef;
            IsDisposed          = false;
        }

        ~NativeObjectBase()
        {
            Dispose(false);
        }

        #endregion

        #region INativeInterface implementation

        public NativeObjectRef NativeObjectRef { get; protected set; }

        public IntPtr AddrOfNativeObject()
        {
            return (NativeObjectRef == null) ? IntPtr.Zero : NativeObjectRef.Pointer;
        }

        #endregion

        #region IDisposable implementation

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            // check object state
            if (IsDisposed)
            {
                return;
            }

#if NATIVE_PLUGINS_DEBUG && !UNITY_ANDROID
            DebugLogger.Log(CoreLibraryDomain.NativePlugins, $"Disposing native object: {this}.");
#endif

            if (disposing)
            {
                // dispose managed state (managed objects)
            }

            // free unmanaged resources (unmanaged objects)
            if (NativeObjectRef != null)
            {
                NativeObjectRef.Dispose();
            }

            // mark that object is released
            IsDisposed = true;
        }

        #endregion
    }
}