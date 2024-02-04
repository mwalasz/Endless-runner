using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using VoxelBusters.CoreLibrary.Editor;

namespace VoxelBusters.CoreLibrary.Editor.NativePlugins.Build
{
    [CustomEditor(typeof(NativePluginsExporterObject))]
    public class NativePluginsExporterObjectEditor : CustomInspector
    { }
}