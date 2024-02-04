#if UNITY_ANDROID
using UnityEngine;
using System.Collections.Generic;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.EssentialKit.SharingServicesCore.Android
{
    public enum NativeSocialShareComposerResult
    {
        Cancelled = 0,
        Done = 1,
        Unavailable = 2,
        Unknown = 3
    }
    public class NativeSocialShareComposerResultHelper
    {
        internal const string kClassName = "com.voxelbusters.essentialkit.sharingservices.Enums$SocialShareComposerResult";

        public static AndroidJavaObject CreateWithValue(NativeSocialShareComposerResult value)
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[NativeSocialShareComposerResultHelper : NativeSocialShareComposerResultHelper][Method(CreateWithValue) : NativeSocialShareComposerResult]");
#endif
            AndroidJavaClass javaClass = new AndroidJavaClass(kClassName);
            AndroidJavaObject[] values = javaClass.CallStatic<AndroidJavaObject[]>("values");
            return values[(int)value];
        }

        public static NativeSocialShareComposerResult ReadFromValue(AndroidJavaObject value)
        {
            return (NativeSocialShareComposerResult)value.Call<int>("ordinal");
        }
    }
}
#endif