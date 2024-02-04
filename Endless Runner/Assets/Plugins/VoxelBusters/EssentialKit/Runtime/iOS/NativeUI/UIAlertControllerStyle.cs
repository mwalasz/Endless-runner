#if UNITY_IOS || UNITY_TVOS
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.EssentialKit.NativeUICore.iOS
{
    internal enum UIAlertControllerStyle : long
    {
        UIAlertControllerStyleActionSheet = 0,

        UIAlertControllerStyleAlert,
    }
}
#endif