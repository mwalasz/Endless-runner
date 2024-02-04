using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.CoreLibrary.Helpers
{
    public class DefaultSaveServiceProvider : ISaveServiceProvider
    {
        #region Static methods

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void OnLoad()
        {
            if (ExternalServiceProvider.SaveServiceProvider != null)
            {
                return;
            }

            // set default service provider
            ExternalServiceProvider.SaveServiceProvider = new DefaultSaveServiceProvider();
        }

        #endregion

        #region ISaveServiceProvider implementation

        public int GetInt(string key, int defaultValue = 0)
        {
            return PlayerPrefs.GetInt(key, defaultValue);
        }

        public float GetFloat(string key, float defaultValue = 0f)
        {
            return PlayerPrefs.GetFloat(key, defaultValue);
        }

        public string GetString(string key, string defaultValue = null)
        {
            return PlayerPrefs.GetString(key, defaultValue);
        }

        public string[] GetStringArray(string key, string[] defaultValue = null)
        {
            string      arrayStr    = PlayerPrefs.GetString(key, null);
            if (!string.IsNullOrEmpty(arrayStr))
            {
                IList   jsonList    = (IList)ExternalServiceProvider.JsonServiceProvider.FromJson(arrayStr);

                // convert to string array
                string[]    array   = new string[jsonList.Count];
                for (int iter = 0; iter < jsonList.Count; iter++)
                {
                    array[iter]     = (string)jsonList[iter];
                }
                return array;
            }

            return null;
        }
        
        public void SetInt(string key, int value)
        {
            PlayerPrefs.SetInt(key, value);
        }

        public void SetFloat(string key, float value)
        {
            PlayerPrefs.SetFloat(key, value);
        }

        public void SetString(string key, string value)
        {
            PlayerPrefs.SetString(key, value);
        }

        public void SetStringArray(string key, string[] value)
        {
            string  arrayStr    = (null == value) ? null : ExternalServiceProvider.JsonServiceProvider.ToJson(value);
            PlayerPrefs.SetString(key, arrayStr);
        }

        public void RemoveKey(string key)
        {
            PlayerPrefs.DeleteKey(key);
        }

        public void Save()
        {
            PlayerPrefs.Save();
        }

        #endregion
    }
}