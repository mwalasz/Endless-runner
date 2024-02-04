#if UNITY_IOS || UNITY_TVOS
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit.SharingServicesCore.iOS
{
    internal sealed class MessageComposerBinding
    {
        [DllImport("__Internal")]
        public static extern bool NPMessageComposerCanSendText();

        [DllImport("__Internal")]
        public static extern bool NPMessageComposerCanSendAttachments();

        [DllImport("__Internal")]
        public static extern bool NPMessageComposerCanSendSubject();

        [DllImport("__Internal")]
        public static extern void NPMessageComposerRegisterCallback(MessageComposerClosedNativeCallback closedCallback);

        [DllImport("__Internal")]
        public static extern IntPtr NPMessageComposerCreate();

        [DllImport("__Internal")]
        public static extern void NPMessageComposerShow(IntPtr nativePtr);

        [DllImport("__Internal")]
        public static extern void NPMessageComposerSetRecipients(IntPtr nativePtr, string[] recipients, int count);

        [DllImport("__Internal")]
        public static extern void NPMessageComposerSetSubject(IntPtr nativePtr, string value);

        [DllImport("__Internal")]
        public static extern void NPMessageComposerSetBody(IntPtr nativePtr, string value);

        [DllImport("__Internal")]
        public static extern void NPMessageComposerAddScreenshot(IntPtr nativePtr, string fileName);

        [DllImport("__Internal")]
        public static extern void NPMessageComposerAddAttachment(IntPtr nativePtr, UnityAttachment data);
    }
}
#endif