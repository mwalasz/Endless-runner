using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace VoxelBusters.CoreLibrary.Editor
{
    public class SettingsObjectEditorManager
    {
        [InitializeOnLoadMethod]
        private static void OnEditorReload()
        {
             var    settingsObjects = AssetDatabaseUtility.FindAssetObjects<SettingsObject>();  
             foreach (var obj in settingsObjects)         
             {
                obj.OnEditorReload();
             }
        }
    }
}