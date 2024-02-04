#if UNITY_ANDROID
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins.Android;
using VoxelBusters.EssentialKit.Common.Android;

namespace VoxelBusters.EssentialKit.SharingServicesCore.Android
{
    internal sealed class MessageComposer : NativeMessageComposerBase, INativeMessageComposer
    {
#region Fields

        private NativeMessageComposer m_instance;

#endregion

#region Constructors

        public MessageComposer()
        {
            m_instance = new NativeMessageComposer(NativeUnityPluginUtility.GetContext());
        }

        ~MessageComposer()
        {
            Dispose(false);
        }

#endregion

#region Static methods

        public static bool CanSendText()
        {
            return NativeMessageComposer.CanSendText(NativeUnityPluginUtility.GetContext());
        }

        public static bool CanSendAttachments()
        {
            return NativeMessageComposer.CanSendAttachments(NativeUnityPluginUtility.GetContext());
        }

        public static bool CanSendSubject()
        {
            return NativeMessageComposer.CanSendSubject(NativeUnityPluginUtility.GetContext());
        }

#endregion

#region Base class methods

        public override void SetRecipients(params string[] values)
        {
            m_instance.SetRecipients(values);
        }

        public override void SetSubject(string value)
        {
            m_instance.SetSubject(value);
        }

        public override void SetBody(string value)
        {
            m_instance.SetBody(value, false);
        }

        public override void AddScreenshot(string fileName)
        {
            VoxelBusters.CoreLibrary.NativePlugins.Android.Utility.TakeScreenshot((byte[] data, string mimeType) =>
            {
                AddAttachmentData(data, mimeType, fileName);
            });
        }

        public override void AddImage(Texture2D image, string fileName)
        {
            string  mimeType;
            byte[]  data        = image.Encode(out mimeType);
            AddAttachmentData(data, mimeType, fileName);
        }

        public override void AddAttachmentData(byte[] data, string mimeType, string fileName)
        {
            m_instance.AddAttachment(new NativeBytesWrapper(data), mimeType, fileName);
        }

        public override void Show()
        {
            SurrogateCoroutine.WaitForEndOfFrameAndInvoke(() =>
            {
                m_instance.Show(new NativeMessageComposerListener()
                {
                    onActionCallback = (result) => SendCloseEvent(Converter.from(result), null)
                });
            });
        }

#endregion
    }
}
#endif