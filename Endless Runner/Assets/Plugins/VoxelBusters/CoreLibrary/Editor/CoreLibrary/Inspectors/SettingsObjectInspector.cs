using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace VoxelBusters.CoreLibrary.Editor
{
    public abstract class SettingsObjectInspector : UnityEditor.Editor
    {
        #region Constants

        private     static  readonly    ButtonMeta[]            s_emptyButtonArray          = new ButtonMeta[0];

        private     static  readonly    string[]                s_ignoredProperties         = new string[] { "m_Script" };

        #endregion

        #region Fields

        private     string                  m_productName;

        private     string                  m_productVersion;

        // Assets
        private     Texture2D               m_logoIcon;

        #endregion

        #region Properties

        public EditorLayoutBuilder LayoutBuilder { get; private set; }

        protected GUIStyle CustomMarginStyle { get; private set; }

        protected GUIStyle GroupBackgroundStyle { get; private set; }

        protected GUIStyle ProductNameStyle { get; private set; }

        protected GUIStyle NormalLabelStyle { get; private set; }

        protected GUIStyle OptionsLabelStyle { get; private set; }

        #endregion

        #region Abstract methods

        protected abstract UnityPackageDefinition GetOwner();

        protected abstract string[] GetTabNames();

        protected abstract EditorSectionInfo[] GetSectionsForTab(string tab);

        #endregion

        #region Unity methods

        protected virtual void OnEnable()
        { }

        public override void OnInspectorGUI()
        {
            EnsurePropertiesAreSet();

            EditorGUILayout.BeginVertical(CustomMarginStyle);
            LayoutBuilder.DoLayout();
            EditorGUILayout.EndVertical();
        }

        public override bool UseDefaultMargins()
        {
            return false;
        }

        #endregion

        #region Draw methods

        protected virtual void DrawTopBar(string tab)
        {
            GUILayout.BeginHorizontal(GroupBackgroundStyle);

            // logo section
            GUILayout.BeginVertical();
            GUILayout.Space(2f);
            GUILayout.Label(m_logoIcon, GUILayout.Height(64f), GUILayout.Width(64f));
            GUILayout.Space(2f);
            GUILayout.EndVertical();

            // product info
            GUILayout.BeginVertical();
            GUILayout.Label(m_productName, ProductNameStyle);
            GUILayout.Label(m_productVersion, NormalLabelStyle);
            GUILayout.Label("Copyright © 2023 Voxel Busters Interactive LLP.", OptionsLabelStyle);
            GUILayout.EndVertical();

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        protected virtual bool DrawTabView(string tab)
        {
            return false;
        }

        protected virtual void DrawFooter(string tab)
        { }

        protected virtual void DrawButtonList(ButtonMeta[] buttons)
        {
            GUILayout.BeginVertical();
            foreach (var item in buttons)
            {
                if (GUILayout.Button(item.Label, GUILayout.MinHeight(80f)))
                {
                    item?.OnClick();
                }
            }
            GUILayout.EndVertical();
        }

        #endregion

        #region Private methods

        private void EnsurePropertiesAreSet()
        {
            if (LayoutBuilder != null) return;

            LoadCustomStyles();
            LoadAssets();

            // Set properties
            var     commonResourcePath  = CoreLibrarySettings.Package.GetEditorResourcesPath();
            var     ownerPackage        = GetOwner();
            m_productName               = ownerPackage.DisplayName;
            m_productVersion            = $"v{ownerPackage.Version}";
            LayoutBuilder             = new EditorLayoutBuilder(serializedObject: serializedObject,
                                                                  tabs: GetTabNames(),
                                                                  getSectionsCallback: GetSectionsForTab,
                                                                  drawTopBarCallback: DrawTopBar,
                                                                  drawTabViewCallback: DrawTabView,
                                                                  drawFooterCallback: DrawFooter,
                                                                  toggleOnIcon: AssetDatabase.LoadAssetAtPath<Texture2D>(commonResourcePath + "/Textures/toggle-on.png"),
                                                                  toggleOffIcon: AssetDatabase.LoadAssetAtPath<Texture2D>(commonResourcePath + "/Textures/toggle-off.png"));
            LayoutBuilder.OnSectionStatusChange       += OnSectionStatusChange;
            LayoutBuilder.OnFocusSectionValueChange   += OnFocusSectionValueChange;
        }

        private void LoadCustomStyles()
        {
            CustomMarginStyle   = new GUIStyle(EditorStyles.inspectorFullWidthMargins)
            {
                margin          = new RectOffset(2, 2, 0, 0),
            };
            GroupBackgroundStyle     = CustomEditorStyles.GroupBackground();
            ProductNameStyle    = CustomEditorStyles.Heading1Label();
            NormalLabelStyle    = CustomEditorStyles.NormalLabel();
            OptionsLabelStyle   = CustomEditorStyles.OptionsLabel();
        }

        private void LoadAssets()
        {
            // load custom assets
            var     ownerResourcePath   = GetOwner().GetEditorResourcesPath();
            m_logoIcon                  = AssetDatabase.LoadAssetAtPath<Texture2D>(ownerResourcePath + "/Textures/logo.png");
        }

        protected void EnsureChangesAreSerialized()
        {
            EditorUtility.SetDirty(target);
            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();
        }

        protected void TryApplyModifiedProperties()
        {
            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
                serializedObject.Update();
            }
        }

        #endregion

        #region Misc methods
#if NATIVE_PLUGINS_SHOW_UPM_MIGRATION
        protected void ShowMigrateToUpmOption()
        {
            EditorLayoutUtility.Helpbox(title: "UPM Support",
                                        description: "You can install the package on UPM.",
                                        actionLabel: "Migrate To UPM",
                                        onClick: GetOwner().MigrateToUpm,
                                        style: GroupBackgroundStyle);
        }
#endif
        #endregion

        #region Event handler methods

        protected virtual void OnSectionStatusChange(EditorSectionInfo section)
        { }

        protected virtual void OnFocusSectionValueChange(EditorSectionInfo section)
        { }

        #endregion

        #region Nested types

        protected class ButtonMeta
        {
            #region Properties

            public string Label { get; private set; }

            public System.Action OnClick { get; private set; }

            #endregion

            #region Constructors

            public ButtonMeta(string label, System.Action onClick)
            {
                // set properties
                Label       = label;
                OnClick     = onClick;
            }

            #endregion
        }

        protected class DefaultTabs
        {
            public  const   string  kGeneral    = "General";

            public  const   string  kServices   = "Services";

            public  const   string  kMisc       = "Help";
        }

        #endregion
    }
}