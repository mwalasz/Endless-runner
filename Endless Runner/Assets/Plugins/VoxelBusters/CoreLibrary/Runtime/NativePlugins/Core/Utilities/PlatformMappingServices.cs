using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace VoxelBusters.CoreLibrary.NativePlugins
{
    public static class PlatformMappingServices
    {
        #region Platform methods

        public static NativePlatform GetActivePlatform()
        {
            return ConvertRuntimePlatformToNativePlatform(Application.platform);
        }

        public static NativePlatform ConvertRuntimePlatformToNativePlatform(RuntimePlatform platform)
        {
            switch (platform)
            {
#if UNITY_EDITOR
                case RuntimePlatform.OSXEditor:
                case RuntimePlatform.WindowsEditor:
                case RuntimePlatform.LinuxEditor:
                    return ConvertBuildTargetToNativePlatform(EditorUserBuildSettings.activeBuildTarget);
#endif
                case RuntimePlatform.IPhonePlayer:
                    return NativePlatform.iOS;

                case RuntimePlatform.tvOS:
                    return NativePlatform.tvOS;

                case RuntimePlatform.Android:
                    return NativePlatform.Android;

                default:
                    return NativePlatform.Unknown;
            }
        }

        #endregion

        #region Editor methods

#if UNITY_EDITOR
        public static NativePlatform ConvertBuildTargetToNativePlatform(BuildTarget buildTarget)
        {
            switch (buildTarget)
            {
                case BuildTarget.iOS:
                    return NativePlatform.iOS;

                case BuildTarget.tvOS:
                    return NativePlatform.tvOS;

                case BuildTarget.Android:
                    return NativePlatform.Android;

                default:
                    return NativePlatform.Unknown;
            }
        }
#endif

        #endregion
    }
}