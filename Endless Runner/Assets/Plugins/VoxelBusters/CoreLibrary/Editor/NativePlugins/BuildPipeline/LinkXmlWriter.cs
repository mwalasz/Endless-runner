using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.CoreLibrary.Editor.NativePlugins.Build
{
    public partial class LinkXmlWriter
    {
        #region Fields

        private     List<AssemblyDefinition>    m_assemblies;
        
        private     string                      m_savePath;

        #endregion

        #region Constructors

        public LinkXmlWriter(string path)
        {
            // set properties
            m_savePath      = path;
            m_assemblies    = new List<AssemblyDefinition>();
        }

        #endregion

        #region Private static methods

        private static void WriteNamespace(XmlWriter writer, string name, bool preserve)
        {
            writer.WriteStartElement("namespace");
            writer.WriteAttributeString("fullname", name);
            writer.WriteAttributeString("preserve", preserve ? "all" : "nothing");
            writer.WriteEndElement();
        }

        private static void WriteType(XmlWriter writer, string name, bool preserve)
        {
            writer.WriteStartElement("type");
            writer.WriteAttributeString("fullname", name);
            writer.WriteAttributeString("preserve", preserve ? "all" : "nothing");
            writer.WriteEndElement();
        }

        #endregion

        #region Public methods

        private AssemblyDefinition GetOrCreateAssemblyDefinition(string assembly)
        {
            var     targetDefinition    = m_assemblies.Find((item) => string.Equals(assembly, item.AssemblyName));
            if (targetDefinition == null)
            {
                var     newDefinition   = new AssemblyDefinition(assembly);
                m_assemblies.Add(newDefinition);

                return newDefinition;
            }
            return targetDefinition;
        }

        public void AddConfiguration(string name, NativeFeatureRuntimeConfiguration runtimeConfiguration, RuntimePlatform platform, bool useFallbackPackage/* = false*/)
        {
            var     packageConfiguration    = runtimeConfiguration.GetPackageForPlatform(platform);
            if (packageConfiguration == null || useFallbackPackage)
            {
                DebugLogger.LogWarning(CoreLibraryDomain.Default, $"Using fallback configuration for : {name}.");
                var     fallbackConfiguration   = runtimeConfiguration.FallbackPackage;
                AddRequiredType(fallbackConfiguration.Assembly, fallbackConfiguration.NativeInterfaceType);
            }
            else
            {
                AddRequiredNamespace(packageConfiguration.Assembly, packageConfiguration.Namespace);
            }
        }

        public void AddRequiredNamespace(string assembly, string ns)
        {
            GetOrCreateAssemblyDefinition(assembly)
                .AddRequiredNamespace(ns);
        }

        public void AddIgnoreNamespace(string assembly, string ns)
        {
            GetOrCreateAssemblyDefinition(assembly)
                .AddIgnoreNamespace(ns);
        }

        public void AddRequiredType(string assembly, string type)
        {
            GetOrCreateAssemblyDefinition(assembly)
                .AddRequiredType(type);
        }

        public void AddIgnoreType(string assembly, string type)
        {
            GetOrCreateAssemblyDefinition(assembly)
                .AddIgnoreType(type);
        }

        public void WriteToFile()
        {
            // remove existing file
            if (IOServices.FileExists(m_savePath))
            {
                IOServices.DeleteFile(m_savePath);
            }

            // create new
            var     settings                = new XmlWriterSettings();
            settings.Encoding               = new System.Text.UTF8Encoding(true);
            settings.ConformanceLevel       = ConformanceLevel.Document;
            settings.OmitXmlDeclaration     = true;
            settings.Indent                 = true;
            using (var writer = XmlWriter.Create(m_savePath, settings))
            {
                writer.WriteStartDocument();

                writer.WriteStartElement("linker");
                foreach (var item in m_assemblies)
                {
                    // write ignored namespace
                    writer.WriteStartElement("assembly");
                    writer.WriteAttributeString("fullname", item.AssemblyName);
                    foreach (string namespaceValue in item.RequiredNamespaces)
                    {
                        WriteNamespace(writer, namespaceValue, true);
                    }
                    foreach (string namespaceValue in item.IgnoreNamespaces)
                    {
                        WriteNamespace(writer, namespaceValue, false);
                    }
                    foreach (string type in item.RequiredTypes)
                    {
                        WriteType(writer, type, true);
                    }
                    foreach (string type in item.IgnoredTypes)
                    {
                        WriteType(writer, type, false);
                    }
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();

                writer.WriteEndDocument();
            }
        }

        #endregion
    }
}