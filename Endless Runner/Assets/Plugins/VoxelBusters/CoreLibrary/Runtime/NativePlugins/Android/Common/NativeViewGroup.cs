#if UNITY_ANDROID
using UnityEngine;

namespace VoxelBusters.CoreLibrary.NativePlugins.Android
{
    public class NativeViewGroup : NativeAndroidJavaObjectWrapper
    {
        public NativeViewGroup(AndroidJavaObject javaObject) : base("android.view.ViewGroup", javaObject)
        {
        }
    }
}
#endif