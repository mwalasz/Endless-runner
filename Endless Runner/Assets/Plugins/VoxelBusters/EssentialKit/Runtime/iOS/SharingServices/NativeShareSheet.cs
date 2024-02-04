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
    public sealed class NativeShareSheet : NativeShareSheetBase, INativeShareSheet
    {
#region Constructors

        static NativeShareSheet()
        {
            // register callbacks
            ShareSheetBinding.NPShareSheetRegisterCallback(closedCallback: HandleShareSheetClosedCallbackInternal);
        }

        public NativeShareSheet()
        {
             // create object
            var     nativePtr   = ShareSheetBinding.NPShareSheetCreate();
            Assert.IsFalse(IntPtr.Zero == nativePtr, Diagnostics.kCreateNativeObjectError);

            // set properties
            NativeObjectRef     = new IosNativeObjectRef(nativePtr, retain: false);

            // track instance
            NativeInstanceMap.AddInstance(nativePtr, this);
        }

        ~NativeShareSheet()
        {
            Dispose(false);
        }

#endregion

#region Base class methods

        public override void AddText(string text)
        {
            ShareSheetBinding.NPShareSheetAddText(AddrOfNativeObject(), text);
        }

        public override void AddScreenshot()
        {
            ShareSheetBinding.NPShareSheetAddScreenshot(AddrOfNativeObject());
        }

        public override void AddImage(byte[] imageData, string mimeType)
        {
            // copy data to managed environment
            var     handle      = GCHandle.Alloc(imageData, GCHandleType.Pinned);

            // send data
            ShareSheetBinding.NPShareSheetAddImage(AddrOfNativeObject(), handle.AddrOfPinnedObject(), imageData.Length);

            // release pinned data object
            handle.Free();
        }

        public override void AddURL(URLString url)
        {
            ShareSheetBinding.NPShareSheetAddURL(AddrOfNativeObject(), url.ToString());
        }

        public override void AddAttachment(byte[] data, string mimeType, string fileName)
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
            ShareSheetBinding.NPShareSheetAddAttachment(AddrOfNativeObject(), attachmentData);

            // release pinned data object
            handle.Free();
        }

        public override void Show(Vector2 screenPosition)
        {
            var     invertedPosition    = UnityEngineUtility.InvertScreenPosition(screenPosition, invertX: false);
            ShareSheetBinding.NPShareSheetShow(AddrOfNativeObject(), invertedPosition.x, invertedPosition.y);
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

        [MonoPInvokeCallback(typeof(ShareSheetClosedNativeCallback))]
        private static void HandleShareSheetClosedCallbackInternal(IntPtr nativePtr, bool completed, string error)
        {
            var     owner       = NativeInstanceMap.GetOwner<NativeShareSheet>(nativePtr);
            Assert.IsPropertyNotNull(owner, "owner");

            // send result
            var     errorObj    = Error.CreateNullableError(description: error);
            owner.SendCloseEvent(completed ? ShareSheetResultCode.Done : ShareSheetResultCode.Cancelled, errorObj);
        }

#endregion
    }
}
#endif