using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.CoreLibrary.Editor.NativePlugins.Build
{
    public class ObjectiveCBindingsWriter : INativeBindingsWriter
    {
        #region Static fields

        private     static  Dictionary<Type, string>    s_dataTypeMap           = new Dictionary<Type, string>()
        {
            { typeof(void), "void" },
            { typeof(bool), "bool" },
            { typeof(int), "int" },
            { typeof(long), "long" },
            { typeof(float), "float" },
            { typeof(double), "double" },
            { typeof(string), "const char*" },
            { typeof(IntPtr), "NPIntPtr" },
            { typeof(Delegate), "void*" },
        };
        
        private     static  Dictionary<Type, string>    s_defaultMethodImplMap  = new Dictionary<Type, string>()
        {
            { typeof(void), "" },
            { typeof(bool), "return false;" },
            { typeof(int), "return 0;" },
            { typeof(long), "return 0;" },
            { typeof(float), "return 0;" },
            { typeof(double), "return 0;" },
            { typeof(string), "return nil;" },
            { typeof(IntPtr), "return nil;" },
            { typeof(Delegate), "return nil;" },
        };

        #endregion

        #region Fields

        private     string                          m_headerFilePath;

        private     StreamWriter                    m_headerFileWriter;

        private     string                          m_implementationFilePath;

        private     StreamWriter                    m_implementationFileWriter;

        private     NativeBindingsGeneratorOptions  m_options;

        #endregion

        #region Constructors

        public ObjectiveCBindingsWriter(string path,
                                        string fileName,
                                        NativeBindingsGeneratorOptions options)
        {
            // Set properties
            m_options   = options;
            if (NeedsHeaderFile())
            {
                m_headerFilePath    = IOServices.CombinePath(path, $"{fileName}.h");
                m_headerFileWriter  = new StreamWriter(m_headerFilePath, append: false);
            }
            if (NeedsSourceFile())
            {
                m_implementationFilePath    = IOServices.CombinePath(path, $"{fileName}.mm");
                m_implementationFileWriter  = new StreamWriter(m_implementationFilePath, append: false);
            }
        }

        #endregion

        #region Static methods

        private static bool IsStructType(Type type) => type.IsValueType && !type.IsPrimitive && !type.IsEnum;

        #endregion

        #region Private methods

        private bool NeedsHeaderFile() => m_options.HasFlag(NativeBindingsGeneratorOptions.Header);

        private bool NeedsSourceFile() => m_options.HasFlag(NativeBindingsGeneratorOptions.Source);

        #endregion

        #region INativeBindingsGenerator implementation

        public void WriteStart(string product = null,
                               string author = null,
                               string copyrights = null)
        {
            // Header file
            string  headerName          = null;
            if (NeedsHeaderFile())
            {
                headerName      = IOServices.GetFileName(m_headerFilePath);
                WriteHeaderComments(m_headerFileWriter,
                                    headerName,
                                    product,
                                    author,
                                    copyrights);
                m_headerFileWriter.WriteLine();

                WriteHeaderImports(m_headerFileWriter,
                                   "#import <Foundation/Foundation.h>",
                                   "#import \"NPDefines.h\"",
                                   "#import \"NPUnityDataTypes.h\"");
                m_headerFileWriter.WriteLine();
            }

            // Implementation file
            if (NeedsSourceFile())
            {
                var     implementationName  = IOServices.GetFileName(m_implementationFilePath);
                WriteHeaderComments(m_implementationFileWriter,
                                    implementationName,
                                    product,
                                    author,
                                    copyrights);
                m_implementationFileWriter.WriteLine();

                if (headerName != null)
                {
                    WriteHeaderImports(m_implementationFileWriter,
                                       $"#import \"{headerName}\"");
                }
                WriteHeaderImports(m_implementationFileWriter,
                                   "#import \"NPDefines.h\"",
                                   "#import \"NPUnityDataTypes.h\"");
                m_implementationFileWriter.WriteLine();
            }
        }

        public void WriteCustomTypeDeclarations(Type[] customTypes)
        {
            var     writer      = m_headerFileWriter ?? m_implementationFileWriter;
            if (writer == null) return;

            for (int iter = 0; iter < customTypes.Length; iter++)
            {
                var     item    = customTypes[iter];
                if (iter != 0)
                {
                    writer.WriteLine();
                }

                if (IsStructType(item))
                {
                    WriteStructTypeDeclaration(writer, item);
                }
            }
            writer.WriteLine();
        }

        public void WriteMethod(MethodInfo method)
        {
            var     returnTypeName  = GetTypeDefinition(method.ReturnType);
            string  methodName      = method.Name;
            var     parameters      = Array.ConvertAll(method.GetParameters(),
                                                       (pItem) => GetParameterDefinition(pItem));
            var     methodBody      = GetDefaultMethodBodyDefinition(method.ReturnType);
            var     methodSignature = $"{returnTypeName} {methodName}({string.Join(", ", parameters)})";

            // Header section
            if (NeedsHeaderFile())
            {
                m_headerFileWriter.WriteLine($"{methodSignature};");
            }

            // Implementation section
            if (NeedsSourceFile())
            {
                m_implementationFileWriter.WriteLine($"NPBINDING DONTSTRIP {methodSignature}\n{methodBody}");
                m_implementationFileWriter.WriteLine();
            }
        }

        public void WriteEnd(out string[] files)
        {
            var     fileList    = new List<string>();
            if (NeedsHeaderFile())
            {
                fileList.Add(m_headerFilePath);
                m_headerFileWriter.Close();
            }
            if (NeedsSourceFile())
            {
                fileList.Add(m_implementationFilePath);
                m_implementationFileWriter.Close();
            }

            // Set reference values
            files   = fileList.ToArray();
        }

        #endregion

        #region Write methods

        private void WriteHeaderComments(StreamWriter writer,
                                         string fileName,
                                         string product,
                                         string author,
                                         string copyrights)
        {
            writer.WriteLine("//");
            writer.WriteLine($"// {fileName}");
            writer.WriteLine($"// {product}");
            writer.WriteLine("//");
            if (author != null)
            {
                writer.WriteLine($"// Created by {author} on {DateTime.Now.ToString("d")}.");
            }
            if (copyrights != null)
            {
                writer.WriteLine($"// {copyrights}");
            }
            writer.WriteLine("//");
        }

        private void WriteHeaderImports(StreamWriter writer,
                                        params string[] files)
        {
            foreach (var item in files)
            {
                writer.WriteLine($"{item}");
            }
        }

        private void WriteStructTypeDeclaration(StreamWriter writer,
                                                Type structType)
        {
            writer.WriteLine($"struct {structType.Name}");
            writer.WriteLine("{");
            writer.WriteLine("};");
            writer.WriteLine($"typedef struct {structType.Name} {structType.Name};");
        }

        #endregion

        #region Private methods

        private string GetTypeDefinition(Type type)
        {
            // Resolve the type to the suitable representation
            if (typeof(Delegate).IsAssignableFrom(type))
            {
                type    = typeof(Delegate);
            }

            // Match type to its representation in objective c
            if (type.IsArray || type.IsByRef)
            {
                return GetTypeDefinition(type.GetElementType()) + "*";
            }
            if (type.IsEnum)
            {
                return "int";
            }

            if (s_dataTypeMap.TryGetValue(type, out string typeName))
            {
                return typeName;
            }
            if (TryGetBuiltinTypeName(type, out string nativeTypeName))
            {
                return nativeTypeName;
            }
            return type.Name;
        }

        private string GetParameterDefinition(ParameterInfo param)
        {
            var     paramType   = GetTypeDefinition(param.ParameterType);
            var     paramName   = param.Name;
            return $"{paramType} {paramName}";
        }

        public string GetDefaultMethodBodyDefinition(Type returnType)
        {
            string methodBody   = null;
            if (returnType.IsEnum)
            {
                returnType      = typeof(int);
            }

            if (!s_defaultMethodImplMap.TryGetValue(returnType, out methodBody))
            {
                if (TryGetBuiltinTypeName(returnType, out string nativeType))
                {
                    if (returnType.IsValueType)
                    {
                        methodBody  = $"return {nativeType}();";
                    }
                }
            }
            return string.IsNullOrEmpty(methodBody)
                ? "{ }"
                : $"{{\n\t{methodBody}\n}}";
        }

        private bool TryGetBuiltinTypeName(Type type, out string nativeType)
        {
            nativeType  = null;

            if (type.FullName.StartsWith("VoxelBusters"))
            {
                if (IsStructType(type))
                {
                    string  typeName    = type.Name;
                    if (typeName.StartsWith("Unity"))
                    {
                        nativeType      = $"NP{typeName}";
                        return true;
                    }
                    if (typeName.StartsWith("Native"))
                    {
                        nativeType      = typeName.Replace("Native", "NP");
                        return true;
                    }
                }
            }
            return false;
        }

        #endregion
    }
}