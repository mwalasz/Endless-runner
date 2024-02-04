#if UNITY_IOS || UNITY_TVOS
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit.NativeUICore.iOS
{
    internal static class AlertControllerUtility
    {
        #region Converter methods

        public static UIAlertControllerStyle ConvertToUIAlertControllerStyle(AlertDialogStyle style)
        {
            switch (style)
            {
                case AlertDialogStyle.Default:
                    return UIAlertControllerStyle.UIAlertControllerStyleAlert;

                case AlertDialogStyle.ActionSheet:
                    return UIAlertControllerStyle.UIAlertControllerStyleActionSheet;

                default:
                    throw VBException.SwitchCaseNotImplemented(style);
            }
        }

        #endregion
    }
}
#endif