using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace VoxelBusters.CoreLibrary.Editor.NativePlugins.Build
{
    public class UnsupportedPlatformNativePluginsManager : NativePluginsManager
    {
        #region Base class methods

        protected override bool IsSupported(BuildTarget target)
        {
            return !((target == BuildTarget.iOS) ||
                (target == BuildTarget.tvOS) ||
                (target == BuildTarget.Android));
        }

        protected override void OnPreprocessNativePlugins()
        {
            var     pluginsProcessors   = FindPluginsProcessors<UnsupportedPlatformNativePluginsProcessor>(this);
            var     linkerFileWriter    = CreateDefaultLinkXmlWriter();
            pluginsProcessors.ForEach(
                (item) =>
                {
                    item.OnUpdateLinkXml(linkerFileWriter);
                });
            linkerFileWriter.WriteToFile();
        }

        protected override void OnPostprocessNativePlugins()
        { }

        #endregion
    }
}