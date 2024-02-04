#if UNITY_ANDROID
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System;
using UnityEngine;
using AOT;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins;
using VoxelBusters.CoreLibrary.NativePlugins.Android;
using VoxelBusters.EssentialKit.Common.Android;

namespace VoxelBusters.EssentialKit.SharingServicesCore.Android
{
    public sealed class ShareSheet : NativeShareSheetBase, INativeShareSheet
    {
#region Fields

        private NativeShareSheet m_instance;

#endregion

#region Constructors

        public ShareSheet()
        {
            m_instance = new NativeShareSheet(NativeUnityPluginUtility.GetContext());
        }

        ~ShareSheet()
        {
            Dispose(false);
        }

#endregion

#region Base class methods

        public override void AddText(string text)
        {
            m_instance.SetText(text);
        }

        public override void AddScreenshot()
        {
            VoxelBusters.CoreLibrary.NativePlugins.Android.Utility.TakeScreenshot((byte[] data, string mimeType) =>
            {
                AddImage(data, mimeType);
            });
        }

        public override void AddImage(byte[] imageData, string mimeType)
        {
            AddAttachmentData(imageData, mimeType, DateTime.Now.Ticks.ToString() + GetFileExtension(mimeType));
        }

        public override void AddURL(URLString url)
        {
            m_instance.SetUrl(url.ToString());
        }

        public override void AddAttachment(byte[] data, string mimeType, string filename)
        {
            AddAttachmentData(data, mimeType, filename);
        }

        public override void Show(Vector2 screenPosition)
        {
            SurrogateCoroutine.WaitForEndOfFrameAndInvoke(() =>
            {
                m_instance.Show(new NativeShareSheetListener()
                {
                    onActionCallback = (result) =>
                    {
                        SendCloseEvent(Converter.from(result), null);
                    }
                });
            });
        }

        protected override void Dispose(bool disposing)
        {
            // check whether object is released
            if (IsDisposed)
            {
                return;
            }

            // release all unmanaged type objects
            var     nativePtr   = AddrOfNativeObject();
            if (nativePtr != IntPtr.Zero)
            {
                NativeInstanceMap.RemoveInstance(nativePtr);
            }

            base.Dispose(disposing);
        }

#endregion

#region Private methods

        private void AddAttachmentData(byte[] data, string mimeType, string fileName)
        {
            m_instance.AddAttachment(new NativeBytesWrapper(data), mimeType, fileName);
        }

        private string GetFileExtension(string mimeType)
        {
            if (MimeType.kJPGImage.Equals(mimeType))
            {
                return ".jpg";
            }
            else if (MimeType.kPNGImage.Equals(mimeType))
            {
                return ".png";
            }
            else
            {
                return ".jpg";
            }
        }

#endregion
    }
}
#endif