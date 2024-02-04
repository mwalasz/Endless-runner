using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace VoxelBusters.CoreLibrary.Editor.NativePlugins.Build
{
    public enum NativeBindingsGeneratorOptions
    {
        Header = 1 << 0,

        Source = 1 << 1,
    }

    public class NativeBindingsGenerator
    {
        #region Properties

        private     string      m_output;
        
        private     NativeBindingsGeneratorOptions  m_options;

        private     string      m_product;
        
        private     string      m_author;
        
        private     string      m_copyrights;

        #endregion

        #region Constructors

        public NativeBindingsGenerator(string outputPath,
                                       NativeBindingsGeneratorOptions options)
        {
            // Set properties
            m_output        = IOServices.GetAbsolutePath(outputPath);
            m_options       = options;
        }

        #endregion

        #region Static methods

        public static INativeBindingsWriter CreateBindingsWriter(string path,
                                                                 string fileName,
                                                                 NativeBindingsGeneratorOptions options,
                                                                 BuildTarget buildTarget)
        {
            switch (buildTarget)
            {
                case BuildTarget.iOS:
                    return new ObjectiveCBindingsWriter(path, fileName, options);

                default:
                    throw new NotSupportedException();
            }
        }

        #endregion

        #region Public methods

        public NativeBindingsGenerator SetProduct(string value)
        {
            // Set value
            m_product   = value;

            return this;
        }

        public NativeBindingsGenerator SetAuthor(string value)
        {
            // Set value
            m_author     = value;

            return this;
        }

        public NativeBindingsGenerator SetCopyrights(string value)
        {
            // Set value
            m_copyrights = value;

            return this;
        }

        public void Generate(Type cScriptType,
                             string fileName,
                             BuildTarget buildTarget,
                             Type[] customTypes,
                             out string[] files)
        {
            // Create output folder
            IOServices.CreateDirectory(m_output, false);

            // Create native files
            var     bindingsWriter  = CreateBindingsWriter(m_output,
                                                           fileName,
                                                           m_options,
                                                           buildTarget);
            try
            {
                var     externMethods   = Array.FindAll(cScriptType.GetMethods(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public),
                                                        (method) => (method.GetMethodBody() == null) && method.IsStatic);
                bindingsWriter.WriteStart(m_product,
                                          m_author,
                                          m_copyrights);
                if (!customTypes.IsNullOrEmpty())
                {
                    bindingsWriter.WriteCustomTypeDeclarations(customTypes);
                }
                foreach (var method in externMethods)
                {
                    bindingsWriter.WriteMethod(method);
                }
            }
            finally
            {
                bindingsWriter.WriteEnd(out files);
            }
        }

        #endregion
    }
}