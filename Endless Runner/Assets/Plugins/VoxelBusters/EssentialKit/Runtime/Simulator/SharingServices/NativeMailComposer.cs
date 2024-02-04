using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.EssentialKit.SharingServicesCore.Simulator
{
    public sealed class NativeMailComposer : NativeMailComposerBase, INativeMailComposer
    {
        #region Fields

        private     string[]            m_toRecipients      = null;

        private     string              m_subject           = null;

        private     string              m_body              = null;

        #endregion

        #region Static methods

        public static bool CanSendMail()
        {
            return true;
        }

        #endregion

        #region Base class methods

        public override void SetToRecipients(params string[] values)
        {
            m_toRecipients = values;
        }

        public override void SetCcRecipients(params string[] values)
        {}

        public override void SetBccRecipients(params string[] values)
        {}

        public override void SetSubject(string value)
        {
            m_subject   = value;
        }

        public override void SetBody(string value, bool isHtml)
        {
            m_body      = value;
        } 

        public override void AddScreenshot(string fileName)
        {}

        public override void AddAttachmentData(byte[] data, string mimeType, string fileName)
        {}

        public override void Show()
        {
            // create mailto link
            string  toAddress   = (m_toRecipients != null) ? string.Join(",", m_toRecipients) : string.Empty;
            string  subject     = SystemUtility.EscapeString(m_subject);
            string  body        = SystemUtility.EscapeString(m_body);
            string  expression  = string.Format("mailto:{0}?subject={1}&body={2}", toAddress, subject, body);

            // execute expression
            Application.OpenURL(expression);     
               
            // send result
            SendCloseEvent(MailComposerResultCode.Sent, null);
        }

        #endregion
    }
}