using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.CoreLibrary.Editor
{
    [CustomPropertyDrawer(typeof(StringPopupAttribute), true)]
    public class StringPopupAttributeDrawer : PropertyDrawer 
    {
        #region Base class methods

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            label   = EditorGUI.BeginProperty(position, label, property);

            // determine whether popup is required
            bool    canShowPopup    = true;
            var     popupAttribute  = (StringPopupAttribute)attribute;
            if (popupAttribute.PreferencePropertyName != null)
            {
                var     preferencePropertyPath  = property.propertyPath.Replace(property.name, popupAttribute.PreferencePropertyName);
                var     preferenceProperty      = property.serializedObject.FindProperty(preferencePropertyPath);
                canShowPopup                    = (preferenceProperty != null) && (preferenceProperty.boolValue == popupAttribute.PreferencePropertyValue);
            }

            // draw property as per preference
            if (canShowPopup)
            {
                var     options         = popupAttribute.Options;
                int     selectedIndex   = Array.FindIndex(options, (item) => string.Equals(item, property.stringValue));
                selectedIndex           = EditorGUI.Popup(position, label.text, selectedIndex, options);

                // assign value
                property.stringValue    = (selectedIndex == -1) ? string.Empty : options[selectedIndex];
            }
            else
            {
                EditorGUI.PropertyField(position, property);
            }

            EditorGUI.EndProperty();
        }

        #endregion
    }
}