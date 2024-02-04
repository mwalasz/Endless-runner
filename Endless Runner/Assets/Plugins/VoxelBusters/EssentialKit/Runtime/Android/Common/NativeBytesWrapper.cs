#if UNITY_ANDROID
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary.NativePlugins.Android;


namespace VoxelBusters.EssentialKit.Common.Android
{
    public class NativeBytesWrapper : NativeAndroidJavaObjectWrapper
    {
#region Fields

        private const string kClassName = "com.voxelbusters.essentialkit.utilities.common.BytesWrapper";
        private byte[] m_cachedBytes;

#endregion

        public NativeBytesWrapper(byte[] array) : base(kClassName, array)
        {
            Debug.Log("Creating from byte array : " + array);
        }

        public NativeBytesWrapper(AndroidJavaObject androidJavaObject) : base(kClassName, androidJavaObject)
        {
            Debug.Log("Creating from android native object : " + androidJavaObject.GetRawObject());
        }

        public byte[] GetBytes()
        {
            if (m_nativeObject == null)
                return default(byte[]);

            if(m_cachedBytes == null)
            {
                m_cachedBytes = Call<byte[]>("getBytes");
            }

            return m_cachedBytes;
        }
    }
}
#endif
