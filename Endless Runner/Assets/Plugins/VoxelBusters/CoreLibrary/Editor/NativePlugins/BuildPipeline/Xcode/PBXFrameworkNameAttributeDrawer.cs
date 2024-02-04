using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;

namespace VoxelBusters.CoreLibrary.Editor.NativePlugins.Build
{
    [CustomPropertyDrawer(typeof(PBXFrameworkNameAttribute))]
    public class PBXFrameworkNameAttributeDrawer : PropertyDrawer 
    {
        #region Unity methods

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            label   = EditorGUI.BeginProperty(position, label, property);

            // show popup
            string[]    options         = ((PBXFrameworkNameAttribute)attribute).GetFrameworkNames();
            int         selectedIndex   = Array.FindIndex(options, (item) => string.Equals(item, property.stringValue));
            selectedIndex               = EditorGUI.Popup(position, label.text, selectedIndex, options);

            // assign value
            property.stringValue        = (selectedIndex == -1) ? options[0] : options[selectedIndex];

            EditorGUI.EndProperty();
        }

        #endregion
    }
}