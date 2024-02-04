using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.CoreLibrary
{
    public static class RuntimePlatformConstantUtility 
    {
        #region Static methods

        public static RuntimePlatformConstant FindConstantForActivePlatform(RuntimePlatformConstant[] array)
        {
            var     activePlatform  = ApplicationServices.GetActiveOrSimulationPlatform();
            return FindConstantForPlatform(array, activePlatform);
        }

        public static RuntimePlatformConstant FindConstantForPlatform(RuntimePlatformConstant[] array, RuntimePlatform platform)
        {
            if (array != null)
            {
                return Array.Find(array, (item) => item.IsEqualToPlatform(platform));
            }

            return null;
        }

        public static string GetConstantValueForActivePlatform(RuntimePlatformConstant[] array)
        {
            var     targetConstant  = FindConstantForActivePlatform(array);
            return targetConstant?.Value;
        }
            
        public static string GetConstantValueForPlatform(RuntimePlatformConstant[] array, RuntimePlatform platform)
        {
            var     targetConstant  = FindConstantForPlatform(array, platform);
            return targetConstant?.Value;
        }

        #endregion
    }
}