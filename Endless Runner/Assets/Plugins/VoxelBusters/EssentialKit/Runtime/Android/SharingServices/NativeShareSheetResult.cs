#if UNITY_ANDROID
using UnityEngine;
using System.Collections.Generic;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.EssentialKit.SharingServicesCore.Android
{
    public enum NativeShareSheetResult
    {
        Cancelled = 0,
        Done = 1,
        Unknown = 2
    }
    public class NativeShareSheetResultHelper
    {
        internal const string kClassName = "com.voxelbusters.essentialkit.sharingservices.Enums$ShareSheetResult";

        public static AndroidJavaObject CreateWithValue(NativeShareSheetResult value)
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[NativeShareSheetResultHelper : NativeShareSheetResultHelper][Method(CreateWithValue) : NativeShareSheetResult]");
#endif
            AndroidJavaClass javaClass = new AndroidJavaClass(kClassName);
            AndroidJavaObject[] values = javaClass.CallStatic<AndroidJavaObject[]>("values");
            return values[(int)value];
        }

        public static NativeShareSheetResult ReadFromValue(AndroidJavaObject value)
        {
            return (NativeShareSheetResult)value.Call<int>("ordinal");
        }
    }
}
#endif