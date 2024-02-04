using System;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

namespace VoxelBusters.CoreLibrary.Editor.NativePlugins.Build
{
    public abstract class NativePluginsManager : IPreprocessBuildWithReport, IPostprocessBuildWithReport
    {
        #region Proprerties

        public BuildReport Report { get; private set; }

        public BuildTarget BuildTarget => Report.summary.platform;

        public string OutputPath => Report.summary.outputPath;

        public bool IsPreprocessing { get; set; }

        public bool IsPostprocessing { get; set; }

        #endregion

        #region Static methods

        protected static T[] FindPluginsProcessors<T>(NativePluginsManager manager) where T : NativePluginsProcessor
        {
            var     baseClassType   = typeof(T);
            var     targetTypes     = ReflectionUtility.FindAllTypes((type) => !type.IsAbstract && type.IsSubclassOf(baseClassType));
            return Array.ConvertAll(
                targetTypes,
                (type) =>
                {
                    var     processor   = ReflectionUtility.CreateInstance(type) as T;
                    processor.SetPropertyValue("Manager", manager);
                    return processor;
                });
        }

        protected static LinkXmlWriter CreateDefaultLinkXmlWriter()
        {
            var     defaultPath     = IOServices.GetAbsolutePath("Assets/Plugins/VoxelBusters/link.xml");
            return new LinkXmlWriter(defaultPath);
        }

        #endregion

        #region Base class methods

        protected virtual bool IsSupported(BuildTarget target) => true;

        protected abstract void OnPreprocessNativePlugins();

        protected abstract void OnPostprocessNativePlugins();

        #endregion

        #region IPreprocessBuildWithReport implementation

        int IOrderedCallback.callbackOrder => int.MaxValue;

        void IPreprocessBuildWithReport.OnPreprocessBuild(BuildReport report)
        {
            if (!IsSupported(target: report.summary.platform)) return;

            try
            {
                // Set properties
                Report              = report;
                IsPreprocessing     = true;

                // Send appropriate message
                OnPreprocessNativePlugins();
            }
            finally
            {
                // Reset properties
                IsPreprocessing     = false;
            }
        }

        #endregion

        #region IPostprocessBuildWithReport implementation

        void IPostprocessBuildWithReport.OnPostprocessBuild(BuildReport report)
        {
            if (!IsSupported(target: report.summary.platform)) return;

            try
            {
                // Set properties
                Report              = report;
                IsPostprocessing    = true;

                // Send appropriate messages
                OnPostprocessNativePlugins();
            }
            finally
            {
                // Reset properties
                IsPostprocessing    = false;
            }
        }

        #endregion
    }
}