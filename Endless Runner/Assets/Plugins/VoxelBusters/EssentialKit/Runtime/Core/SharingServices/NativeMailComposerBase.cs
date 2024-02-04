using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit.SharingServicesCore
{
    public abstract class NativeMailComposerBase : NativeObjectBase, INativeMailComposer
    {
        #region INativeMailComposer implementation

        public event MailComposerClosedInternalCallback OnClose;

        public abstract void SetToRecipients(params string[] values);
        
        public abstract void SetCcRecipients(params string[] values);
        
        public abstract void SetBccRecipients(params string[] values);
        
        public abstract void SetSubject(string value);
        
        public abstract void SetBody(string value, bool isHtml);

        public abstract void AddScreenshot(string fileName);

        public abstract void AddAttachmentData(byte[] data, string mimeType, string fileName);

        public abstract void Show();

        #endregion

        #region Private methods

        protected void SendCloseEvent(MailComposerResultCode resultCode, Error error)
        {
            CallbackDispatcher.InvokeOnMainThread(() => OnClose(resultCode, error));
        }

        #endregion
    }
}