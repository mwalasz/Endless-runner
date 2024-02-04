#if UNITY_ANDROID
using UnityEngine;
using System.Collections.Generic;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.EssentialKit.SharingServicesCore.Android
{
    public enum NativeSocialShareComposerType
    {
        None = 0,
        Facebook = 1,
        Twitter = 2,
        Instagram = 3,
        Whatsapp = 4
    }
    public class NativeSocialShareComposerTypeHelper
    {
        internal const string kClassName = "com.voxelbusters.essentialkit.sharingservices.Enums$SocialShareComposerType";

        public static AndroidJavaObject CreateWithValue(NativeSocialShareComposerType value)
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[NativeSocialShareComposerTypeHelper : NativeSocialShareComposerTypeHelper][Method(CreateWithValue) : NativeSocialShareComposerType]");
#endif
            AndroidJavaClass javaClass = new AndroidJavaClass(kClassName);
            AndroidJavaObject[] values = javaClass.CallStatic<AndroidJavaObject[]>("values");
            return values[(int)value];
        }

        public static NativeSocialShareComposerType ReadFromValue(AndroidJavaObject value)
        {
            return (NativeSocialShareComposerType)value.Call<int>("ordinal");
        }
    }
}
#endif