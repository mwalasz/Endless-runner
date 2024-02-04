#if UNITY_IOS || UNITY_TVOS
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.iOS.Xcode;

namespace VoxelBusters.CoreLibrary.Editor.NativePlugins.Build.Xcode
{
    public abstract class PBXNativePluginsProcessor : NativePluginsProcessor
    {
        #region Postprocess message methods

        public virtual void OnUpdateInfoPlist(PlistDocument doc)
        { }

        public virtual void OnUpdateCapabilities(ProjectCapabilityManager capabilityManager)
        { }

        public virtual void OnUpdateMacroDefinitions(MacroDefinitionsManager manager)
        { }

        #endregion
    }
}
#endif