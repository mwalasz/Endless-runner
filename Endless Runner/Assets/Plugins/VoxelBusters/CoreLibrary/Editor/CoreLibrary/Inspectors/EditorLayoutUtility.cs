using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace VoxelBusters.CoreLibrary.Editor
{
    public static class EditorLayoutUtility
    {
        #region Static methods

        public static void DisableGUI()
        {
            GUI.enabled = false;
        }

        public static void EnableGUI()
        {
            GUI.enabled = true;
        }

        public static bool TransparentButton(Rect rect,
                                             string label = "")
        {
            var     originalColor   = GUI.color;
            try
            {
                GUI.color   = Color.clear;
                return GUI.Button(rect, label);
            }
            finally
            {
                GUI.color   = originalColor;
            }
        }

        public static void StringPopup(int selectedIndex,
                                       string[] options,
                                       Callback<int> onValueChange,
                                       params GUILayoutOption[] layoutOptions)
        {
            int     newValue    = EditorGUILayout.Popup(selectedIndex, options, layoutOptions);
            if (newValue != selectedIndex)
            {
                onValueChange?.Invoke(newValue);
            }
        }

        public static void StringPopup(SerializedProperty stringProperty,
                                       ref int selectedIndex,
                                       string[] displayedOptions,
                                       Callback<int> onValueChange = null,
                                       params GUILayoutOption[] layoutOptions)
        {
            int     newValue    = EditorGUILayout.Popup(new GUIContent(stringProperty.displayName, stringProperty.tooltip),
                                                        selectedIndex,
                                                        displayedOptions,
                                                        layoutOptions);
            if (newValue != selectedIndex)
            {
                selectedIndex   = newValue;
                stringProperty.stringValue  = displayedOptions[newValue];

                onValueChange?.Invoke(newValue);
            }
        }

        public static void Helpbox(string title,
                                   string description,
                                   System.Action drawFunc,
                                   GUIStyle style)
        {
            GUILayout.BeginVertical(style);
            GUILayout.Label(title, EditorStyles.boldLabel);
            GUILayout.Label(description, EditorStyles.wordWrappedLabel);
            drawFunc();
            GUILayout.EndVertical();
        }

        public static void Helpbox(string title,
                                   string description,
                                   string actionLabel,
                                   System.Action onClick,
                                   GUIStyle style)
        {
            Helpbox(
                title: title,
                description: description,
                drawFunc: () =>
                {
                    if (GUILayout.Button(actionLabel))
                    {
                        onClick?.Invoke();
                    }
                },
                style: style);
        }

        public static void BeginBottomBar(Color? borderColor = null,
                                          params GUILayoutOption[] options)
        {
            var     bottomBarRect   = EditorGUILayout.BeginHorizontal(options);
            bottomBarRect.height    = 1f;
            if (borderColor != null)
            {
                EditorGUI.DrawRect(bottomBarRect, borderColor.Value);
            }
        }

        public static void EndBottomBar()
        {
            EditorGUILayout.EndHorizontal();
        }

        #endregion
    }
}
