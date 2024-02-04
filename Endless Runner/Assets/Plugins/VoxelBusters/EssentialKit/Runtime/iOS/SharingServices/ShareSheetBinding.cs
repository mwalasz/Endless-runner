#if UNITY_IOS || UNITY_TVOS
using System;
using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit.SharingServicesCore.iOS
{
    internal static class ShareSheetBinding
    {
        [DllImport("__Internal")]
        public static extern void NPShareSheetRegisterCallback(ShareSheetClosedNativeCallback closedCallback);

        [DllImport("__Internal")]
        public static extern IntPtr NPShareSheetCreate();

        [DllImport("__Internal")]
        public static extern void NPShareSheetShow(IntPtr nativePtr, float posX, float posY);

        [DllImport("__Internal")]
        public static extern void NPShareSheetAddText(IntPtr nativePtr, string value);
        
        [DllImport("__Internal")]
        public static extern void NPShareSheetAddScreenshot(IntPtr nativePtr);
        
        [DllImport("__Internal")]
        public static extern void NPShareSheetAddImage(IntPtr nativePtr, IntPtr dataArrayPtr, int dataLength);
        
        [DllImport("__Internal")]
        public static extern void NPShareSheetAddURL(IntPtr nativePtr, string url);

        [DllImport("__Internal")]
        public static extern void NPShareSheetAddAttachment(IntPtr nativePtr, UnityAttachment data);

    }
}
#endif