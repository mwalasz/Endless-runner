using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit.SharingServicesCore
{
    public sealed class NullMailComposer : NativeMailComposerBase, INativeMailComposer
    {
        #region Constructors

        public NullMailComposer()
        { }

        #endregion
        
        #region Private static methods

        private static void LogNotSupported()
        {
            Diagnostics.LogNotSupported("MailComposer");
        }

        #endregion

        #region Base class methods

        public override void SetToRecipients(params string[] values)
        { 
            LogNotSupported();
        }

        public override void SetCcRecipients(params string[] values)
        { 
            LogNotSupported();
        }

        public override void SetBccRecipients(params string[] values)
        { 
            LogNotSupported();
        }

        public override void SetSubject(string value)
        { 
            LogNotSupported();
        }

        public override void SetBody(string value, bool isHtml)
        { 
            LogNotSupported();
        }

        public override void AddScreenshot(string fileName)
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
            SendCloseEvent(MailComposerResultCode.Failed, Diagnostics.kFeatureNotSupported);
        }
        
        #endregion
    }
}