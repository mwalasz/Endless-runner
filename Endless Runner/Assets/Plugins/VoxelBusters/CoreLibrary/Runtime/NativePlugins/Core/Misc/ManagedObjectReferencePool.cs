using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.EssentialKit
{
    internal static class ManagedObjectReferencePool
    {
        #region Static fields

        [ClearOnReload]
        private     static      List<object>        s_objectList;

        #endregion

        #region Static methods

        public static void Retain(object obj)
        {
            // Validate arguments
            Assert.IsArgNotNull(obj, "obj");

            EnsureInitialized();
            s_objectList.Add(obj);
        }

        public static void Release(object obj)
        {
            // Validate arguments
            Assert.IsArgNotNull(obj, "obj");

            EnsureInitialized();
            s_objectList.Remove(obj);
        }

        #endregion

        #region Private static methods

        private static void EnsureInitialized()
        {
            if (s_objectList != null) return;

            s_objectList    = new List<object>(capacity: 8);
        }

        #endregion
    }
}