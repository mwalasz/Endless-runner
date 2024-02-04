using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace VoxelBusters.CoreLibrary.Editor
{
    public class CustomInspector : UnityEditor.Editor
    {
        #region Unity methods

        protected virtual void OnEnable()
        { }

        protected virtual void OnDisable()
        { }

        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();

            DrawCustomInspector();

            bool    isDirty     = EditorGUI.EndChangeCheck();
            if (isDirty || UnityEditorUtility.GetIsEditorDirty())
            {
                serializedObject.ApplyModifiedProperties();
                UnityEditorUtility.SetIsEditorDirty(false);
            }
        }

        protected virtual void DrawCustomInspector()
        {
            var     property    = serializedObject.GetIterator();
            if (property.NextVisible(true))
            {
                if (property.name == "m_Script")
                {
                    GUI.enabled = false;
                    EditorGUILayout.PropertyField(property, true);
                    GUI.enabled = true;
                }
                while (property.NextVisible(false))
                {
                    EditorGUILayout.PropertyField(property, true);
                }
            }
        }

        #endregion
    }
}