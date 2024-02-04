#if UNITY_ANDROID
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System;
using UnityEngine;
using AOT;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins;
using VoxelBusters.CoreLibrary.NativePlugins.Android;
using VoxelBusters.EssentialKit.Common.Android;

namespace VoxelBusters.EssentialKit.SharingServicesCore.Android
{
    public sealed class SocialShareComposer : NativeSocialShareComposerBase, INativeSocialShareComposer
    {
#region Fields

        private NativeSocialShareComposer m_instance;

#endregion

#region Constructors

        public SocialShareComposer(SocialShareComposerType composerType)
        {
            m_instance = new NativeSocialShareComposer(NativeUnityPluginUtility.GetContext());
            m_instance.SetComposerType(Converter.from(composerType));
        }

        ~SocialShareComposer()
        {
            Dispose(false);
        }

#endregion

#region Static methods

        internal static bool IsComposerAvailable(SocialShareComposerType composerType)
        {
            return NativeSocialShareComposer.IsComposerAvailable(NativeUnityPluginUtility.GetContext(), Converter.from(composerType));
        }

#endregion

#region Base class methods

        public override void SetText(string value)
        {
            m_instance.SetText(value);
        }

        public override void AddScreenshot()
        {
            VoxelBusters.CoreLibrary.NativePlugins.Android.Utility.TakeScreenshot((byte[] data, string mimeType) =>
            {
                AddImage(data);
            });
        }

        public override void AddImage(byte[] imageData)
        {
            m_instance.AddAttachment(new NativeBytesWrapper(imageData), MimeType.kJPGImage, DateTime.Now.Ticks.ToString() + ".jpg");
        }

        public override void AddURL(URLString url)
        {
            m_instance.SetUrl(url.ToString());
        }

        public override void Show(Vector2 screenPosition)
        {
            SurrogateCoroutine.WaitForEndOfFrameAndInvoke(() =>
            {
                m_instance.Show(new NativeSocialShareComposerListener()
                {
                    onActionCallback = (result) =>
                    {
                        SendCloseEvent(Converter.from(result), null);
                    }
                });
            });
        }

#endregion
    }
}
#endif