#if UNITY_IOS || UNITY_TVOS
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace VoxelBusters.EssentialKit.NativeUICore.iOS
{
    internal static class AlertControllerBinding 
    {
        [DllImport("__Internal")]
        public static extern void NPAlertControllerRegisterCallback(AlertActionSelectNativeCallback actionSelectCallback);

        [DllImport("__Internal")]
        public static extern IntPtr NPAlertControllerCreate(string title, string message, UIAlertControllerStyle preferredStyle);

        [DllImport("__Internal")]
        public static extern void NPAlertControllerShow(IntPtr nativePtr);

        [DllImport("__Internal")]
        public static extern void NPAlertControllerDismiss(IntPtr nativePtr);

        [DllImport("__Internal")]
        public static extern void NPAlertControllerSetTitle(IntPtr nativePtr, string value);

        [DllImport("__Internal")]
        public static extern string NPAlertControllerGetTitle(IntPtr nativePtr);

        [DllImport("__Internal")]
        public static extern void NPAlertControllerSetMessage(IntPtr nativePtr, string value);

        [DllImport("__Internal")]
        public static extern string NPAlertControllerGetMessage(IntPtr nativePtr);
        
        [DllImport("__Internal")]
        public static extern void NPAlertControllerAddAction(IntPtr nativePtr, string text, bool isCancelType);

    }
}
#endif