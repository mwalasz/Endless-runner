using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.CoreLibrary.Editor
{
    public static class AssemblyDefinitionServices
    {
        #region Static methods

        public static void CreateDefinition(string path,
                                            string name,
                                            string[] includePlatforms = null,
                                            string[] references = null)
        {
            string      json    = $"{{" +
                $"\n\t\"name\":\"{name}\"," +
                $"\n\t\"includePlatforms\":{Jsonify(includePlatforms)}," +
                $"\n\t\"references\":{Jsonify(references)}" +
                $"\n}}";
            IOServices.CreateFile(IOServices.CombinePath(path, $"{name}.asmdef"), json);
        }

        private static string Jsonify(string[] array)
        {
            if (array == null) return "[]";

            return $"[{string.Join(",", System.Array.ConvertAll(array, (item) => $"\"{item}\""))}]";
        }

        #endregion
    }
}