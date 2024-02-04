using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.CoreLibrary.Editor
{
    [CustomPropertyDrawer(typeof(FileBrowserAttribute))]
    public class FileBrowserAttributeDrawer : PropertyDrawer 
    {
        #region Base class methods

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            // draw property
            var valueRect   = new Rect(position.x, position.y, position.width - 50, position.height);
            var buttonRect  = new Rect(position.xMax - 45, position.y, 45, position.height);

            EditorGUI.PropertyField(valueRect, property, label);
            if (GUI.Button(buttonRect, new GUIContent("Select")))
            {   
                EditorApplication.delayCall += () =>
                {
                    OpenFileBrowser(property);
                }; 
            }

            EditorGUI.EndProperty();
        }

        #endregion

        #region Private methods
            
        private void OpenFileBrowser(SerializedProperty property)
        {
            var value   = EditorUtility.OpenFilePanel("Select file", IOServices.GetAbsolutePath(property.stringValue), ((FileBrowserAttribute)attribute).Extension);
            if (!string.IsNullOrEmpty(value))
            {
                property.stringValue    = ((FileBrowserAttribute)attribute).UsesRelativePath ? IOServices.GetRelativePath(IOServices.GetAbsolutePath(""), value) : value;
                UnityEditorUtility.SetIsEditorDirty(true);
            }
        }

        #endregion
    }
}