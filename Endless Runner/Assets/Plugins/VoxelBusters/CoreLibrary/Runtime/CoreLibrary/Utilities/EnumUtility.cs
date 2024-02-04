using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.CoreLibrary
{
    public static class EnumUtility
    {
        #region Static methods

        public static string[] GetEnumNames(Type enumType, bool ignoreDefault = false)
        {
            var     list    = new List<string>();
            foreach (var item in Enum.GetValues(enumType))
            {
                list.Add(item.ToString());
            }
            if (ignoreDefault)
            {
                list.RemoveAt(0);
            }
            return list.ToArray();
        }

        public static T[] GetEnumValues<T>(bool ignoreDefault = false)
        {
            var     list    = new List<T>();
            foreach (var item in Enum.GetValues(typeof(T)))
            {
                list.Add((T)item);
            }
            if (ignoreDefault)
            {
                list.RemoveAt(0);
            }
            return list.ToArray();
        }

        #endregion
    }
}