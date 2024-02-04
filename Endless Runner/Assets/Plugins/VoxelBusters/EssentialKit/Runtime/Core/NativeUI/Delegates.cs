using System;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.EssentialKit.NativeUICore
{
    public delegate void AlertButtonClickInternalCallback(int selectedButtonIndex);

    public delegate void DatePickerClosedInternalCallback(DateTime? selectedDate, Error error);
}