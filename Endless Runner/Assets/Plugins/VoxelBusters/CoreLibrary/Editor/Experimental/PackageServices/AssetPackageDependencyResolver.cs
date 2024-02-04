using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Compilation;
using System.IO;
using System;

using JsonUtility = VoxelBusters.CoreLibrary.Parser.JsonUtility;

namespace VoxelBusters.CoreLibrary.Editor.Experimental
{
    public class UnityPackageManifestKey
    {
        public const string kDependencies = "dependencies";
    }

    public class AssetPackageDependencyResolver : AssetPostprocessor
    {
       /* #region Constants

        private const string kMainManifestPath  = "Packages/manifest.json";

        #endregion

        #region Fields

        private static Dictionary<string, object> s_mainManifestDict;

        #endregion

        #region Static methods

        [MenuItem("Window/Voxel Busters/Asset Packages/Resolve Dependency")]
        public static void Resolve()
        {
            var     assetManifestFiles  = Array.FindAll(
                Array.ConvertAll(AssetDatabase.FindAssets("package"), (guid) => AssetDatabase.GUIDToAssetPath(guid)),
                (assetPath) => IsPackageManifest(assetPath));
            if (assetManifestFiles.Length > 0)
            {
                AddMissingDependencies(assetManifestFiles);
            }
        }

        #endregion

        #region Private static methods

        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            var     assetManifestFiles      = Array.FindAll(
                importedAssets,
                (assetPath) => IsPackageManifest(assetPath));
            AddMissingDependencies(assetManifestFiles);
        }

        private static bool IsPackageManifest(string assetPath)
        {
            return assetPath.StartsWith("Assets") && string.Equals(Path.GetFileName(assetPath), "package.json");
        }

        private static void AddMissingDependencies(string[] assetManifestFiles)
        { 
            // Gather all the dependencies from the packages installed in Assets
            // Manually add them to project manifest file
            var     registeredDependencies  = GetOrCreateMainManifestObject()[UnityPackageManifestKey.kDependencies] as Dictionary<string, object>;
            var     newDependencies         = new List<KeyValuePair<string, string>>();
            foreach (var file in assetManifestFiles)
            {
                var     assetManifestDict   = OpenManifestAsObject(file);
                if (assetManifestDict.TryGetValue(UnityPackageManifestKey.kDependencies, out object assetDependencies))
                {
                    var     assetDependenciesDict    = assetDependencies as Dictionary<string, object>;
                    foreach (var dependency in assetDependenciesDict)
                    {
                        if (registeredDependencies.ContainsKey(dependency.Key)) continue;

                        AddDependencyToList(ref newDependencies, dependency.Key, (string)dependency.Value);
                    }
                }
            }

            // Check whether we have any unregisted dependecies
            if (newDependencies.Count > 0)
            {
                EditorApplication.delayCall += () =>
                {
                    PromptUserToMergeDependency(newDependencies);
                };
            }
        }

        private static void AddDependencyToList(ref List<KeyValuePair<string, string>> list, string name, string versionOrPath)
        {
            int     existingItemIndex;
            if ((existingItemIndex = list.FindIndex((item) => string.Equals(item.Key, name))) == -1)
            {
                list.Add(new KeyValuePair<string, string>(name, versionOrPath));
            }
            else if (string.Compare(list[existingItemIndex].Value, versionOrPath) >= 0)
            {
                list[existingItemIndex] = new KeyValuePair<string, string>(name, versionOrPath);
            }
        }

        private static Dictionary<string, object> OpenManifestAsObject(string path)
        {
            var     contents    = File.ReadAllText(path);
            return JsonUtility.FromJson(contents) as Dictionary<string, object>;
        }

        private static Dictionary<string, object> GetOrCreateMainManifestObject()
        {
            if (s_mainManifestDict == null)
            {
                s_mainManifestDict  = OpenManifestAsObject(kMainManifestPath);
            }
            return s_mainManifestDict;
        }

        private static void PromptUserToMergeDependency(List<KeyValuePair<string, string>> dependencies)
        {
            if (EditorUtility.DisplayDialog(
                title: "Resolve Dependencies",
                message: "System has detected that there are one or more dependecies used by the Asset-Packages that are missing from the Package-Manifest file. Do you approve adding it to the Package-Manifest file?",
                ok: "Ok",
                cancel: "Cancel"))
            {
                PerformMergeDependency(dependencies);
                return;
            }
        }

        private static void PerformMergeDependency(List<KeyValuePair<string, string>> values)
        {
            // Update dependency collection
            var     contentDict         = GetOrCreateMainManifestObject();
            var     dependenciesDict    = contentDict[UnityPackageManifestKey.kDependencies]as Dictionary<string, object>;
            bool    isDirty             = false;
            foreach (var newValue in values)
            {
                if (!dependenciesDict.ContainsKey(newValue.Key))
                {
                    dependenciesDict.Add(newValue.Key, newValue.Value);
                    isDirty = true;
                }
            }

            // Save changes
            if (isDirty)
            {
                string  jsonStr = JsonUtility.ToJson(contentDict);
                File.WriteAllText(kMainManifestPath, jsonStr);

                CompilationPipeline.RequestScriptCompilation();
            }
        }

        #endregion*/
    }
}