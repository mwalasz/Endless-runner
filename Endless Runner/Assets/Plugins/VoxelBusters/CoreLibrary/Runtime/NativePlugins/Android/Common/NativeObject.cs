#if UNITY_ANDROID
using UnityEngine;

namespace VoxelBusters.CoreLibrary.NativePlugins.Android
{
    public class NativeObject : NativeAndroidJavaObjectWrapper
    {
        public NativeObject(AndroidJavaObject javaObject) : base("android.lang.Object", javaObject)
        {
        }
    }
}
#endif