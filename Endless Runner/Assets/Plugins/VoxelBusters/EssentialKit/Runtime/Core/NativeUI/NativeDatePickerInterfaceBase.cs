using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit.NativeUICore
{
    public abstract class NativeDatePickerInterfaceBase : NativeObjectBase, INativeDatePickerInterface
    {
        #region Properties

        public DatePickerMode Mode { get; private set; }

        #endregion

        #region Constructors

        protected NativeDatePickerInterfaceBase(DatePickerMode mode)
        { 
            // set properties
            Mode    = mode;
        }

        #endregion

        #region INativeDatePickerInterface implementation

        public event DatePickerClosedInternalCallback OnClose;

        public abstract void SetKind(DateTimeKind value);

        public abstract void SetMinimumDate(DateTime? value);

        public abstract void SetMaximumDate(DateTime? value);

        public abstract void SetInitialDate(DateTime? value);

        public abstract void Show();

        #endregion

        #region Private methods

        protected void SendCloseEvent(DateTime? selectedDate, Error error)
        {
            CallbackDispatcher.InvokeOnMainThread(() => OnClose(selectedDate, error));
        }

        #endregion
    }
}