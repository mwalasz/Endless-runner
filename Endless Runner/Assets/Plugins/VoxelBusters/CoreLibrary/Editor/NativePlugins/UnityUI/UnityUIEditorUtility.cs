using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using VoxelBusters.CoreLibrary.Editor;
using VoxelBusters.CoreLibrary.NativePlugins;
using VoxelBusters.CoreLibrary.NativePlugins.UnityUI;

namespace VoxelBusters.CoreLibrary.Editor.NativePlugins.UnityUI
{
    public static class UnityUIEditorUtility
    {
        #region Static methods

        public static UnityUIRenderer LoadRendererPrefab()
        {
            return LoadPrefab<UnityUIRenderer>("UnityUIRenderer.prefab");
        }

        public static UnityUIAlertDialog LoadAlertDialogPrefab()
        {
            return LoadPrefab<UnityUIAlertDialog>("UnityUIAlertDialog.prefab");
        }

        private static T LoadPrefab<T>(string name) where T : Object
        {
            string  prefabPath  = CoreLibrarySettings.Package.GetFullPath($"Prefabs/{name}");
            return AssetDatabase.LoadAssetAtPath<T>(prefabPath);
        }

        #endregion
    }
}