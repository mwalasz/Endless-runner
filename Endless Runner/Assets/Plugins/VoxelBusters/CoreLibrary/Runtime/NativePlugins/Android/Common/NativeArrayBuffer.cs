#if UNITY_ANDROID
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.CoreLibrary.NativePlugins.Android
{
    public class NativeArrayBuffer<T> : NativeAndroidJavaObjectWrapper
    {
        public NativeArrayBuffer(AndroidJavaObject androidJavaObject) : base("com.voxelbusters.android.essentialkit.common.ArrayBuffer", androidJavaObject)
        {
        }

        public int Size()
        {
            return m_nativeObject.Call<int>("size");
        }

        public T Get(int index)
        {
            if (m_nativeObject == null)
                return default(T);

            AndroidJavaObject androidJavaObject = Call<AndroidJavaObject>("get", index);
            var instance = (T)Activator.CreateInstance(typeof(T), new object[] { androidJavaObject });
            return instance;
        }

        public T[] GetArray()
        {
            if (m_nativeObject == null)
                return default(T[]);

            if (typeof(T) == typeof(byte))
            {
                sbyte[] sbyteArray = AndroidJNIHelper.ConvertFromJNIArray<sbyte[]>(NativeObject.GetRawObject());
                int length = sbyteArray.Length;
                byte[] bytes = new byte[length];
                Buffer.BlockCopy(sbyteArray, 0, bytes, 0, length);
                return bytes as T[];
            }
            else
            {
                return AndroidJNIHelper.ConvertFromJNIArray<T[]>(NativeObject.GetRawObject());
            }
        }
    }
}
#endif