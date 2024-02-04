#if UNITY_IOS || UNITY_TVOS
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.iOS.Xcode;

namespace VoxelBusters.CoreLibrary.Editor.NativePlugins.Build.Xcode
{
    public static class PlistUtility
    {
        #region Plist extensions

        public static bool TryGetElement<T>(this PlistElementDict dict, string key, out T element) where T : PlistElement
        {
            IDictionary<string, PlistElement>   dictionary  = dict.values;
            PlistElement                        value;
            if (dictionary.TryGetValue(key, out value))
            {
                element = (T)value;
                return true;
            }

            element     = default(T);
            return false;
        }

        public static bool Contains(this PlistElementArray array, string value)
        {
            List<PlistElement>   valueList  = array.values;
            for (int iter = 0; iter < valueList.Count; iter++)
            {
                if (valueList[iter].AsString() == value)
                {
                    return true;
                }
            }

            return false;
        }

        public static void AddUniqueString(this PlistElementArray array, string value)
        {
            foreach (PlistElement each in array.values)
            {
                if (each is PlistElementString)
                {
                    string existingValue = each.AsString();
                    if (!string.IsNullOrEmpty(existingValue) && existingValue.Equals(value))
                    {
                       return;
                    }
                }
            }

            // We reached here as value doesn't exist
            array.AddString(value);
        }

        // This add new array if the key doesn't exist. If exists, returns it.
        public static PlistElementArray GetArray(this PlistElementDict source, string key)
        {
            PlistElementArray  array = null;
            if (source.values.ContainsKey(key))
            {
                array   = source.values[key] as PlistElementArray;
            }
            if (array == null)
            {
                array   = source.CreateArray(key);
            }
            return array;
        }

        #endregion
    }
}
#endif