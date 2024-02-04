using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.Editor;
using VoxelBusters.EssentialKit.AddressBookCore.Simulator;

namespace VoxelBusters.EssentialKit.Editor
{
    [CustomEditor(typeof(EssentialKitSettings))]
    public class EssentialKitSettingsInspector : SettingsObjectInspector
    {
        #region Fields

        private     SerializedProperty          m_appSettingsProperty;

        private     EditorSectionInfo[]         m_serviceSections;

        private     ButtonMeta[]                m_resourceButtons;

        #endregion

        #region Base class methods

        protected override void OnEnable()
        {
            // Set properties
            m_appSettingsProperty   = serializedObject.FindProperty("m_applicationSettings");
            m_serviceSections       = new EditorSectionInfo[]
            {
                new EditorSectionInfo(displayName: "Address Book",
                                      description: "Access the user's contacts, and format and localize contact information.",
                                      property: serializedObject.FindProperty("m_addressBookSettings"),
                                      drawDetailsCallback: DrawAddressBookSettingsDetails),
                new EditorSectionInfo(displayName: "Native UI",
                                      description: "Support display of native dialogs within your apps.",
                                      property: serializedObject.FindProperty("m_nativeUISettings"),
                                      drawDetailsCallback: DrawNativeUISettingsDetails),
                new EditorSectionInfo(displayName: "Network Services",
                                      description: "Detect the user's device network capabilities.",
                                      property: serializedObject.FindProperty("m_networkServicesSettings"),
                                      drawDetailsCallback: DrawNetworkServicesSettingsDetails),
                new EditorSectionInfo(displayName: "Sharing",
                                      description: "Support sharing capabilities on social media platforms.",
                                      property: serializedObject.FindProperty("m_sharingServicesSettings"),
                                      drawDetailsCallback: DrawSharingServicesSettingsDetails),
                new EditorSectionInfo(displayName: "Utilities",
                                      description: "Other useful features that are most commonly used on mobile games.",
                                      property: serializedObject.FindProperty("m_utilitySettings"),
                                      drawDetailsCallback: DrawUtilitySettingsDetails),
            };
            m_resourceButtons   = new ButtonMeta[]
            {
                new ButtonMeta(label: "Documentation",  onClick: EssentialKitEditorUtility.OpenDocumentation),
                new ButtonMeta(label: "Tutorials",      onClick: EssentialKitEditorUtility.OpenTutorials),
                new ButtonMeta(label: "Forum",          onClick: EssentialKitEditorUtility.OpenForum),
                new ButtonMeta(label: "Discord",        onClick: EssentialKitEditorUtility.OpenSupport),
                new ButtonMeta(label: "Write Review",	onClick: () => EssentialKitEditorUtility.OpenAssetStorePage(false)),
            };

            base.OnEnable();
        }

        protected override UnityPackageDefinition GetOwner()
        {
            return EssentialKitSettings.Package;
        }

        protected override string[] GetTabNames()
        {
            return new string[]
            {
                DefaultTabs.kGeneral,
                DefaultTabs.kServices,
                DefaultTabs.kMisc,
            };
        }

        protected override EditorSectionInfo[] GetSectionsForTab(string tab)
        {
            if (tab == DefaultTabs.kServices)
            {
                return m_serviceSections;
            }
            return null;
        }

        protected override bool DrawTabView(string tab)
        {
            switch (tab)
            {
                case DefaultTabs.kGeneral:
                    DrawGeneralTabView();
                    return true;

                case DefaultTabs.kMisc:
                    DrawMiscTabView();
                    return true;

                default:
                    return false;
            }
        }

        protected override void DrawFooter(string tab)
        {
            base.DrawFooter(tab);

            if (tab == DefaultTabs.kGeneral)
            {
                // provide option to add resources
                /*EditorLayoutUtility.Helpbox(title: "Essentials",
                                            description: "Add resources to your project that are essential for using Essential Kit plugin.",
                                            actionLabel: "Import Essentials",
                                            onClick: EssentialKitEditorUtility.ImportEssentialResources,
                                            style: GroupBackgroundStyle);*/

                //ShowMigrateToUpmOption();
            }
        }

        #endregion

        #region Section methods

        private void DrawGeneralTabView()
        {
            var     storeIdsSection     = new EditorSectionInfo(displayName: "Store Id's",
                                                                property: m_appSettingsProperty.FindPropertyRelative("m_appStoreIds"),
                                                                drawStyle: EditorSectionDrawStyle.Expand);
            var     permissionSection   = new EditorSectionInfo(displayName: "Usage Permissions",
                                                                property: m_appSettingsProperty.FindPropertyRelative("m_usagePermissionSettings"),
                                                                drawStyle: EditorSectionDrawStyle.Expand);
            var     rateMyAppSection    = new EditorSectionInfo(displayName: "Rate My App",
                                                                property: m_appSettingsProperty.FindPropertyRelative("m_rateMyAppSettings"),
                                                                drawStyle: EditorSectionDrawStyle.Expand);
            EditorGUILayout.BeginVertical(GroupBackgroundStyle);
            EditorGUILayout.PropertyField(m_appSettingsProperty.FindPropertyRelative("m_logLevel"));
            EditorGUILayout.EndVertical();
                                                  
            LayoutBuilder.DrawSection(storeIdsSection,
                                      showDetails: true,
                                      selectable: false);
            LayoutBuilder.DrawSection(permissionSection,
                                      showDetails: true,
                                      selectable: false);
            LayoutBuilder.DrawSection(rateMyAppSection,
                                      showDetails: true,
                                      selectable: false);
        }

        private void DrawMiscTabView()
        {
            DrawButtonList(m_resourceButtons);
        }

        private void DrawAddressBookSettingsDetails(EditorSectionInfo section)
        {
            EditorLayoutBuilder.DrawChildProperties(property: section.Property,
                                                    ignoreProperties: section.IgnoreProperties);

            GUILayout.BeginVertical();
            if (GUILayout.Button("Resource Page"))
            {
                ProductResources.OpenResourcePage(NativeFeatureType.kAddressBook);
            }
            if (GUILayout.Button("Reset Simulator"))
            {
                AddressBookSimulator.Reset();
            }
            GUILayout.EndVertical();
        }

        

        private void DrawNetworkServicesSettingsDetails(EditorSectionInfo section)
        {
           EditorLayoutBuilder.DrawChildProperties(property: section.Property,
                                                    ignoreProperties: section.IgnoreProperties);

            GUILayout.BeginVertical();
            if (GUILayout.Button("Resource Page"))
            {
                ProductResources.OpenResourcePage(NativeFeatureType.kNetworkServices);
            }
            GUILayout.EndVertical();
        }

       

        private void DrawNativeUISettingsDetails(EditorSectionInfo section)
        {
            EditorLayoutBuilder.DrawChildProperties(property: section.Property,
                                                    ignoreProperties: section.IgnoreProperties);

            GUILayout.BeginVertical();
            if (GUILayout.Button("Resource Page"))
            {
                ProductResources.OpenResourcePage(NativeFeatureType.kNativeUI);
            }
            GUILayout.EndVertical();
        }

        

        private void DrawSharingServicesSettingsDetails(EditorSectionInfo section)
        {
            EditorLayoutBuilder.DrawChildProperties(property: section.Property,
                                                    ignoreProperties: section.IgnoreProperties);

            GUILayout.BeginVertical();
            if (GUILayout.Button("Resource Page"))
            {
                ProductResources.OpenResourcePage(NativeFeatureType.kSharingServices);
            }

            GUILayout.EndVertical();
        }


        private void DrawUtilitySettingsDetails(EditorSectionInfo section)
        {
            EditorLayoutBuilder.DrawChildProperties(property: section.Property,
                                                    ignoreProperties: section.IgnoreProperties);

            GUILayout.BeginVertical();
            if (GUILayout.Button("Resource Page"))
            {
                ProductResources.OpenResourcePage(NativeFeatureType.kExtras);
            }
            GUILayout.EndVertical();
        }

        #endregion
    }
}