using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit.SharingServicesCore
{
    public interface INativeMessageComposer : INativeObject
    {
        #region Events

        event MessageComposerClosedInternalCallback OnClose;

        #endregion

        #region Methods

        // setter methods
        void SetRecipients(params string[] values);

        void SetSubject(string value);

        void SetBody(string value);

        // attachment methods
        void AddScreenshot(string fileName);
        
        void AddImage(Texture2D image, string fileName);
        
        void AddAttachmentData(byte[] data, string mimeType, string fileName);
        
        // presentation methods
        void Show();

        #endregion
    }
}