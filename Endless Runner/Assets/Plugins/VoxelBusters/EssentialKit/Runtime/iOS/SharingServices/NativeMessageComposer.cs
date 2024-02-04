#if UNITY_IOS || UNITY_TVOS
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System;
using UnityEngine;
using AOT;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins;
using VoxelBusters.CoreLibrary.NativePlugins.iOS;

namespace VoxelBusters.EssentialKit.SharingServicesCore.iOS
{
    internal sealed class NativeMessageComposer : NativeMessageComposerBase, INativeMessageComposer
    {
        #region Constructors

        static NativeMessageComposer()
        {
            // register for callbacks
            MessageComposerBinding.NPMessageComposerRegisterCallback(closedCallback: HandleMessageComposerClosedCallbackInternal);
        }

        public NativeMessageComposer()
        {
             // create object
            var     nativePtr   = MessageComposerBinding.NPMessageComposerCreate();
            Assert.IsFalse(IntPtr.Zero == nativePtr, Diagnostics.kCreateNativeObjectError);

            // set properties
            NativeObjectRef     = new IosNativeObjectRef(nativePtr, retain: false);

            // track instance
            NativeInstanceMap.AddInstance(nativePtr, this);
        }

        ~NativeMessageComposer()
        {
            Dispose(false);
        }

        #endregion

        #region Static methods

        public static bool CanSendText()
        {
            return MessageComposerBinding.NPMessageComposerCanSendText();
        }

        public static bool CanSendAttachments()
        {
            return MessageComposerBinding.NPMessageComposerCanSendAttachments();
        }

        public static bool CanSendSubject()
        {
            return MessageComposerBinding.NPMessageComposerCanSendSubject();
        }

        #endregion

        #region Base class methods

        public override void SetRecipients(params string[] values)
        {
            int     count   = values.Length;
            MessageComposerBinding.NPMessageComposerSetRecipients(AddrOfNativeObject(), values, count);
        }

        public override void SetSubject(string value)
        {
            MessageComposerBinding.NPMessageComposerSetSubject(AddrOfNativeObject(), value);
        }

        public override void SetBody(string value)
        {
            MessageComposerBinding.NPMessageComposerSetBody(AddrOfNativeObject(), value);
        }

        public override void AddScreenshot(string fileName)
        {
            MessageComposerBinding.NPMessageComposerAddScreenshot(AddrOfNativeObject(), fileName);
        }

        public override void AddImage(Texture2D image, string fileName)
        {
            string  mimeType;
            byte[]  data        = image.Encode(out mimeType);
            AddAttachmentData(data, mimeType, fileName);
        }

        public override void AddAttachmentData(byte[] data, string mimeType, string fileName)
        {
            // create data
            var     handle          = GCHandle.Alloc(data, GCHandleType.Pinned);
            var     attachmentData  = new UnityAttachment()
            {
                DataArrayLength     = data.Length,
                DataArrayPtr        = handle.AddrOfPinnedObject(),
                MimeTypePtr         = Marshal.StringToHGlobalAuto(mimeType),
                FileNamePtr         = Marshal.StringToHGlobalAuto(fileName),
            };
            MessageComposerBinding.NPMessageComposerAddAttachment(AddrOfNativeObject(), attachmentData);

            // release pinned data object
            handle.Free();
        }

        public override void Show()
        {
            MessageComposerBinding.NPMessageComposerShow(AddrOfNativeObject());
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

        [MonoPInvokeCallback(typeof(MessageComposerClosedNativeCallback))]
        private static void HandleMessageComposerClosedCallbackInternal(IntPtr nativePtr, MFMessageComposeResult result)
        {
            var     owner       = NativeInstanceMap.GetOwner<NativeMessageComposer>(nativePtr);
            Assert.IsPropertyNotNull(owner, "owner");

            // send result
            owner.SendCloseEvent(SharingUtility.ConvertToMessageComposerResultCode(result), null);
        }

        #endregion
    }
}
#endif