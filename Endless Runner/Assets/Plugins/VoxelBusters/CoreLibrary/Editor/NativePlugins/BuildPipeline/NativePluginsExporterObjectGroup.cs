using System;
using UnityEngine;
using UnityEditor;

namespace VoxelBusters.CoreLibrary.Editor.NativePlugins.Build
{
    [CreateAssetMenu(fileName = "NativePluginsExporterObjectGroup", menuName = "Native Plugins/Exporter Object Group", order = 0)]
    public class NativePluginsExporterObjectGroup : ScriptableObject
    {
        #region Fields

        [SerializeField]
        private     string      m_name;
        
        [SerializeField]
        private     bool        m_usesNestedHeierarchy      = true;

        #endregion

        #region Properties

        public string Name => m_name;

        public bool UsesNestedHeierarchy => m_usesNestedHeierarchy;

        #endregion
    }
}