using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit.SharingServicesCore
{
    public abstract class NativeShareSheetBase : NativeObjectBase, INativeShareSheet
    {
        #region INativeShareSheet implementation

        public event ShareSheetClosedInternalCallback OnClose;

        public abstract void AddText(string text);
        
        public abstract void AddScreenshot();
        
        public abstract void AddImage(byte[] imageData, string mimeType);

        public abstract void AddAttachment(byte[] data, string mimeType, string filename);

        public abstract void AddURL(URLString url);

        public abstract void Show(Vector2 screenPosition);

        #endregion

        #region Private methods

        protected void SendCloseEvent(ShareSheetResultCode resultCode, Error error)
        {
            CallbackDispatcher.InvokeOnMainThread(() => OnClose(resultCode, error));
        }

        #endregion
    }
}