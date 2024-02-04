using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
#if UNITY_2019_1_OR_NEWER
using UnityEngine.UIElements;
#else
using UnityEngine.Experimental.UIElements;
#endif

namespace VoxelBusters.CoreLibrary.Editor
{
    public class SettingsProviderZ : SettingsProvider
    {
        #region Fields

        private     SettingsObject              m_settingsObject;

        private     SettingsObjectInspector     m_settingsObjectInspector;

        #endregion

        #region Constructors

        private SettingsProviderZ(SettingsObject settingsObject, string path, SettingsScope scopes)
            : base(path, scopes)
        {
            // set properties
            keywords                    = GetSearchKeywordsFromSerializedObject(new SerializedObject(settingsObject));
            m_settingsObject            = settingsObject;
            m_settingsObjectInspector   = null;
        }

        #endregion

        #region Create methods

        public static SettingsProviderZ Create(SettingsObject settingsObject, string path, SettingsScope scopes)
        {
            return new SettingsProviderZ(settingsObject, path, scopes);
        }

        #endregion

        #region Base class methods

        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            base.OnActivate(searchContext, rootElement);

            m_settingsObjectInspector   = UnityEditor.Editor.CreateEditor(m_settingsObject) as SettingsObjectInspector;
        }

        public override void OnTitleBarGUI()
        {
            EditorGUILayout.InspectorTitlebar(false, m_settingsObject);
        }

        public override void OnGUI(string searchContext)
        {
            if (m_settingsObjectInspector == null)
            {
                return;
            }
            
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(10f);
            EditorGUILayout.BeginVertical();
            m_settingsObjectInspector.OnInspectorGUI();
            EditorGUILayout.EndVertical();
            GUILayout.Space(10f);
            EditorGUILayout.EndHorizontal();
        }

        public override void OnDeactivate()
        {
            base.OnDeactivate();

            if (m_settingsObjectInspector)
            {
                Object.DestroyImmediate(m_settingsObjectInspector);
                m_settingsObjectInspector   = null;
            }
        }

        #endregion

    }
}