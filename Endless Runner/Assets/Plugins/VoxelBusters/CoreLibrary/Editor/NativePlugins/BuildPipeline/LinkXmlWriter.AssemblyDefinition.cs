using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.CoreLibrary.Editor.NativePlugins.Build
{
    public partial class LinkXmlWriter
    {
        private class AssemblyDefinition
        {
            #region Fields

            private     List<string>        m_requiredNamespaces;
            
            private     List<string>        m_ignoreNamespaces;
            
            private     List<string>        m_requiredTypes;
            
            private     List<string>        m_ignoreTypes;

            #endregion

            #region Properties

            public string AssemblyName { get; private set; }

            public string[] RequiredNamespaces { get { return m_requiredNamespaces.ToArray(); } }

            public string[] IgnoreNamespaces { get { return m_ignoreNamespaces.ToArray(); } }

            public string[] RequiredTypes { get { return m_requiredTypes.ToArray(); } }

            public string[] IgnoredTypes { get { return m_ignoreTypes.ToArray(); } }

            #endregion

            #region Constructors

            public AssemblyDefinition(string assembly)
            {
                // set default properties
                AssemblyName            = assembly;
                m_requiredNamespaces    = new List<string>();
                m_ignoreNamespaces      = new List<string>();
                m_requiredTypes         = new List<string>();
                m_ignoreTypes           = new List<string>();
            }

            #endregion

            #region Public methods

            public void AddRequiredNamespace(string name)
            {
                AddValueIfNotFound(m_requiredNamespaces, name);
            }

            public void AddIgnoreNamespace(string name)
            {
                AddValueIfNotFound(m_ignoreNamespaces, name);
            }

            public void AddRequiredType(string name)
            {
                AddValueIfNotFound(m_requiredTypes, name);
            }

            public void AddIgnoreType(string name)
            {
                AddValueIfNotFound(m_ignoreTypes, name);
            }

            #endregion

            #region Private methods

            private void AddValueIfNotFound(List<string> list, string value)
            {
                if (!list.Contains(value))
                {
                    list.Add(value);
                }
            }

            #endregion
        }
    }
}