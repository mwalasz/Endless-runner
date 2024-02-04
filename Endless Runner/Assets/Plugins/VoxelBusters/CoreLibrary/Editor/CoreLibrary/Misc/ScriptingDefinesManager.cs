using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.CoreLibrary.Editor
{
    public static class ScriptingDefinesManager
    {
        #region Static fields

        private     static      Dictionary<BuildTargetGroup, List<string>>      s_addDefinesCollection;

        private     static      Dictionary<BuildTargetGroup, List<string>>      s_removeDefinesCollection;

        private     static      BuildTargetGroup[]                              s_supportedTargetGroups;


        #endregion

        #region Static methods

        public static void AddDefine(string define, params BuildTargetGroup[] targetGroups)
        {
            EnsureInitialized();

            AddDefineToCollection(
                definesCollection: s_addDefinesCollection,
                define: define,
                targetGroups: GetBuildTargetGroupsOrDefault(targetGroups));
            UpdateDefineSymbolsDelayed();
        }

        public static void RemoveDefine(string define, params BuildTargetGroup[] targetGroups)
        {
            EnsureInitialized();

            AddDefineToCollection(
                definesCollection: s_removeDefinesCollection,
                define: define,
                targetGroups: GetBuildTargetGroupsOrDefault(targetGroups));
            UpdateDefineSymbolsDelayed();
        }

        private static void EnsureInitialized()
        {
            if (s_addDefinesCollection != null) return;

            // set properties
            s_addDefinesCollection      = new Dictionary<BuildTargetGroup, List<string>>();
            s_removeDefinesCollection   = new Dictionary<BuildTargetGroup, List<string>>();
            s_supportedTargetGroups     = GetSupportedBuildTargetGroups();
        }

        private static BuildTargetGroup[] GetSupportedBuildTargetGroups()
        {
            var     newList     = new List<BuildTargetGroup>();
            foreach (BuildTarget buildTarget in System.Enum.GetValues(typeof(BuildTarget)))
            {
                var     buildTargetGroup    = BuildPipeline.GetBuildTargetGroup(buildTarget);
                if (BuildPipeline.IsBuildTargetSupported(buildTargetGroup, buildTarget))
                {
                    newList.AddUnique(buildTargetGroup);
                }
            }
            return newList.ToArray();
        }

        private static BuildTargetGroup[] GetBuildTargetGroupsOrDefault(BuildTargetGroup[] targetGroups)
        {
            return ((targetGroups == null) || (targetGroups.Length == 0))
                ? s_supportedTargetGroups
                : targetGroups;
        }

        private static void AddDefineToCollection(Dictionary<BuildTargetGroup, List<string>> definesCollection, string define, BuildTargetGroup[] targetGroups)
        {
            // add define symbol for all the specified target groups
            foreach (var group in targetGroups)
            {
                if (!System.Array.Exists(s_supportedTargetGroups, (item) => (item == group)))
                {
                    continue;
                }

                if (!definesCollection.TryGetValue(group, out List<string> groupDefines))
                {
                    var     newDefines  = new List<string>();
                    definesCollection.Add(group, newDefines);

                    groupDefines        = newDefines;
                }

                groupDefines.AddUnique(define);
            }
        }

        private static void UpdateDefineSymbolsDelayed()
        {
            EditorApplication.delayCall -= UpdateDefineSymbols;
            EditorApplication.delayCall += UpdateDefineSymbols;
        }

        private static void UpdateDefineSymbols()
        {
            try
            {
                EnsureInitialized();
                AssetDatabase.StartAssetEditing();

                foreach (var targetGroup in s_supportedTargetGroups)
                {
                    var     existingDefines = PlayerSettings.GetScriptingDefineSymbolsForGroup(targetGroup).Split(';');
                    var     updatedDefines  = new List<string>(existingDefines);
                    bool    isModified      = false;
                    if (s_addDefinesCollection.TryGetValue(targetGroup, out List<string> addDefines))
                    {
                        foreach (var define in addDefines)
                        {
                            isModified     |= updatedDefines.AddUnique(define);
                        }
                    }
                    if (s_removeDefinesCollection.TryGetValue(targetGroup, out List<string> removeDefines))
                    {
                        foreach (var define in removeDefines)
                        {
                            isModified     |= updatedDefines.Remove(define);
                        }
                    }

                    // set values if there are modifications
                    if (isModified)
                    {
                        PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, string.Join(";", updatedDefines.ToArray()));
                    }
                }
            }
            finally
            {
                AssetDatabase.StopAssetEditing();

                // reset properties
                s_addDefinesCollection.Clear();
                s_removeDefinesCollection.Clear();
            }
        }

        #endregion
    }
}