#if UNITY_ANDROID
using VoxelBusters.CoreLibrary.NativePlugins;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins.Android;
using UnityEngine;
using VoxelBusters.EssentialKit.Common.Android;

namespace VoxelBusters.EssentialKit.SharingServicesCore.Android
{
    internal sealed class MailComposer : NativeMailComposerBase, INativeMailComposer
    {
#region Fields

        private NativeMailComposer m_instance;

#endregion

#region Constructors

        public MailComposer()
        {
            m_instance = new NativeMailComposer(NativeUnityPluginUtility.GetContext());
        }

        ~MailComposer()
        {
            Dispose(false);
        }

#endregion

#region Static methods

        public static bool CanSendMail()
        {
            return NativeMailComposer.CanSendMail(NativeUnityPluginUtility.GetContext());
        }

#endregion

#region Base class methods

        public override void SetToRecipients(params string[] values)
        {
            m_instance.SetToRecipients(values);
        }

        public override void SetCcRecipients(params string[] values)
        {
            m_instance.SetCcRecipients(values);
        }

        public override void SetBccRecipients(params string[] values)
        {
            m_instance.SetBccRecipients(values);
        }
        
        public override void SetSubject(string value)
        {
            m_instance.SetSubject(value);
        }

        public override void SetBody(string value, bool isHtml)
        {
            m_instance.SetBody(value, isHtml);
        }

        public override void AddScreenshot(string fileName)
        {
            VoxelBusters.CoreLibrary.NativePlugins.Android.Utility.TakeScreenshot((byte[] data, string mimeType) =>
            {
                AddAttachmentData(data, mimeType, fileName);
            });
        }

        public override void AddAttachmentData(byte[] data, string mimeType, string fileName)
        {
            m_instance.AddAttachment(new NativeBytesWrapper(data), mimeType, fileName);
        }

        public override void Show()
        {
            SurrogateCoroutine.WaitForEndOfFrameAndInvoke(() =>
            {
                m_instance.Show(new NativeMailComposerListener()
                {
                    onActionCallback = (result) => SendCloseEvent(Converter.from(result), null)
                });
            });
            
        }
        
#endregion
    }
}
#endif