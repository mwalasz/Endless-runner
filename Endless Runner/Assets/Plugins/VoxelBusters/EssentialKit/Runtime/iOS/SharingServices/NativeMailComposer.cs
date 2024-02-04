#if UNITY_IOS || UNITY_TVOS
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.InteropServices;
using UnityEngine;
using AOT;
using VoxelBusters.CoreLibrary.NativePlugins;
using VoxelBusters.CoreLibrary.NativePlugins.iOS;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.EssentialKit.SharingServicesCore.iOS
{
    internal sealed class NativeMailComposer : NativeMailComposerBase, INativeMailComposer
    {
        #region Constructors

        static NativeMailComposer()
        {
            // register callbacks
            MailComposerBinding.NPMailComposerRegisterCallback(closedCallback: HandleMailComposerClosedCallbackInternal);
        }

        public NativeMailComposer()
        {
             // create object
            var     nativePtr   = MailComposerBinding.NPMailComposerCreate();
            Assert.IsFalse(IntPtr.Zero == nativePtr, Diagnostics.kCreateNativeObjectError);

            // set properties
            NativeObjectRef     = new IosNativeObjectRef(nativePtr, retain: false);

            // track instance
            NativeInstanceMap.AddInstance(nativePtr, this);
        }

        ~NativeMailComposer()
        {
            Dispose(false);
        }

        #endregion

        #region Static methods

        public static bool CanSendMail()
        {
            return MailComposerBinding.NPMailComposerCanSendMail();
        }

        #endregion

        #region Base class methods

        public override void SetToRecipients(params string[] values)
        {
            int     count   = values.Length;
            MailComposerBinding.NPMailComposerSetRecipients(AddrOfNativeObject(), MailRecipientType.To, values, count);
        }

        public override void SetCcRecipients(params string[] values)
        {
            int     count   = values.Length;
            MailComposerBinding.NPMailComposerSetRecipients(AddrOfNativeObject(), MailRecipientType.Cc, values, count);
        }

        public override void SetBccRecipients(params string[] values)
        {
            int     count   = values.Length;
            MailComposerBinding.NPMailComposerSetRecipients(AddrOfNativeObject(), MailRecipientType.Bcc, values, count);
        }
        
        public override void SetSubject(string value)
        {
            MailComposerBinding.NPMailComposerSetSubject(AddrOfNativeObject(), value);
        }

        public override void SetBody(string value, bool isHtml)
        {
            MailComposerBinding.NPMailComposerSetBody(AddrOfNativeObject(), value, isHtml);
        }

        public override void AddScreenshot(string fileName)
        {
            MailComposerBinding.NPMailComposerAddScreenshot(AddrOfNativeObject(), fileName);
        }

        public override void AddAttachmentData(byte[] data, string mimeType, string fileName)
        {
            // create data
            GCHandle            handle          = GCHandle.Alloc(data, GCHandleType.Pinned);
            UnityAttachment     attachmentData  = new UnityAttachment()
            {
                DataArrayLength = data.Length,
                DataArrayPtr    = handle.AddrOfPinnedObject(),
                MimeTypePtr     = Marshal.StringToHGlobalAuto(mimeType),
                FileNamePtr     = Marshal.StringToHGlobalAuto(fileName),
            };
            MailComposerBinding.NPMailComposerAddAttachment(AddrOfNativeObject(), attachmentData);

            // release pinned data object
            handle.Free();
        }

        public override void Show()
        {
            MailComposerBinding.NPMailComposerShow(AddrOfNativeObject());
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

        #region Native callback methods

        [MonoPInvokeCallback(typeof(MailComposerClosedNativeCallback))]
        private static void HandleMailComposerClosedCallbackInternal(IntPtr nativePtr, MFMailComposeResult result, string error)
        {
            var     owner   = NativeInstanceMap.GetOwner<NativeMailComposer>(nativePtr);
            Assert.IsPropertyNotNull(owner, "owner");

            // send result
            var     errorObj    = Error.CreateNullableError(description: error);
            owner.SendCloseEvent(SharingUtility.ConvertToMailComposerResultCode(result), errorObj);
        }

        #endregion
    }
}
#endif