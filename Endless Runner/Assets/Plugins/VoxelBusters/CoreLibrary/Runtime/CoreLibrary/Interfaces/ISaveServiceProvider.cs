using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.CoreLibrary
{
    /// <summary>
    /// Adapter interface for supporting save service compatible with the plugin.
    /// </summary>
    public interface ISaveServiceProvider
    {
        #region Methods

        int GetInt(string key, int defaultValue = 0);

        float GetFloat(string key, float defaultValue = 0f);

        string GetString(string key, string defaultValue = null);

        string[] GetStringArray(string key, string[] defaultValue = null);
        
        void SetInt(string key, int value);

        void SetFloat(string key, float value);

        void SetString(string key, string value);

        void SetStringArray(string key, string[] value);

        void RemoveKey(string key);

        void Save();

        #endregion
    }
}