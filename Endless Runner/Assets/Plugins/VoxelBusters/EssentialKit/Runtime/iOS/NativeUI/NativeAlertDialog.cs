#if UNITY_IOS || UNITY_TVOS
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using AOT;
using VoxelBusters.CoreLibrary.NativePlugins;
using VoxelBusters.CoreLibrary.NativePlugins.iOS;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.EssentialKit.NativeUICore.iOS
{
    internal sealed class NativeAlertDialog : NativeAlertDialogInterfaceBase, INativeAlertDialogInterface
    {
        #region Constructors

        static NativeAlertDialog()
        {
            // initialise component
            AlertControllerBinding.NPAlertControllerRegisterCallback(actionSelectCallback: HandleAlertActionSelectCallbackInternal);
        }

        public NativeAlertDialog(AlertDialogStyle alertStyle)
        {
            // prepare component
            var     nativePtr   = AlertControllerBinding.NPAlertControllerCreate(title: string.Empty, message: string.Empty, preferredStyle: AlertControllerUtility.ConvertToUIAlertControllerStyle(alertStyle));
            Assert.IsFalse(IntPtr.Zero == nativePtr, Diagnostics.kCreateNativeObjectError);

            // set property
            NativeObjectRef     = new IosNativeObjectRef(nativePtr, retain: false);

            // add to collection to map action
            NativeInstanceMap.AddInstance(nativePtr, this);
        }

        ~NativeAlertDialog()
        {
            Dispose(false);
        }

        #endregion

        #region Base class methods

        public override void SetTitle(string value)
        {
            AlertControllerBinding.NPAlertControllerSetTitle(AddrOfNativeObject(), value);
        }

        public override string GetTitle()
        {
            return AlertControllerBinding.NPAlertControllerGetTitle(AddrOfNativeObject());
        }
            
        public override void SetMessage(string value)
        {
            AlertControllerBinding.NPAlertControllerSetMessage(AddrOfNativeObject(), value);
        }

        public override string GetMessage()
        {
            return AlertControllerBinding.NPAlertControllerGetMessage(AddrOfNativeObject());
        }

        public override void AddButton(string text, bool isCancelType)
        {
            AlertControllerBinding.NPAlertControllerAddAction(AddrOfNativeObject(), text, isCancelType);
        }

        public override void Show()
        {
            AlertControllerBinding.NPAlertControllerShow(AddrOfNativeObject());
        }

        public override void Dismiss()
        {
            AlertControllerBinding.NPAlertControllerDismiss(AddrOfNativeObject());
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

        [MonoPInvokeCallback(typeof(AlertActionSelectNativeCallback))]
        private static void HandleAlertActionSelectCallbackInternal(IntPtr nativePtr, int selectedButtonIndex)
        {
            var     owner   = NativeInstanceMap.GetOwner<NativeAlertDialog>(nativePtr);
            Assert.IsPropertyNotNull(owner, "owner");

            owner.SendButtonClickEvent(selectedButtonIndex);
        }

        #endregion
    }
}
#endif