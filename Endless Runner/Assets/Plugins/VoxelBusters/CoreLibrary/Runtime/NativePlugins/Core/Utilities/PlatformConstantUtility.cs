using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.CoreLibrary.NativePlugins
{
    [Obsolete("This class is deprecated. Instead use RuntimePlatformConstantUtility.", true)]
    public static class PlatformConstantUtility 
    {
        #region Static methods

        public static PlatformConstant FindConstantForActivePlatform(PlatformConstant[] array)
        {
            var     activePlatform  = PlatformMappingServices.GetActivePlatform();
            return FindConstantForPlatform(array, activePlatform);
        }

        public static PlatformConstant FindConstantForPlatform(PlatformConstant[] array, NativePlatform platform)
        {
            if (array != null)
            {
                return Array.Find(array, (item) => item.IsEqualToPlatform(platform));
            }

            return null;
        }

        public static string GetActivePlatformConstantValue(PlatformConstant[] array)
        {
            var     targetConstant  = FindConstantForActivePlatform(array);
            return (targetConstant != null) ? targetConstant.Value : null;
        }
            
        public static string GetPlatformConstantValue(PlatformConstant[] array, NativePlatform platform)
        {
            var     targetConstant  = FindConstantForPlatform(array, platform);
            return (targetConstant != null) ? targetConstant.Value : null;
        }

        #endregion
    }
}