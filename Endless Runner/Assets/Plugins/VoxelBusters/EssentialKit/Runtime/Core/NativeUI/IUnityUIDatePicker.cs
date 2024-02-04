using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.EssentialKit
{
    public interface IUnityUIDatePicker
    {
        #region Properties

        DatePickerMode Mode { get; set; }

        DateTime? MinDate { get; set; }

        DateTime? MaxDate { get; set; }

        DateTime? InitialDate { get; set; }

        DateTimeKind Kind { get; set; }

        DateTime SelectedDate { get; set; }
        
        #endregion

        #region Methods

        void Show();

        void Dismiss();

        void SetCompletionCallback(EventCallback<DateTime?> callback);

        #endregion
    }
}