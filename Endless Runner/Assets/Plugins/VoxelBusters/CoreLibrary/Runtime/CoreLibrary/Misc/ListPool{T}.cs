using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.CoreLibrary
{
    public static class ListPool<T>
    {
        #region Static fields

        private static ObjectPool<List<T>> s_listObjectPool;

        #endregion

        #region Static methods

        public static List<T> Get()
        {
            EnsureInitialized();

            return s_listObjectPool.Get();
        }

        public static void Release(List<T> obj)
        {
            EnsureInitialized();

            s_listObjectPool.Add(obj);
        }

        #endregion

        #region Private static methods

        private static void EnsureInitialized()
        {
            if (s_listObjectPool != null) return;

            s_listObjectPool    = new ObjectPool<List<T>>(
                createFunc: OnCreateItem,
                actionOnGet: null,
                actionOnAdd: OnReleaseItem);
        }

        private static List<T> OnCreateItem()
        {
            return new List<T>(capacity: 8);
        }

        private static void OnReleaseItem(List<T> item)
        {
            item.Clear();
        }

        #endregion
    }
}