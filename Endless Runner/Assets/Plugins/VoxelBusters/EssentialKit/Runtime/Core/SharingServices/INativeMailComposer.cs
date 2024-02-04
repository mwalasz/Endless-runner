using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit.SharingServicesCore
{
    public interface INativeMailComposer : INativeObject
    {
        #region Events

        event MailComposerClosedInternalCallback OnClose;

        #endregion

        #region Methods

        // setter methods
        void SetToRecipients(params string[] values);

        void SetCcRecipients(params string[] values);

        void SetBccRecipients(params string[] values);

        void SetSubject(string value);

        void SetBody(string value, bool isHtml);

        // attachments
        void AddScreenshot(string fileName);
        
        void AddAttachmentData(byte[] data, string mimeType, string fileName);

        // presentation methods
        void Show();

        #endregion
    }
}