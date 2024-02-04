using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.EssentialKit
{
    public class DatePickerResult
    {
        #region Properties

        public DateTime? SelectedDate { get; private set; }

        #endregion

        #region Constructors

        internal DatePickerResult(DateTime? selectedDate)
        {
            // Set properties
            SelectedDate    = selectedDate;
        }

        #endregion
    }
}