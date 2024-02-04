using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.CoreLibrary.Editor.NativePlugins.Build
{
    [Serializable, CreateAssetMenu(fileName = "PBXNativePluginsExporterObject", menuName = "Native Plugins/PBX Exporter Object", order = 0)]
    public class PBXNativePluginsExporterObject : NativePluginsExporterObject
    {
        #region Fields

		[SerializeField]
        private     string[]                    m_compileFlags;
        
        [SerializeField]
        private     List<PBXFile>               m_headerPaths       = new List<PBXFile>();
		
        [SerializeField]
		private	    List<PBXFramework>		    m_frameworks		= new List<PBXFramework>();
		
        [SerializeField]
		private	    List<PBXCapability>         m_capabilities		= new List<PBXCapability>();
        
        [SerializeField]
        private     List<StringKeyValuePair>    m_macros            = new List<StringKeyValuePair>();
        
        [SerializeField]
        private     List<StringKeyValuePair>    m_buildProperties   = new List<StringKeyValuePair>();

        #endregion

        #region Properties

        public string[] CompileFlags => m_compileFlags;

        public PBXFile[] HeaderPaths => m_headerPaths.ToArray();

        public PBXFramework[] Frameworks => m_frameworks.ToArray();

        public PBXCapability[] Capabilities => m_capabilities.ToArray();

        public StringKeyValuePair[] Macros => m_macros.ToArray();

        public StringKeyValuePair[] BuildProperties => m_buildProperties.ToArray();

        #endregion

        #region Public methods

        public void SetCompilerFlags(params string[] flags)
        {
            m_compileFlags = flags;
        }

        public void AddCapability(PBXCapability value)
        {
            m_capabilities.Add(value);
        }

        public void RemoveCapability(PBXCapability value)
        {
            m_capabilities.Remove(value);
        }

        public void ClearCapabilities()
        {
            m_capabilities.Clear();
        }

        public void AddMacro(string name, string value)
        {
            var     keyValuePair    = new StringKeyValuePair(name, value);
            m_macros.AddOrReplace(
                keyValuePair,
                (item) => string.Equals(item.Key, name));
        }

        public void RemoveMacro(string name)
        {
            m_macros.Remove((item) => string.Equals(item.Key, name));
        }

        public void ClearMacros()
        {
            m_macros.Clear();
        }

        public void AddFramework(PBXFramework value)
        {
            m_frameworks.Add(value);
        }

        public void RemoveFramework(PBXFramework value)
        {
            m_frameworks.Remove(value);
        }

        public void ClearFrameworks()
        {
            m_frameworks.Clear();
        }

        public void AddBuildProperty(string name, string value)
        {
            var     keyValuePair    = new StringKeyValuePair(name, value);
            m_buildProperties.AddOrReplace(
                keyValuePair,
                (item) => string.Equals(item.Key, name));
        }

        public void RemoveBuildProperty(string name)
        {
            m_buildProperties.Remove((item) => string.Equals(item.Key, name));
        }

        public void ClearBuildProperties()
        {
            m_buildProperties.Clear();
        }

        #endregion
    }
}