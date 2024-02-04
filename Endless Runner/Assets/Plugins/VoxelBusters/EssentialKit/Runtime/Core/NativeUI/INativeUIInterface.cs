using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit.NativeUICore
{
    public interface INativeUIInterface : INativeFeatureInterface
    {
        #region Methods

        INativeAlertDialogInterface CreateAlertDialog(AlertDialogStyle style);

        INativeDatePickerInterface CreateDatePicker(DatePickerMode mode);

        #endregion
    }
}