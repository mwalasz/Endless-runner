using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.CoreLibrary
{
    public static class ResourcesUtility
    {
        #region Static methods

        public static T LoadBuiltinAsset<T>(string name) where T : Object
        {
            return Resources.Load<T>(name);
        }

        public static T LoadAsset<T>(this UnityPackageDefinition package, string name) where T : Object
        {
            string path = $"{package.GetMutableResourcesPath()}/{name}";
            return Resources.Load<T>(path);
        }

        #endregion
    }
}