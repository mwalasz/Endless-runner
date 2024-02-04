#if UNITY_ANDROID
using UnityEngine;

namespace VoxelBusters.CoreLibrary.NativePlugins.Android
{
    public class NativeContext : NativeAndroidJavaObjectWrapper
    {
        public NativeContext(AndroidJavaObject javaObject) : base("android.content.Context", javaObject)
        {
        }
    }
}
#endif