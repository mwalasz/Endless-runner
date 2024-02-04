using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.CoreLibrary
{
    /// <summary>
    /// Adapter interface for supporting json conversion compatible with the plugin.
    /// </summary>
    public interface IJsonServiceProvider
    {
        #region Methods

        /// <summary>
        /// Generate a JSON representation of the given object.
        /// </summary>
        /// <returns>The json.</returns>
        /// <param name="obj">Object.</param>
        string ToJson(object obj);

        /// <summary>
        /// Create an object from specified JSON representation.
        /// </summary>
        /// <returns>The object.</returns>
        /// <param name="jsonString">Json string.</param>
        object FromJson(string jsonString);

        #endregion
    }
}