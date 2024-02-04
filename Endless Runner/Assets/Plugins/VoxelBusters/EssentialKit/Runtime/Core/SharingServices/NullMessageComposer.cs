using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit.SharingServicesCore
{
    public sealed class NullMessageComposer : NativeMessageComposerBase, INativeMessageComposer
    {
        #region Constructors

        public NullMessageComposer()
        { }

        #endregion
        
        #region Private static methods

        private static void LogNotSupported()
        {
            Diagnostics.LogNotSupported("MessageComposer");
        }

        #endregion

        #region Base class methods

        public override void SetRecipients(params string[] values)
        { 
            LogNotSupported();
        }

        public override void SetSubject(string value)
        { 
            LogNotSupported();
        }

        public override void SetBody(string value)
        { 
            LogNotSupported();
        }

        public override void AddScreenshot(string fileName)
        { 
            LogNotSupported();
        }

        public override void AddImage(Texture2D image, string fileName)
        { 
            LogNotSupported();
        }

        public override void AddAttachmentData(byte[] data, string mimeType, string fileName)
        { 
            LogNotSupported();
        }

        public override void Show()
        {
            LogNotSupported();

            // send result
            SendCloseEvent(MessageComposerResultCode.Failed, Diagnostics.kFeatureNotSupported);
        }

        #endregion
    }
}