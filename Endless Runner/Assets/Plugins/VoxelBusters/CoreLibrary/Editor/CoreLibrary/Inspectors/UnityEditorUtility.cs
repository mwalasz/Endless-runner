using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace VoxelBusters.CoreLibrary.Editor
{
    public static class UnityEditorUtility
    {
        #region Static methods

        public static bool ShowFoldableHeader(string prefKeyName, string content, string tooltip = null)
        {
            bool isExpanded     = EditorGUILayout.Foldout(EditorPrefs.GetBool(prefKeyName, false), new GUIContent(content, tooltip));
            EditorPrefs.SetBool(prefKeyName, isExpanded);

            return isExpanded;
        }

        public static bool ShowFoldableHeader(SerializedProperty property, string displayName = null)
        {
            bool isExpanded     = EditorGUILayout.Foldout(property.isExpanded, new GUIContent(displayName ?? property.displayName, property.tooltip));
            property.isExpanded = isExpanded;

            return isExpanded;
        }

        public static void SetIsEditorDirty(bool value)
        {
            EditorPrefs.SetBool("editor-is-dirty", value);
        }

        public static bool GetIsEditorDirty()
        {
            return EditorPrefs.GetBool("editor-is-dirty", false);
        }

        #endregion

        #region Mask field methods

        public static void EnumFlagsField(Rect position, GUIContent label, SerializedProperty property, Type type)
        {
            property.intValue   = EnumFlagsField(position, label, property.intValue, type);
        }

        public static int EnumFlagsField(Rect position, GUIContent label, int value, Type type)
        {
            EditorGUI.BeginChangeCheck();
#if UNITY_2017_3_OR_NEWER
            Enum    newValue    = EditorGUI.EnumFlagsField(position, label, GetValueAsEnum(value, type));
#else
            Enum    newValue    = EditorGUI.EnumMaskField(position, label, GetValueAsEnum(value, type));
#endif
            if (EditorGUI.EndChangeCheck())
            {
                return GetEnumAsInt(newValue, type);
            }
            return value;
        }

        public static T EnumFlagsField<T>(Rect position, string label, T value)
        {
            return (T)(object)EnumFlagsField(position, new GUIContent(label), (int)(object)value, typeof(T));
        }

        #endregion

        #region Private static methods

        private static Array GetEnumValues(Type type)
        {
            return Enum.GetValues(type);
        }

        private static Enum GetValueAsEnum(int value, Type type)
        {
            return (Enum)Enum.ToObject(type, value);
        }

        private static int GetEnumAsInt(Enum value, Type type)
        {
            int  newValueInt = Convert.ToInt32(value);

            // if "Everything" is set, force Unity to unset the extra bits by iterating through them
            if (newValueInt < 0)
            {
                int bits = 0;
                foreach (var enumValue in GetEnumValues(type))
                {
                    int checkBit = newValueInt & (int)enumValue;
                    if (checkBit != 0)
                    {
                        bits  |= (int)enumValue;
                    }
                }
                newValueInt = bits;
            }
            return newValueInt;
        }

        #endregion
    }
}