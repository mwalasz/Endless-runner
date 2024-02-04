#if UNITY_ANDROID
using System;
using UnityEngine;

namespace VoxelBusters.CoreLibrary.NativePlugins.Android
{
    public class NativeActivity : NativeAndroidJavaObjectWrapper
    {
        public NativeActivity(AndroidJavaObject javaObject) : base("android.app.Activity", javaObject)
        {
        }
        public NativeActivity(NativeAndroidJavaObjectWrapper wrapper) : base(wrapper)
        {
        }

        public void RunOnUiThread(Action action)
        {
            m_nativeObject.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                action();
            }));
        }


    }
}
#endif