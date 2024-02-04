using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace VoxelBusters.CoreLibrary.Editor.NativePlugins.Build
{
    public class TargetBuilder
    {
#if NATIVE_PLUGINS_DEBUG
        [MenuItem("Tools/Make Build")]
#endif
        public static void Build()
        {
#if UNITY_IOS
            BuildIos();
#elif UNITY_ANDROID
            BuildAndroid();
#elif UNITY_STANDALONE_WIN
            BuildWindows();
#elif UNITY_STANDALONE_OSX
            BuildOsx();
#elif UNITY_STANDALONE_LINUX
            BuildLinux();
#elif UNITY_WEBGL
            BuildLinux();
#else
            Debug.LogError("No platform target defined!");
#endif
        }

        private static void BuildIos()
        {
            Build(BuildTargetGroup.iOS, BuildTarget.iOS, null);
        }

        private static void BuildAndroid()
        {
            EditorUserBuildSettings.exportAsGoogleAndroidProject = true;
			PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);
            PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARMv7 | AndroidArchitecture.ARM64;
            if (EditorUserBuildSettings.development)
            {
#if !UNITY_2019_1_OR_NEWER
                PlayerSettings.Android.targetArchitectures |= AndroidArchitecture.X86;
#endif
            }

            Build(BuildTargetGroup.Android, BuildTarget.Android, null, /*BuildOptions.AcceptExternalModificationsToPlayer |*/ BuildOptions.AllowDebugging);
        }

        private static void BuildWindows()
        {
            Build(BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows64, "exe");
        }

        private static void BuildOsx()
        {
            Build(BuildTargetGroup.Standalone, BuildTarget.StandaloneOSX, "app");
        }

        private static void BuildLinux()
        {
            Build(BuildTargetGroup.Standalone, BuildTarget.StandaloneLinux64, null);
        }

        private static void BuildWebGl()
        {
            Build(BuildTargetGroup.WebGL, BuildTarget.WebGL, null);
        }

        private static void BuildAllPlatforms()
        {
            BuildMobilePlatforms();
            BuildNonMobilePlatforms();
        }

        private static void BuildMobilePlatforms()
        {
            BuildIos();
            BuildAndroid();
        }

        private static void BuildNonMobilePlatforms()
        {
            BuildOsx();
            BuildWindows();
            BuildLinux();
            BuildWebGl();
        }

        private static void Build(BuildTargetGroup targetGroup, BuildTarget target, string extension, BuildOptions options = BuildOptions.None)
        {
            string platform = GetPlatformName(target);

            //Set values from environment variables
            LoadEnvironmentVariables(target);

            //Switch to active target
            bool isSwitchSuccess = EditorUserBuildSettings.SwitchActiveBuildTarget(targetGroup, target);

            if (!isSwitchSuccess)
                Debug.LogError(string.Format("Failed switching to platform : {0}", platform));

            //Build
#if UNITY_2019_3_OR_NEWER
            string targetPath = (extension != null) ? string.Format("../builds/{0}.{1}", platform, extension) : string.Format("../builds/{0}/{1}", platform, PlayerSettings.productName);
#else
            string targetPath = (extension != null) ? string.Format("../builds/{0}.{1}", platform, extension) : string.Format("../builds/{0}", platform);
#endif
            BuildReport report = UnityEditor.BuildPipeline.BuildPlayer(EditorBuildSettings.scenes, targetPath, target, options);
            BuildSummary summary = report.summary;

            if (summary.result == BuildResult.Succeeded)
            {
                Debug.Log(string.Format("Finished building for {0} : {1}", platform, targetPath));
            }
            else if (summary.result == BuildResult.Failed)
            {
                Debug.LogError(string.Format("Failed building for {0} : {1}", platform, targetPath));
            }
            else if (summary.result == BuildResult.Cancelled)
            {
                Debug.LogError(string.Format("Cancelled building for {0} : {1}", platform, targetPath));
            }
            else
            {
                Debug.LogError(string.Format("Failed building for {0} : {1}", platform, targetPath));
            }
        }

        private static string GetPlatformName(BuildTarget target)
        {
            switch (target)
            {
                case BuildTarget.iOS:
                    return "ios";
                case BuildTarget.Android:
                    return "android";
                case BuildTarget.StandaloneLinux64:
                    return "linux";
                case BuildTarget.StandaloneWindows64:
                    return "windows";
                case BuildTarget.StandaloneOSX:
                    return "osx";
                case BuildTarget.WebGL:
                    return "webgl";
                default:
                    throw new System.Exception("Platform name not set!");
            }
        }

        private static void LoadEnvironmentVariables(BuildTarget target)
        {
            PlayerSettings.productName = GetEnvironmentVariable<string>("PRODUCT_NAME", PlayerSettings.productName);
            PlayerSettings.bundleVersion = GetEnvironmentVariable<string>("PRODUCT_VERSION", PlayerSettings.bundleVersion);
            PlayerSettings.companyName = GetEnvironmentVariable<string>("PRODUCT_COMPANY_NAME", PlayerSettings.companyName);

            switch (target)
            {
                case BuildTarget.Android:
                    PlayerSettings.applicationIdentifier = GetEnvironmentVariable<string>("ANDROID_PACKAGE_NAME", PlayerSettings.applicationIdentifier);
                    break;
                case BuildTarget.iOS:
                    PlayerSettings.applicationIdentifier = GetEnvironmentVariable<string>("IOS_BUNDLE_IDENTIFIER", PlayerSettings.applicationIdentifier);
                    break;
            }

        }

        private static T GetEnvironmentVariable<T>(string key, T defaultValue) where T : IConvertible
        {
            object value = System.Environment.GetEnvironmentVariable(key);
            if (value != null)
                return (T)Convert.ChangeType(value, typeof(T));
            else
                return defaultValue;
        }
    }
}
