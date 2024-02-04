#if UNITY_IOS || UNITY_TVOS
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace VoxelBusters.EssentialKit.NativeUICore.iOS
{
    internal static class DatePickerControllerBinding 
    {
        [DllImport("__Internal")]
        public static extern void NPDatePickerControllerRegisterCallback(DatePickerControllerNativeCallback callback);

        [DllImport("__Internal")]
        public static extern void NPDatePickerControllerShow(UIDatePickerMode mode, long initialEpoch, long minimumEpoch, long maximumEpoch, IntPtr tagPtr);
    }
}
#endif