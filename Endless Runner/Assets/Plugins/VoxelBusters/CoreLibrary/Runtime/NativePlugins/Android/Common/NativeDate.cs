#if UNITY_ANDROID
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.CoreLibrary.NativePlugins.Android
{
    public class NativeDate : NativeAndroidJavaObjectWrapper
    {
        private const string kClassName = "java.util.Date";
        public NativeDate() : base(kClassName)
        {
            m_nativeObject = new AndroidJavaObject(kClassName);
        }
        public NativeDate(AndroidJavaObject nativeObject) : base(kClassName, nativeObject)
        {
        }

        public void SetDateTime(DateTime dateTime)
        {
            DateTime epochReference = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            long epoch = Convert.ToInt64((dateTime - epochReference).TotalMilliseconds);
            m_nativeObject.Call("setTime", epoch);
        }

        public DateTime GetDateTime()
        {
            long epoch = m_nativeObject.Call<long>("getTime")/1000;
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return dateTime.AddSeconds(epoch);
        }
    }
}
#endif