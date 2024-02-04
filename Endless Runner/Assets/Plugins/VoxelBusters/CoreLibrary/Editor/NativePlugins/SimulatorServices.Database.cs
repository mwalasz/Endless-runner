using System;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.CoreLibrary.Editor.NativePlugins
{
    public partial class SimulatorServices
    {
        [Serializable]
        public class Database
        {
            #region Fields

            [SerializeField]
            private     List<StringKeyValuePair>    m_dataItems     = new List<StringKeyValuePair>();

            #endregion

            #region Data methods

            public void SetObject(string key, object obj)
            {
                // convert object to serialized form
                string  serializedData      = JsonUtility.ToJson(obj);
                var     newItem             = new StringKeyValuePair() { Key = key, Value = serializedData };

                // check whether key exists
                int     keyIndex            = FindItemIndex(key);
                if (keyIndex == -1)
                {
                    m_dataItems.Add(newItem);
                }
                else
                {
                    m_dataItems[keyIndex]   = newItem;
                }
            }

            public T GetObject<T>(string key)
            {
                int     keyIndex            = FindItemIndex(key);
                if (keyIndex != -1)
                {
                    string  serializedData  = m_dataItems[keyIndex].Value;
                    return JsonUtility.FromJson<T>(serializedData);
                }

                return default(T);
            }

            public void RemoveObject(string key)
            {
                int     keyIndex            = FindItemIndex(key);
                if (keyIndex != -1)
                {
                    m_dataItems.RemoveAt(keyIndex);
                }
            }

            public void RemoveAllObjects()
            {
                // remove existing data
                m_dataItems.Clear();
            }

            #endregion

            #region Private methods

            private int FindItemIndex(string key)
            {
                return m_dataItems.FindIndex((item) => string.Equals(item.Key, key));
            }

            #endregion
        }
    }
}