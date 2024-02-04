using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.Editor;

namespace VoxelBusters.EssentialKit.Editor
{
    internal static class EssentialKitPackageLayout
    {
        public static string ExtrasPath => "Assets/Plugins/VoxelBusters/EssentialKit/Essentials";

        public static string EditorExtrasPath => $"{ExtrasPath}/Editor";

        public static string IosPluginPath => $"{ExtrasPath}/Plugins/iOS";

        public static string AndroidPluginPath => $"Assets/Plugins/Android";

        // android
        public static string AndroidEditorSourcePath => $"{EssentialKitSettings.Package.GetEditorScriptsPath()}/BuildPipeline/Android";

        public static string AndroidProjectRootNamespace => "com.voxelbusters.essentialkit";

        public static string AndroidProjectFolderName => $"{AndroidProjectRootNamespace}.androidlib";

        public static string AndroidProjectPath => $"{AndroidPluginPath}/{AndroidProjectFolderName}";

        public static string AndroidProjectAllLibsPath => AndroidProjectPath + "/all_libs";

        public static string AndroidProjectLibsPath => AndroidProjectPath + "/libs";

        public static string AndroidProjectResPath => AndroidProjectPath + "/res";

        public static string AndroidProjectResRawPath => AndroidProjectResPath + "/raw";

        public static string AndroidProjectResDrawablePath => AndroidProjectResPath + "/drawable";

        public static string AndroidProjectResValuesPath => AndroidProjectResPath + "/values";

        public static string AndroidProjectResValuesStringsPath => AndroidProjectResValuesPath + "/essential_kit_strings.xml";
    }
}