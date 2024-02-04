using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.CoreLibrary
{
    /// <summary>
    /// Object represents a container to store key-value pairs.
    /// </summary>
    public class KeyValueDataStore
    {
        #region Fields

        private     Dictionary<string, string>      m_dataCollection;

        private     string                          m_savePath;

        #endregion

        #region Constructors

        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="savePath">The file path to save contents.</param>
        public KeyValueDataStore(string savePath)
        {
            // set properties
            m_dataCollection    = LoadDataFromPath(savePath) ?? new Dictionary<string, string>();
            m_savePath          = savePath;
        }

        #endregion

        #region Get value methods

        /// <summary>
        /// Returns the boolean value associated with the specified key.
        /// </summary>
        /// <param name="key">A string used to identify the value stored in the data store.</param>
        /// <param name="defaultValue">Default value.</param>
        /// <returns></returns>
        public bool GetBool(string key, bool defaultValue = default)
        {
            if (m_dataCollection.TryGetValue(key, out string value))
            {
                return bool.Parse(value);
            }
            return defaultValue;
        }

        /// <summary>
        /// Returns the long value associated with the specified key.
        /// </summary>
        /// <param name="key">A string used to identify the value stored in the data store.</param>
        /// <param name="defaultValue">Default value.</param>
        /// <returns></returns>
        public long GetLong(string key, long defaultValue = default)
        {
            if (m_dataCollection.TryGetValue(key, out string value))
            {
                return long.Parse(value);
            }
            return defaultValue;
        }

        /// <summary>
        /// Returns the double value associated with the specified key.
        /// </summary>
        /// <param name="key">A string used to identify the value stored in the data store.</param>
        /// <param name="defaultValue">Default value.</param>
        /// <returns></returns>
        public double GetDouble(string key, double defaultValue = default)
        {
            if (m_dataCollection.TryGetValue(key, out string value))
            {
                return double.Parse(value);
            }
            return defaultValue;
        }

        /// <summary>
        /// Returns the string value associated with the specified key.
        /// </summary>
        /// <param name="key">A string used to identify the value stored in the data store.</param>
        /// <param name="defaultValue">Default value.</param>
        /// <returns></returns>
        public string GetString(string key, string defaultValue = default)
        {
            if (m_dataCollection.TryGetValue(key, out string value))
            {
                return value;
            }
            return defaultValue;
        }

        /// <summary>
        /// Returns the byte array object associated with the specified key.      
        /// </summary>
        /// <param name="key">A string used to identify the value stored in the data store.</param>
        /// <param name="defaultValue">Default value.</param>
        /// <returns></returns>
        public byte[] GetByteArray(string key, byte[] defaultValue = default)
        {
            if (m_dataCollection.TryGetValue(key, out string value))
            {
                return System.Convert.FromBase64String(value);
            }
            return defaultValue;
        }

        /// <summary>
        /// Returns the complete snapshot of data.      
        /// </summary>
        /// <returns>Returns IDictionary with snapshot data</returns>
        public IDictionary GetSnapshot()
        {
            return m_dataCollection;
        }

        #endregion

        #region Set value methods

        /// <summary>
        /// Sets a boolean value for the specified key in the data store.
        /// </summary>
        /// <param name="key">The key under which to store the value.</param>
        /// <param name="value">The boolean value to store.</param>
        public void SetBool(string key, bool value)
        {
            // save value
            m_dataCollection[key]   = value.ToString();
        }

        /// <summary>
        /// Sets a long value for the specified key in the data store.
        /// </summary>
        /// <param name="key">The key under which to store the value.</param>
        /// <param name="value">The long value to store.</param>
        public void SetLong(string key, long value)
        {
            // save value
            m_dataCollection[key]   = value.ToString();
        }

        /// <summary>
        /// Sets a double value for the specified key in the data store.
        /// </summary>
        /// <param name="key">The key under which to store the value.</param>
        /// <param name="value">The double value to store.</param>
        public void SetDouble(string key, double value)
        {
            // save value
            m_dataCollection[key]   = value.ToString();
        }

        /// <summary>
        /// Sets a string value for the specified key in the data store.
        /// </summary>
        /// <param name="key">The key under which to store the value.</param>
        /// <param name="value">The string value to store.</param>
        public void SetString(string key, string value)
        {
            // save value
            m_dataCollection[key]   = value;
        }

        /// <summary>
        /// Sets a byte array value for the specified key in the data store.
        /// </summary>
        /// <param name="key">The key under which to store the value.</param>
        /// <param name="value">The long value to store.</param>
        public void SetByteArray(string key, byte[] value)
        {
            // save value
            m_dataCollection[key]   = System.Convert.ToBase64String(value);
        }

        #endregion

        #region Misc methods

        /// <summary>
        /// Explicitly synchronizes in-memory data with those stored on disk.
        /// </summary>
        public void Synchronize()
        {
            string  jsonContent = ExternalServiceProvider.JsonServiceProvider.ToJson(m_dataCollection);
            IOServices.CreateFile(m_savePath, jsonContent);
        }

        /// <summary>
        /// Removes all the entries from the data store.
        /// </summary>
        public void Clear()
        {
            m_dataCollection.Clear();
            Synchronize();
        }

        /// <summary>
        /// Removes the value associated with the specified key from the data store.
        /// </summary>
        /// <param name="key">The key corresponding to the value you want to remove.</param>
        public bool RemoveKey(string key)
        {
            return m_dataCollection.Remove(key);
        }

        #endregion

        #region Private methods

        private Dictionary<string, string> LoadDataFromPath(string path)
        {
            if (!IOServices.FileExists(path)) return null;

            var     jsonContent = IOServices.ReadFile(path);
            return ExternalServiceProvider.JsonServiceProvider.FromJson(jsonContent) as Dictionary<string, string>;
        }

        #endregion
    }
}