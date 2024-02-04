using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace VoxelBusters.CoreLibrary.Editor.NativePlugins.Build
{
    public class NativePluginsProcessor
    {
        #region Properties

        public NativePluginsManager Manager { get; private set; }

        public BuildReport BuildReport => Manager.Report;

        public BuildTarget BuildTarget => BuildReport.summary.platform;

        public string OutputPath => BuildReport.summary.outputPath;

        #endregion

        #region Preprocess message methods


        public virtual void OnCheckConfiguration()
        { }

        public virtual void OnUpdateExporterObjects()
        { }

        public virtual void OnUpdateLinkXml(LinkXmlWriter writer)
        { }

        #endregion

        #region Postprocess message methods

        public virtual void OnAddFiles()
        { }

        public virtual void OnAddFolders()
        { }

        public virtual void OnAddResources()
        { }

        public virtual void OnUpdateConfiguration()
        { }

        #endregion
    }
}