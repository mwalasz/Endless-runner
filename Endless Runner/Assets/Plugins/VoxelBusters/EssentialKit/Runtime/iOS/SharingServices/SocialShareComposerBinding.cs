#if UNITY_IOS || UNITY_TVOS
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System;
using UnityEngine;

namespace VoxelBusters.EssentialKit.SharingServicesCore.iOS
{
    internal static class SocialShareComposerBinding 
    {
        [DllImport("__Internal")]
        public static extern bool NPSocialShareComposerIsComposerAvailable(SocialShareComposerType composerType);
        
        [DllImport("__Internal")]
        public static extern void NPSocialShareComposerRegisterCallback(SocialShareComposerClosedNativeCallback closedCallback);
         
        [DllImport("__Internal")]
        public static extern IntPtr NPSocialShareComposerCreate(SocialShareComposerType composerType);
        
        [DllImport("__Internal")]
        public static extern void NPSocialShareComposerShow(IntPtr nativePtr, float posX, float posY);
        
        [DllImport("__Internal")]
        public static extern void NPSocialShareComposerAddText(IntPtr nativePtr, string value);
        
        [DllImport("__Internal")]
        public static extern void NPSocialShareComposerAddScreenshot(IntPtr nativePtr);
        
        [DllImport("__Internal")]
        public static extern void NPSocialShareComposerAddImage(IntPtr nativePtr, IntPtr dataArrayPtr, int dataLength);
        
        [DllImport("__Internal")]
        public static extern void NPSocialShareComposerAddURL(IntPtr nativePtr, string url);
    }
}
#endif