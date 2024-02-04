#if UNITY_ANDROID
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.CoreLibrary.NativePlugins.Android
{
    public class NativeException : NativeAndroidJavaObjectWrapper
    {
        public NativeException(AndroidJavaObject androidJavaObject) : base("java.lang.Exception", androidJavaObject)
        {
        }

        public string GetMessage()
        {
            return m_nativeObject.Call<string>("getMessage");
        }

        public void PrintStackTrace()
        {
            m_nativeObject.Call("printStackTrace");
        }
    }
}
#endif