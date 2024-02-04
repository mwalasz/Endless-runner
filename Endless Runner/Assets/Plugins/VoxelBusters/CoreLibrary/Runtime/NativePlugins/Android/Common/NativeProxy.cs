#if UNITY_ANDROID
using System;
using UnityEngine;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.CoreLibrary.NativePlugins.Android
{
    public abstract class NativeProxyBase : AndroidJavaProxy
    {
        public NativeProxyBase(string interfaceName) : base(interfaceName)
        {
        }

        protected void DispatchOnMainThread(Callback action)
        {
            // Dispatch on Unity Thread
            CallbackDispatcher.InvokeOnMainThread(action);
        }
    }

    public class NativeProxy<T> : NativeProxyBase
    {
        protected T m_callback;

        public NativeProxy(T m_callback, string interfaceName) : base(interfaceName)
        {
            this.m_callback = m_callback;
        }
    }
}
#endif