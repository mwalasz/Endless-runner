using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.CoreLibrary.Editor.NativePlugins
{
    public static class NativePluginsPackageLayout
    {
        public static string ExtrasPath { get { return "Assets/Plugins/VoxelBusters/CoreLibrary"; } }

        public static string IosPluginPath { get { return ExtrasPath + "/Plugins/iOS"; } }
    }
}