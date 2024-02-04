#if UNITY_IOS || UNITY_TVOS
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.CoreLibrary.Editor.NativePlugins.Build.Xcode
{
    public static class BuildConfigurationKey
    {
        public  const   string  kBuildSettings          = "buildSettings";

        public  const   string  kHeaderSearchPaths      = "HEADER_SEARCH_PATHS";

        public  const   string  kLibrarySearchPaths     = "LIBRARY_SEARCH_PATHS";

        public  const   string  kFrameworkSearchPaths   = "FRAMEWORK_SEARCH_PATHS";

        public  const   string  kOtherCFlags            = "OTHER_CFLAGS";

        public  const   string  kOtherLDFlags           = "OTHER_LDFLAGS";

        public  const   string  kCodeSignEntitlements   = "CODE_SIGN_ENTITLEMENTS";
    }
}
#endif