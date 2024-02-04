#if UNITY_ANDROID
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.CoreLibrary.NativePlugins.Android
{
    public class NativeList<T> : NativeAndroidJavaObjectWrapper
    {
        public NativeList(AndroidJavaObject androidJavaObject) : base("java.util.List", androidJavaObject)
        {
        }

        public int Size()
        {
            return m_nativeObject.Call<int>("size");
        }

        public List<T> Get()
        {
            int size = Size();
            List<T> list = new List<T>();
            for (int eachIndex = 0; eachIndex < size; eachIndex++)
            {
                AndroidJNI.PushLocalFrame(128);
                AndroidJavaObject eachNativeObject = m_nativeObject.Call<AndroidJavaObject>("get", eachIndex);
                var instance = (T)Activator.CreateInstance(typeof(T), new object[] { eachNativeObject });
                list.Add(instance);
                AndroidJNI.PopLocalFrame(IntPtr.Zero);
            }

            return list;
        }
    }
}
#endif