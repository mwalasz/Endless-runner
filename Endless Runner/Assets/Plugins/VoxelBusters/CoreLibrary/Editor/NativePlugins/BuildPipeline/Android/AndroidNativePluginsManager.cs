#if UNITY_EDITOR && UNITY_ANDROID
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Build;
using UnityEditor;

namespace VoxelBusters.CoreLibrary.Editor.NativePlugins.Build.Android
{
    public abstract class AndroidNativePluginsManager : NativePluginsManager
    {

#region Fields

        private     AndroidNativePluginsProcessor[]     m_pluginsProcessors;
        
#endregion

#region Properties

        private AndroidNativePluginsProcessor[] PluginsProcessors => m_pluginsProcessors ?? (m_pluginsProcessors = FindPluginsProcessors<AndroidNativePluginsProcessor>(this));

#endregion

        protected override void OnPreprocessNativePlugins()
        {
            // Send message to complete preprocess actions to all the NativeProcessors
            var linkerFileWriter = CreateDefaultLinkXmlWriter();

            PluginsProcessors.ForEach(
                (item) =>
                {
                    item.OnCheckConfiguration();
                    item.OnUpdateLinkXml(linkerFileWriter);
                    item.OnAddFiles();
                    item.OnAddFolders();
                    item.OnAddResources();
                    item.OnUpdateConfiguration();
                });

            linkerFileWriter.WriteToFile();
        }

        protected override void OnPostprocessNativePlugins()
        { }
    }
}
#endif
