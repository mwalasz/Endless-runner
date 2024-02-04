#if UNITY_ANDROID
using UnityEngine;
using System.Collections.Generic;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.EssentialKit.SharingServicesCore.Android
{
    public enum NativeMessageComposerResult
    {
        Cancelled = 0,
        Sent = 1,
        Unknown = 2
    }
    public class NativeMessageComposerResultHelper
    {
        internal const string kClassName = "com.voxelbusters.essentialkit.sharingservices.Enums$MessageComposerResult";

        public static AndroidJavaObject CreateWithValue(NativeMessageComposerResult value)
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[NativeMessageComposerResultHelper : NativeMessageComposerResultHelper][Method(CreateWithValue) : NativeMessageComposerResult]");
#endif
            AndroidJavaClass javaClass = new AndroidJavaClass(kClassName);
            AndroidJavaObject[] values = javaClass.CallStatic<AndroidJavaObject[]>("values");
            return values[(int)value];
        }

        public static NativeMessageComposerResult ReadFromValue(AndroidJavaObject value)
        {
            return (NativeMessageComposerResult)value.Call<int>("ordinal");
        }
    }
}
#endif