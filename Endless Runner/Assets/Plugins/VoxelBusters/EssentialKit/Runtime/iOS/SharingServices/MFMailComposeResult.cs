#if UNITY_IOS || UNITY_TVOS
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.EssentialKit.SharingServicesCore.iOS
{
    internal enum MFMailComposeResult : long
    {
        MFMailComposeResultCancelled,

        MFMailComposeResultSaved,

        MFMailComposeResultSent,

        MFMailComposeResultFailed,
    }
}
#endif