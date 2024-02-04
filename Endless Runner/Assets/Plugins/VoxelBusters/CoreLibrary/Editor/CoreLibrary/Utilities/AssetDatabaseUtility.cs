using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace VoxelBusters.CoreLibrary.Editor
{
    public static class AssetDatabaseUtility
    {
        #region Resources methods

        public static void CreateFolder(string folder)
        {
            var     pathComponents  = folder.Split('/');

            string  currentPath     = string.Empty;
            for (int iter = 0; iter < pathComponents.Length; iter++)
            {
                string  component   = pathComponents[iter]; 
                string  newPath     = Path.Combine(currentPath, component);
                if (!AssetDatabase.IsValidFolder(newPath))
                {
                    AssetDatabase.CreateFolder(currentPath, component);
                }

                // update path
                currentPath         = newPath;
            }
        }

        public static void CreateAssetAtPath(Object asset,
                                             string assetPath)
        {
            // create container folder
            string  parentFolder    = assetPath.Substring(0, assetPath.LastIndexOf('/'));
            CreateFolder(parentFolder);

            // create asset
            AssetDatabase.CreateAsset(asset, assetPath);
        }

        public static T CreateScriptableObject<T>(string assetPath,
                                                  System.Func<T> createFunc = null,
                                                  System.Action<T> onInit = null) where T : ScriptableObject
        {
            var     instance    = (createFunc != null)
                ? createFunc()
                : ScriptableObject.CreateInstance<T>();
            onInit?.Invoke(instance);

            // create file
            CreateAssetAtPath(instance, assetPath);
            AssetDatabase.Refresh();

            return instance;
        }

        public static T LoadScriptableObject<T>(string assetPath,
                                                System.Action<T> onLoad = null,
                                                System.Func<System.Exception> throwErrorFunc = null) where T : ScriptableObject
        {
            var     instance    = AssetDatabase.LoadAssetAtPath<T>(assetPath);
            if (instance)
            {
                onLoad?.Invoke(instance);
                return instance;
            }

            if (throwErrorFunc != null)
            {
                throw throwErrorFunc();
            }

            return null;
        }

        public static T[] FindAssetObjects<T>() where T : ScriptableObject
        {
            var    guids   = AssetDatabase.FindAssets($"t:{typeof(T).Name}");
            return System.Array.ConvertAll(guids, (item) =>
            {
                string  path    = AssetDatabase.GUIDToAssetPath(item);
                return AssetDatabase.LoadAssetAtPath<T>(path);
            });
        }

        #endregion
    }
}