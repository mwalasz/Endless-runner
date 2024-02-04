using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace VoxelBusters.CoreLibrary.NativePlugins
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct UnityDateComponents
    {
        #region Properties

        public Calendar Calendar { get; set; }

        public long Year { get; set; }

        public long Month { get; set; }

        public long Day { get; set; }

        public long Hour { get; set; }

        public long Minute { get; set; }

        public long Second { get; set; }

        public long Nanosecond { get; set; }

        public long Weekday { get; set; }

        public long WeekOfMonth { get; set; }

        public long WeekOfYear { get; set; }

        #endregion

        #region Operator methods

        public static implicit operator UnityDateComponents(DateComponents dateComponents)
        {
            return new UnityDateComponents()
            {
                Calendar        = dateComponents.Calendar,
                Year            = dateComponents.Year,
                Month           = dateComponents.Month,
                Day             = dateComponents.Day,
                Hour            = dateComponents.Hour,
                Minute          = dateComponents.Minute,
                Second          = dateComponents.Second,
                Nanosecond      = dateComponents.Nanosecond,
                Weekday         = dateComponents.Weekday,
                WeekOfMonth     = dateComponents.WeekOfMonth,
                WeekOfYear      = dateComponents.WeekOfYear,
            };
        }

        public static implicit operator DateComponents(UnityDateComponents dateComponents)
        {
            return new DateComponents()
            {
                Calendar        = dateComponents.Calendar,
                Year            = (int)dateComponents.Year,
                Month           = (int)dateComponents.Month,
                Day             = (int)dateComponents.Day,
                Hour            = (int)dateComponents.Hour,
                Minute          = (int)dateComponents.Minute,
                Second          = (int)dateComponents.Second,
                Nanosecond      = (int)dateComponents.Nanosecond,
                Weekday         = (int)dateComponents.Weekday,
                WeekOfMonth     = (int)dateComponents.WeekOfMonth,
                WeekOfYear      = (int)dateComponents.WeekOfYear,
            };
        }

        #endregion

    }
}