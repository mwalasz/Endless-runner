using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace VoxelBusters.CoreLibrary.Editor
{
    public class EditorLayoutBuilder
    {
        #region Fields

        private     SerializedObject            m_serializedObject;

        private     string[]                    m_tabs;

        private     GetSectionsCallback         m_getSectionsCallback;

        private     DrawCallback                m_drawTopBarCallback;

        private     DrawTabViewCallback         m_drawTabViewCallback;

        private     DrawCallback                m_drawFooterCallback;

        private     string                      m_selectedTab;

        private     EditorSectionInfo[]         m_selectedTabSections;

        private     EditorSectionInfo           m_focusSection;

        private     Vector2                     m_tabBarScrollPosition;

        private     Texture2D                   m_toggleOnIcon;

        private     Texture2D                   m_toggleOffIcon;

        private     GUIStyle                    m_backgroundStyle;

        private     GUIStyle                    m_titleLabelStyle;

        private     GUIStyle                    m_subtitleLabelStyle;

        private     GUIStyle                    m_tabBarLabelNormalStyle;

        private     GUIStyle                    m_tabBarLabelSelectedStyle;

        private     GUIStyle                    m_selectableLabelStyle;

        private     GUIStyle                    m_invisibleButtonStyle;

        #endregion

        #region Delegates

        public delegate EditorSectionInfo[] GetSectionsCallback(string tab);

        public delegate void DrawCallback(string tab);

        public delegate bool DrawTabViewCallback(string tab);

        #endregion

        #region Events

        public event Callback<EditorSectionInfo> OnSectionStatusChange;

        public event Callback<EditorSectionInfo> OnFocusSectionValueChange;

        #endregion

        #region Constructors

        public EditorLayoutBuilder(SerializedObject serializedObject,
                                   string[] tabs,
                                   GetSectionsCallback getSectionsCallback,
                                   DrawCallback drawTopBarCallback,
                                   DrawTabViewCallback drawTabViewCallback,
                                   DrawCallback drawFooterCallback,
                                   Texture2D toggleOnIcon,
                                   Texture2D toggleOffIcon)
        {
            // Set properties
            m_serializedObject          = serializedObject;
            m_tabs                      = tabs;
            m_getSectionsCallback       = getSectionsCallback;
            m_drawTopBarCallback        = drawTopBarCallback;
            m_drawTabViewCallback       = drawTabViewCallback;
            m_drawFooterCallback        = drawFooterCallback;
            m_toggleOnIcon              = toggleOnIcon;
            m_toggleOffIcon             = toggleOffIcon;

            LoadStyles();
            SetSelectedTab(m_tabs[0]);
        }

        #endregion

        #region Static methods

        public static void DrawChildProperties(SerializedProperty property,
                                               string prefix = null,
                                               bool indent = true,
                                               params string[] ignoreProperties)
        {
            try
            {
                if (indent)
                {
                    EditorGUI.indentLevel++;
                }

                // Move pointer to first element
                var     currentProperty  = property.Copy();
                var     endProperty      = default(SerializedProperty);

                // Start iterating through the properties
                bool    firstTime       = true;
                while (currentProperty.NextVisible(enterChildren: firstTime))
                {
                    if (firstTime)
                    {
                        endProperty      = property.GetEndProperty();
                        firstTime        = false;
                    }
                    if (SerializedProperty.EqualContents(currentProperty, endProperty))
                    {
                        break;
                    }

                    // Exclude specified properties
                    if ((ignoreProperties != null) && System.Array.Exists(ignoreProperties, (item) => string.Equals(item, currentProperty.name)))
                    {
                        continue;
                    }

                    // Display the property
                    if (prefix != null)
                    {
                        EditorGUILayout.PropertyField(currentProperty, new GUIContent($"{prefix} {currentProperty.displayName}", currentProperty.tooltip), true);
                    }
                    else
                    {
                        EditorGUILayout.PropertyField(currentProperty, true);
                    }
                }
            }
            finally
            {
                if (indent)
                {
                    EditorGUI.indentLevel--;
                }
            }
        }

        #endregion

        #region Public methods

        public void DoLayout()
        {
            if (CanShowFocusSection())
            {
                DrawFocusSection();
            }
            else
            {
                m_drawTopBarCallback?.Invoke(m_selectedTab);
                DrawTabBar();
                DrawTabView();
                m_drawFooterCallback?.Invoke(m_selectedTab);
            }
            m_serializedObject.ApplyModifiedProperties();
            m_serializedObject.Update();
        }

        public void DrawSection(EditorSectionInfo section,
                                bool showDetails = true,
                                bool selectable = true)
        {
            EditorGUILayout.BeginVertical(m_backgroundStyle);
            bool    expanded        = DrawSectionHeader(section,
                                                        selectable);
            bool    endGroup        = true;
            if (showDetails || expanded)
            {
                if (section.DrawStyle == EditorSectionDrawStyle.Default)
                {
                    endGroup        = false;
                    EditorGUILayout.EndVertical();
                }

                if (section.DrawDetailsCallback != null)
                {
                    section.DrawDetailsCallback(section);
                }
                else
                {
                    if (section.DrawStyle == EditorSectionDrawStyle.Default)
                    {
                        endGroup    = true;
                        EditorGUILayout.BeginVertical(m_backgroundStyle);
                    }
                    DrawSectionDetails(section);
                }
            }
            if (endGroup)
            {
                EditorGUILayout.EndVertical();
            }
        }

        public void DrawSectionDetails(EditorSectionInfo section)
        {
            bool    originalGUIState    = GUI.enabled;
            try
            {
                // Update edit capability
                GUI.enabled     = section.IsEnabled;

                // Draw section properties
                DrawChildProperties(property: section.Property,
                                    ignoreProperties: section.IgnoreProperties);
            }
            finally
            {
                // Reset gui state
                GUI.enabled     = originalGUIState;
            }
        }

        public void BeginVertical()
        {
            EditorGUILayout.BeginVertical(m_backgroundStyle);
        }

        public void EndVertical()
        {
            EditorGUILayout.EndVertical();
        }

        #endregion

        #region Private methods

        private void LoadStyles()
        {
            m_backgroundStyle           = CustomEditorStyles.GroupBackground();
            m_titleLabelStyle           = CustomEditorStyles.Heading2Label();
            m_subtitleLabelStyle        = CustomEditorStyles.OptionsLabel(wordWrap: false);
            m_tabBarLabelNormalStyle    = CustomEditorStyles.SelectableLabel(fontSize: 16, textColor: Color.gray);
            m_tabBarLabelSelectedStyle  = CustomEditorStyles.SelectableLabel(fontSize: 16, fontStyle: FontStyle.Bold);
            m_selectableLabelStyle      = CustomEditorStyles.SelectableLabel();
            m_invisibleButtonStyle      = CustomEditorStyles.InvisibleButton();
        }

        private bool CanShowFocusSection()
        {
            return (m_focusSection != null) && (m_focusSection.DrawStyle == EditorSectionDrawStyle.Default);
        }

        private void DrawTabBar()
        {
            if (m_tabs.Length > 1)
            {
                m_tabBarScrollPosition  = GUILayout.BeginScrollView(m_tabBarScrollPosition, m_backgroundStyle, GUILayout.Height(30f));
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                for (int iter = 0; iter < m_tabs.Length; iter++)
                {
                    var     current     = m_tabs[iter];
                    var     labelStyle  = (current == m_selectedTab) ? m_tabBarLabelSelectedStyle : m_tabBarLabelNormalStyle;
                    if (GUILayout.Button(current, labelStyle, GUILayout.Width(80f)))
                    {
                        SetSelectedTab(current);
                    }
                }
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
                GUILayout.EndScrollView();
            }
        }

        private void DrawTabView()
        {
            if ((m_drawTabViewCallback == null) || !m_drawTabViewCallback(m_selectedTab))
            {
                for (int iter = 0; iter < m_selectedTabSections.Length; iter++)
                {
                    var     current = m_selectedTabSections[iter];
                    DrawSection(section: current,
                                showDetails: (current.DrawStyle == EditorSectionDrawStyle.Expand) && (current == m_focusSection));
                }
            }
        }

        private bool DrawSectionHeader(EditorSectionInfo section,
                                       bool selectable = true)
        {
            bool    isSelected          = (section == m_focusSection);
            bool    hasSubtitle         = (section.Description != null);
            float   headerHeight        = hasSubtitle ? 52f : 30f;

            // Draw rect
            var     rect                = EditorGUILayout.GetControlRect(false, headerHeight);
            //GUI.Box(rect, GUIContent.none, HeaderButtonStyle);

            /*
            // Draw expand control
            if (drawStyle == PropertyGroupDrawStyle.Expand)
            {
                var     foldOutRect         = new Rect(rect.x + 5f, rect.y, 50f, rect.height);
                EditorGUI.LabelField(foldOutRect, isSelected ? "-" : "+", CustomEditorStyles.Heading3);
            }
            */

            // Draw text 
            var     titleRect           = new Rect(rect.x + 5f,
                                                   rect.y + 4f,
                                                   rect.width * 0.8f,
                                                   22f);
            EditorGUI.LabelField(titleRect, section.DisplayName, m_titleLabelStyle);
            if (hasSubtitle)
            {
                var     subtitleRect    = new Rect(rect.x + 5f,
                                                   rect.y + 30f,
                                                   rect.width * 0.8f,
                                                   18f);
                EditorGUI.LabelField(subtitleRect, section.Description, m_subtitleLabelStyle);
            }

            // Draw selectable rect
            var     selectableRect      = new Rect(rect.x,
                                                   rect.y,
                                                   rect.width * 0.8f,
                                                   rect.height);
            if (selectable && EditorLayoutUtility.TransparentButton(selectableRect))
            {
                isSelected              = SetFocusSection(section);
            }

            // Draw toggle button
            var     enabledProperty     = section.EnabledProperty;
            if (enabledProperty != null)
            {
                var     toggleIcon      = enabledProperty.boolValue ? m_toggleOnIcon : m_toggleOffIcon;
                var     iconSize        = new Vector2(64f, 32f);
                var     toggleRect      = new Rect(rect.xMax - (iconSize.x * 1.2f),
                                                   titleRect.yMin + ((titleRect.height - iconSize.y/2)),
                                                   iconSize.x,
                                                   iconSize.y);
                if (GUI.Button(toggleRect, toggleIcon, m_invisibleButtonStyle))
                {
                    enabledProperty.boolValue       = !enabledProperty.boolValue;
                     
                    // Raise an event to notify others, delay is added to ensure that modified properties are serialized
                    EditorApplication.delayCall    += () => { OnSectionStatusChange?.Invoke(section); };
                }
            }
            return isSelected;
        }

        private void DrawFocusSection()
        {
            var     property    = m_focusSection.Property;
            EditorGUILayout.BeginHorizontal(m_backgroundStyle);
            if (GUILayout.Button($"{'\u2190'} Back To Main Menu", m_titleLabelStyle))
            {
                SetFocusSection(null);
                return;
            }
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            property.isExpanded = true;
            DrawSection(section: m_focusSection,
                        selectable: false);
        }

        #endregion

        #region Getter methods

        private EditorSectionInfo[] GetSections(string tab)
        {
            return m_getSectionsCallback(tab) ?? new EditorSectionInfo[0];
        }

        #endregion

        #region Setter methods

        private void SetSelectedTab(string tab)
        {
            // Update cached data
            m_selectedTab           = tab;
            m_selectedTabSections   = GetSections(tab);
        }

        private bool SetFocusSection(EditorSectionInfo section)
        {
            var     oldFocusSection     = m_focusSection;

            // Set new value
            m_focusSection  = section;

            // Update selection state
            if ((section != null) && (section == oldFocusSection))
            {
                section.Property.isExpanded     = !section.Property.isExpanded;
                m_focusSection                  = null;
            }
            else
            {
                if (oldFocusSection != null)
                {
                    oldFocusSection.Property.isExpanded = false;
                }
                if (section != null)
                {
                    section.Property.isExpanded         = true;
                }
            }
            bool    hasChanged  = (oldFocusSection != m_focusSection);
            if (hasChanged)
            {
                OnFocusSectionValueChange?.Invoke(m_focusSection);
            }
            return hasChanged;
        }

        #endregion
    }

    public class EditorSectionInfo
    {
        #region Properties

        public string DisplayName { get; private set; }

        public string Description { get; private set; }

        public bool IsEnabled => (EnabledProperty == null) || EnabledProperty.boolValue;

        public SerializedProperty Property { get; private set; }

        public SerializedProperty EnabledProperty { get; private set; }

        public EditorSectionDrawStyle DrawStyle { get; private set; }

        public DrawCallback DrawDetailsCallback { get; private set; }

        public string[] IgnoreProperties { get; private set; }

        public object Tag { get; private set; }

        #endregion

        #region Delegates

        public delegate void DrawCallback(EditorSectionInfo section);

        #endregion

        #region Constructors

        public EditorSectionInfo(string displayName,
                                 SerializedProperty property,
                                 SerializedProperty enabledProperty = null,
                                 string description = null,
                                 EditorSectionDrawStyle drawStyle = EditorSectionDrawStyle.Default,
                                 DrawCallback drawDetailsCallback = null,
                                 object tag = null,
                                 params string[] ignoreProperties)
        {   
            Assert.IsArgNotNull(displayName, nameof(displayName));
            Assert.IsArgNotNull(property, nameof(property));

            // set properties
            DisplayName             = displayName;
            Description             = description;
            Property                = property;
            EnabledProperty         = enabledProperty ?? property.FindPropertyRelative("m_isEnabled");
            DrawStyle               = drawStyle;
            DrawDetailsCallback     = drawDetailsCallback;
            IgnoreProperties        = ignoreProperties ?? new string[] { "m_isEnabled" };
            Tag                     = tag;
        }

        #endregion
    }

    public enum EditorSectionDrawStyle
    {
        Default,

        Expand,
    }
}