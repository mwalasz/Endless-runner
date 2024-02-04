using UnityEngine;

namespace VoxelBusters.CoreLibrary.Editor.NativePlugins.Build
{
    public class PBXFrameworkNameAttribute : PropertyAttribute
    {
        #region Static fields

        private     static  readonly    string[]    s_frameworkNames    = new string[]
        {
            "Accounts.framework",
            "AVFoundation.framework",
            "Contacts.framework",
            "CloudKit.framework",
            "GameKit.framework",
            "MessageUI.framework",
            "MobileCoreServices.framework",
            "Photos.framework",
            "Social.framework",
            "StoreKit.framework",
            "SystemConfiguration.framework",
            "UserNotifications.framework",
            "WebKit.framework",
            "ReplayKit.framework"
        };

        #endregion

        #region Public Methods

        public string[] GetFrameworkNames()
        {
            return s_frameworkNames;
        }

        #endregion
    }
}