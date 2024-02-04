using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit.SharingServicesCore
{
    public abstract class NativeMessageComposerBase : NativeObjectBase, INativeMessageComposer
    {
        #region INativeMessageComposer implementation

        public event MessageComposerClosedInternalCallback OnClose;

        public abstract void SetRecipients(params string[] values);

        public abstract void SetSubject(string value);

        public abstract void SetBody(string value);

        public abstract void AddScreenshot(string fileName);

        public abstract void AddImage(Texture2D image, string fileName);

        public abstract void AddAttachmentData(byte[] data, string mimeType, string fileName);

        public abstract void Show();

        #endregion

        #region Private methods

        protected void SendCloseEvent(MessageComposerResultCode resultCode, Error error)
        {
            CallbackDispatcher.InvokeOnMainThread(() => OnClose(resultCode, error));
        }

        #endregion
    }
}