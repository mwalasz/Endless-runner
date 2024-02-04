#if UNITY_IOS || UNITY_TVOS
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit.SharingServicesCore.iOS
{
    internal static class MailComposerBinding
    {
        [DllImport("__Internal")]
        public static extern bool NPMailComposerCanSendMail();

        [DllImport("__Internal")]
        public static extern void NPMailComposerRegisterCallback(MailComposerClosedNativeCallback closedCallback);

        [DllImport("__Internal")]
        public static extern IntPtr NPMailComposerCreate();

        [DllImport("__Internal")]
        public static extern void NPMailComposerShow(IntPtr nativePtr);

        [DllImport("__Internal")]
        public static extern void NPMailComposerSetRecipients(IntPtr nativePtr, MailRecipientType recipientType, string[] recipients, int count);

        [DllImport("__Internal")]
        public static extern void NPMailComposerSetSubject(IntPtr nativePtr, string value);

        [DllImport("__Internal")]
        public static extern void NPMailComposerSetBody(IntPtr nativePtr, string value, bool isHtml);

        [DllImport("__Internal")]
        public static extern void NPMailComposerAddScreenshot(IntPtr nativePtr, string fileName);

        [DllImport("__Internal")]
        public static extern void NPMailComposerAddAttachment(IntPtr nativePtr, UnityAttachment data);
    }
}
#endif