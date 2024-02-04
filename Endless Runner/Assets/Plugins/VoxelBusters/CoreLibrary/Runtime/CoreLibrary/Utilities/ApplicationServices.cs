using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace VoxelBusters.CoreLibrary
{
    public static class ApplicationServices
    {
        #region Static fields

        public static float OriginalTimeScale { get; set; }

        #endregion

        #region Constructors

        static ApplicationServices()
        {
            Initialize();
        }

        #endregion

        #region Static methods

        [ExecuteOnReload]
        private static void Initialize()
        {
            OriginalTimeScale   = Time.timeScale;
        }

        public static void SetApplicationPaused(bool pause)
        {
            if (pause)
            {
                // cache original value
                OriginalTimeScale = Time.timeScale;

                // set new value
                Time.timeScale      = 0f;
            }
            else
            {
                Time.timeScale      = OriginalTimeScale;
            }
        }

        public static RuntimePlatform GetActivePlatform()
        {
            return Application.platform;
        }

        public static RuntimePlatform GetActiveOrSimulationPlatform()
        {
#if UNITY_EDITOR
            return ConvertBuildTargetToRuntimePlatform(UnityEditor.EditorUserBuildSettings.activeBuildTarget);
#else
            return Application.platform;
#endif
        }

        public static bool IsPlayingOrSimulatingMobilePlatform()
        {
#if UNITY_EDITOR
            var     platform    = GetActiveOrSimulationPlatform();
            return (platform == RuntimePlatform.Android) || (platform == RuntimePlatform.IPhonePlayer);
#else
            return Application.isMobilePlatform;
#endif
        }

#if UNITY_EDITOR
        public static RuntimePlatform ConvertBuildTargetToRuntimePlatform(UnityEditor.BuildTarget buildTarget)
        {
            switch (buildTarget)
            {
                case UnityEditor.BuildTarget.iOS:
                    return RuntimePlatform.IPhonePlayer;

                case UnityEditor.BuildTarget.tvOS:
                    return RuntimePlatform.tvOS;

                case UnityEditor.BuildTarget.Android:
                    return RuntimePlatform.Android;

                case UnityEditor.BuildTarget.StandaloneOSX:
                    return RuntimePlatform.OSXPlayer;

                case UnityEditor.BuildTarget.StandaloneWindows:
                case UnityEditor.BuildTarget.StandaloneWindows64:
                    return RuntimePlatform.WindowsPlayer;

                case UnityEditor.BuildTarget.WebGL:
                    return RuntimePlatform.WebGLPlayer;

                default:
                    return Application.platform;
            }
        }
#endif

        #endregion
    }
}