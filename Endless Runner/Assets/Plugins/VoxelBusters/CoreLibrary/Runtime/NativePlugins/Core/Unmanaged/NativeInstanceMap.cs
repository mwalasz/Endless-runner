using System;
using System.Collections.Generic;

namespace VoxelBusters.CoreLibrary.NativePlugins
{
    internal static class NativeInstanceMap
    {
        #region Static fields

        [ClearOnReload]
        private static Dictionary<IntPtr, object> s_instanceMap;

        #endregion

        #region Constructors

        static NativeInstanceMap()
        {
            Initialize();
        }

        #endregion

        #region Static methods

        public static void AddInstance(IntPtr nativePtr, object owner)
        {
            s_instanceMap.Add(nativePtr, owner);
        }

        public static bool RemoveInstance(IntPtr nativePtr)
        {
            return s_instanceMap.Remove(nativePtr);
        }

        public static T GetOwner<T>(IntPtr nativePtr) where T : class
        {
            s_instanceMap.TryGetValue(nativePtr, out object owner);

            return owner as T;
        }

        #endregion

        #region Private static methods

        [ExecuteOnReload]
        private static void Initialize()
        {
            // Set properties
            s_instanceMap   = new Dictionary<IntPtr, object>(capacity: 4);
        }

        #endregion
    }
}