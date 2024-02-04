using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace VoxelBusters.CoreLibrary
{
    public static class UnityPackagePathResolver
    {
        #region Static methods

        public static bool IsInstalledWithinAssets(this UnityPackageDefinition package)
        {
            return IOServices.DirectoryExists(package.DefaultInstallPath) &&
                IOServices.FileExists($"{package.DefaultInstallPath}/package.json");
        }

        public static string GetInstallPath(this UnityPackageDefinition package)
        {
            if (IsSupported())
            {
                return  IsInstalledWithinAssets(package) ? package.DefaultInstallPath : package.UpmInstallPath;
            }
            return null;
        }

        public static string GetRuntimeScriptsPath(this UnityPackageDefinition package)
        {
            return CombinePath(pathA: GetInstallPath(package), pathB: "Runtime");
        }

        public static string GetEditorScriptsPath(this UnityPackageDefinition package)
        {
            return CombinePath(pathA: GetInstallPath(package), pathB: "Editor");
        }

        public static string GetEditorResourcesPath(this UnityPackageDefinition package)
        {
            return CombinePath(pathA: GetInstallPath(package), pathB: "EditorResources");
        }

        public static string GetMutableResourcesPath(this UnityPackageDefinition package)
        {
            return package.MutableResourcesPath;
        }

        public static string GetImmutableResourcesPath(this UnityPackageDefinition package)
        {
            return CombinePath(pathA: GetInstallPath(package), pathB: "Resources");
        }

        public static string GetPackageResourcesPath(this UnityPackageDefinition package)
        {
            return CombinePath(pathA: GetInstallPath(package), pathB: "PackageResources");
        }

        public static string GetFullPath(this UnityPackageDefinition package, string relativePath)
        {
            return CombinePath(pathA: GetInstallPath(package), pathB: relativePath);
        }

        public static string GetMutableResourceRelativePath(this UnityPackageDefinition package, string name)
        {
            return CombinePath(pathA: package.MutableResourcesRelativePath, pathB: name);
        }

        public static string GetExtrasPath(this UnityPackageDefinition package)
        {
            return CombinePath(pathA: package.DefaultInstallPath, pathB: "Extras");
        }

        public static string GetEssentialsPath(this UnityPackageDefinition package)
        {
            return CombinePath(pathA: package.DefaultInstallPath, pathB: "Essentials");
        }

        public static string GetGeneratedPath(this UnityPackageDefinition package)
        {
            return CombinePath(pathA: package.DefaultInstallPath, pathB: "Generated");
        }

        private static bool IsSupported() => Application.isEditor;

        private static string CombinePath(string pathA, string pathB)
        {
            if (pathA == null)
            {
                return null;
            }
            else if (pathA == "")
            {
                return pathB;
            }
            else
            {
                return $"{pathA}/{pathB}";
            }
        }

        #endregion
    }
}