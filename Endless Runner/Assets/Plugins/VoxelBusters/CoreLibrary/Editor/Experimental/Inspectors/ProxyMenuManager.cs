using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace VoxelBusters.CoreLibrary.Editor.Experimental
{
    public static class ProxyMenuManager
    {
        #region Constants

        private const string kMenuItemPath = "Window/Voxel Busters";

        #endregion

        #region Static methods

#if !ENABLE_VOXELBUSTERS_ESSENTIAL_KIT
        [MenuItem(kMenuItemPath + "/Essential Kit/Learn More", priority = 0)]
        public static void InstallEssentialKit()
        {
            OpenInstallPath(InstallPath.EssentialKit);
        }
#endif

#if !ENABLE_VOXELBUSTERS_SCREEN_RECORDER_KIT
        [MenuItem(kMenuItemPath + "/Screen Recorder Kit/Learn More", priority = 0)]
        public static void InstallScreenRecorderKit()
        {
            OpenInstallPath(InstallPath.ScreenRecorderKit);
        }
#endif

#if !ENABLE_VOXELBUSTERS_SOCIAL_KIT
        [MenuItem(kMenuItemPath + "/Social Kit/Learn More", priority = 0)]
        public static void InstallSocialKit()
        {
            OpenInstallPath(InstallPath.SocialKit);
        }
#endif

#if !ENABLE_VOXELBUSTERS_ML_KIT
        [MenuItem(kMenuItemPath + "/ML Kit/Learn More", priority = 0)]
        public static void InstallMLKit()
        {
            OpenInstallPath(InstallPath.MLKit);
        }
#endif

#if !ENABLE_VOXELBUSTERS_REPORTING_KIT
        [MenuItem(kMenuItemPath + "/Reporting Kit/Learn More", priority = 0)]
        public static void InstallReportingKit()
        {
            OpenInstallPath(InstallPath.ReportingKit);
        }
#endif

        private static void OpenInstallPath(string path)
        {
            Application.OpenURL(path);
        }

        #endregion
    }
}