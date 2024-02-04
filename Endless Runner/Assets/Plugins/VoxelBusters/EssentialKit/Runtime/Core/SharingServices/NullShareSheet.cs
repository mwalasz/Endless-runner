using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit.SharingServicesCore
{
    public sealed class NullShareSheet : NativeShareSheetBase, INativeShareSheet
    {
        #region Constructors

        public NullShareSheet()
        { }

        #endregion

        #region Private static methods

        private static void LogNotSupported()
        {
            Diagnostics.LogNotSupported("ShareSheet");
        }

        #endregion

        #region Base class methods

        public override void AddText(string text)
        {
            LogNotSupported();
        }

        public override void AddScreenshot()
        {
            LogNotSupported();
        }

        public override void AddImage(byte[] imageData, string mimeType)
        {
            LogNotSupported();
        }

        public override void AddURL(URLString url)
        {
            LogNotSupported();
        }

        public override void AddAttachment(byte[] data, string mimeType, string filename)
        {
            LogNotSupported();
        }

        public override void Show(Vector2 screenPosition)
        {
            LogNotSupported();

            // send result
            SendCloseEvent(ShareSheetResultCode.Cancelled, Diagnostics.kFeatureNotSupported);
        }

        #endregion
    }
}