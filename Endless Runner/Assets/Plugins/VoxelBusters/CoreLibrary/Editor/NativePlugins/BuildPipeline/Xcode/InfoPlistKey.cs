#if UNITY_IOS || UNITY_TVOS
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.CoreLibrary.Editor.NativePlugins.Build.Xcode
{
    public static class InfoPlistKey
    {
        #region Constants

        public  const string   kNSContactsUsage                     = "NSContactsUsageDescription";

        public  const string   kNSCameraUsage                       = "NSCameraUsageDescription";

        public  const string   kNSPhotoLibraryUsage                 = "NSPhotoLibraryUsageDescription";

        public  const string   kNSPhotoLibraryAdd                   = "NSPhotoLibraryAddUsageDescription";

        public  const string   kNSMicrophoneUsage                   = "NSMicrophoneUsageDescription";

        public  const string   kNSLocationWhenInUse                 = "NSLocationWhenInUseUsageDescription";

        public  const string   kNSAppTransportSecurity              = "NSAppTransportSecurity";

        public  const string   kNSAllowsArbitraryLoads              = "NSAllowsArbitraryLoads";

        public  const string   kNSQuerySchemes                      = "LSApplicationQueriesSchemes";

        public  const string   kNSDeviceCapablities                 = "UIRequiredDeviceCapabilities";

        public  const string   kCFBundleURLTypes                    = "CFBundleURLTypes";

        public  const string   kCFBundleURLName                     = "CFBundleURLName";

        public  const string   kCFBundleURLSchemes                  = "CFBundleURLSchemes";

        #endregion
    }
}
#endif