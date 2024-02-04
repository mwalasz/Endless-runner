#if UNITY_ANDROID
using UnityEngine;
using System.Collections.Generic;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.EssentialKit.SharingServicesCore.Android
{
    public enum NativeMailComposerResult
    {
        Cancelled = 0,
        Sent = 1,
        Unknown = 2
    }
    public class NativeMailComposerResultHelper
    {
        internal const string kClassName = "com.voxelbusters.essentialkit.sharingservices.Enums$MailComposerResult";

        public static AndroidJavaObject CreateWithValue(NativeMailComposerResult value)
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[NativeMailComposerResultHelper : NativeMailComposerResultHelper][Method(CreateWithValue) : NativeMailComposerResult]");
#endif
            AndroidJavaClass javaClass = new AndroidJavaClass(kClassName);
            AndroidJavaObject[] values = javaClass.CallStatic<AndroidJavaObject[]>("values");
            return values[(int)value];
        }

        public static NativeMailComposerResult ReadFromValue(AndroidJavaObject value)
        {
            return (NativeMailComposerResult)value.Call<int>("ordinal");
        }
    }
}
#endif