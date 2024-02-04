#if UNITY_IOS || UNITY_TVOS
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.EssentialKit.SharingServicesCore.iOS
{
    internal delegate void MailComposerClosedNativeCallback(IntPtr nativePtr, MFMailComposeResult result, string error);

    internal delegate void MessageComposerClosedNativeCallback(IntPtr nativePtr, MFMessageComposeResult result);

    internal delegate void ShareSheetClosedNativeCallback(IntPtr nativePtr, bool completed, string error);
    
    internal delegate void SocialShareComposerClosedNativeCallback(IntPtr nativePtr, SLComposeViewControllerResult result);
}
#endif