#if UNITY_IOS || UNITY_TVOS
using System;
using System.Collections;
using System.Collections.Generic;

namespace VoxelBusters.CoreLibrary.Editor.NativePlugins.Build.Xcode
{
    public class MacroDefinitionsManager
    {
        #region Fields

        private     string          m_savePath;

        private     string          m_headerComments;
        
        private     List<string>    m_requiredMacros;

        #endregion

        #region Constructors

        public MacroDefinitionsManager(string path, string headerComments)
        {
            // Set properties
            m_savePath          = path;
            m_headerComments    = headerComments;
            m_requiredMacros    = new List<string>();
        }

        #endregion

        #region Public methods

        public void AddMacro(params string[] values)
        {
            foreach (var value in values)
            {
                m_requiredMacros.AddUnique(value);
            }
        }

        public void WriteToFile()
        {
            // Build content
            var     stringBuilder   = new System.Text.StringBuilder(256);
            if (m_headerComments != null)
            {
                stringBuilder.AppendLine(m_headerComments)
                    .AppendLine();
            }
            stringBuilder.AppendLine("#pragma once")
                .AppendLine();
            foreach (var definition in m_requiredMacros)
            {
                stringBuilder.AppendLine($"#define {definition}");
            }

            // Serialize changes
            IOServices.CreateFile(m_savePath, stringBuilder.ToString());
        }

        #endregion
    }
}
#endif