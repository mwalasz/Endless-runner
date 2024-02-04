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
    public sealed class NativeSocialShareComposer : NativeSocialShareComposerBase, INativeSocialShareComposer
    {
        #region Constructors

        static NativeSocialShareComposer()
        {
            // register callbacks
            SocialShareComposerBinding.NPSocialShareComposerRegisterCallback(closedCallback: HandleSocialShareComposerClosedCallbackInternal);
        }

        public NativeSocialShareComposer(SocialShareComposerType composerType)
        {
             // create object
            var     nativePtr   = SocialShareComposerBinding.NPSocialShareComposerCreate(composerType);
            Assert.IsFalse(IntPtr.Zero == nativePtr, Diagnostics.kCreateNativeObjectError);

            // set properties
            NativeObjectRef     = new IosNativeObjectRef(nativePtr, retain: false);

            // track instance
            NativeInstanceMap.AddInstance(nativePtr, this);
        }

        ~NativeSocialShareComposer()
        {
            Dispose(false);
        }

        #endregion

        #region Static methods

        internal static bool IsComposerAvailable(SocialShareComposerType composerType)
        {
            return SocialShareComposerBinding.NPSocialShareComposerIsComposerAvailable(composerType);
        }

        #endregion

        #region Base class methods

        public override void SetText(string value)
        {
            SocialShareComposerBinding.NPSocialShareComposerAddText(AddrOfNativeObject(), value);
        }

        public override void AddScreenshot()
        {
            SocialShareComposerBinding.NPSocialShareComposerAddScreenshot(AddrOfNativeObject());
        }

        public override void AddImage(byte[] imageData)
        {
            // copy data to managed environment
            var     handle      = GCHandle.Alloc(imageData, GCHandleType.Pinned);

            // send data
            SocialShareComposerBinding.NPSocialShareComposerAddImage(AddrOfNativeObject(), handle.AddrOfPinnedObject(), imageData.Length);

            // release pinned data object
            handle.Free();
        }

        public override void AddURL(URLString url)
        {
             SocialShareComposerBinding.NPSocialShareComposerAddURL(AddrOfNativeObject(), url.ToString());
        }

        public override void Show(Vector2 screenPosition)
        {
            var     invertedPosition    = UnityEngineUtility.InvertScreenPosition(screenPosition, invertX: false);
            SocialShareComposerBinding.NPSocialShareComposerShow(AddrOfNativeObject(), invertedPosition.x, invertedPosition.y);
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

        [MonoPInvokeCallback(typeof(SocialShareComposerClosedNativeCallback))]
        private static void HandleSocialShareComposerClosedCallbackInternal(IntPtr nativePtr, SLComposeViewControllerResult result)
        {
            var     owner   = NativeInstanceMap.GetOwner<NativeSocialShareComposer>(nativePtr);
            Assert.IsPropertyNotNull(owner, "owner");

            // send result
            owner.SendCloseEvent(SharingUtility.ConvertToShareComposerResultCode(result), null);
        }

        #endregion
    }
}
#endif