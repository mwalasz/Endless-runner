#if UNITY_ANDROID
using UnityEngine;

namespace VoxelBusters.CoreLibrary.NativePlugins.Android
{
    public class NativeParcel : NativeAndroidJavaObjectWrapper
    {
        public NativeParcel(AndroidJavaObject javaObject) : base("android.os.Parcel", javaObject)
        {
        }
    }
}
#endif