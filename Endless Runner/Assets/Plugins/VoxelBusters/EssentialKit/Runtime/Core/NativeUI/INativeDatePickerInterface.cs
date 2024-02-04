using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit.NativeUICore
{
    public interface INativeDatePickerInterface : INativeObject
    {
        #region Properties

        DatePickerMode Mode { get; }

        #endregion

        #region Events

        event DatePickerClosedInternalCallback OnClose;

        #endregion

        #region Methods

        void SetKind(DateTimeKind value);

        void SetMinimumDate(DateTime? value);

        void SetMaximumDate(DateTime? value);

        void SetInitialDate(DateTime? value);

        void Show();

        #endregion
    }
}