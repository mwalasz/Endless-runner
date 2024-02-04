using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit.SharingServicesCore
{
    public sealed class NullSocialShareComposer : NativeSocialShareComposerBase, INativeSocialShareComposer
    {
        #region Construtors

        public NullSocialShareComposer(SocialShareComposerType composerType)
        { }

        #endregion

        #region Private static methods

        private static void LogNotSupported()
        {
            Diagnostics.LogNotSupported("SocialShareComposer");
        }

        #endregion

        #region Base class methods

        public override void SetText(string value)
        { 
            LogNotSupported();
        }

        public override void AddScreenshot()
        { 
            LogNotSupported();
        }

        public override void AddImage(byte[] imageData)
        { 
            LogNotSupported();
        }

        public override void AddURL(URLString url)
        { 
            LogNotSupported();
        }

        public override void Show(Vector2 screenPosition)
        {
            LogNotSupported();

            // send result
            SendCloseEvent(SocialShareComposerResultCode.Cancelled, Diagnostics.kFeatureNotSupported);
        }

        #endregion
    }
}