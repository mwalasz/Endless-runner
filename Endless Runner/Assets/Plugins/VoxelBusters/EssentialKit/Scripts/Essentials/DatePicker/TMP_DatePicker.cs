using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using VoxelBusters.EssentialKit.NativeUICore;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.EssentialKit.Extensions
{
    public class TMP_DatePicker : UnityUIDatePicker
    {
        #region Fields

        [SerializeField]
        private     RectTransform           m_dateNode          = null;

        [SerializeField]
        private     TMP_Dropdown            m_dayDropdown       = null;

        [SerializeField]
        private     TMP_Dropdown            m_monthDropdown     = null;

        [SerializeField]
        private     TMP_Dropdown            m_yearDropdown      = null;

        [SerializeField]
        private     RectTransform           m_timeNode          = null;

        [SerializeField]
        private     TMP_Dropdown            m_hourDropdown      = null;

        [SerializeField]
        private     TMP_Dropdown            m_minuteDropdown    = null;

        #endregion

        #region Private static methods

        private static int GetDropdownValue(TMP_Dropdown dropdown)
        {
            var     option  = dropdown.options[dropdown.value];
            return int.Parse(option.text);
        }

        private static void SelectDropdownValue(TMP_Dropdown dropdown, int value)
        {
            var     option  = dropdown.options[0];
            int     start   = int.Parse(option.text);

            // set offset from starting value
            dropdown.value  = (value - start);
        }

        private static List<string> ConvertIntegerToStringNames(int startIndex, int count, string format)
        {
            int     end     = startIndex + count;
            var     values  = new List<string>(count);
            for (int iter = startIndex; iter < end; iter++)
            {
                values.Add(iter.ToString(format));
            }

            return values;
        }

        #endregion

        #region Base class methods

        protected override void Awake()
        { 
            base.Awake();
        }

        protected override void Start()
        {
            base.Start();
        }

        public override void Show()
        {
            base.Show();

            // set initial selected date
            SelectedDate    = InitialDate ?? GetCurrentDateTime(Kind);

            // configure components
            ConfigureYearDropdown();
            ConfigureMonthDropdown();
            ConfigureDayDropdown(SelectedDate.Month, SelectedDate.Year);
            ConfigureHourDropdown();
            ConfigureMinuteDropdown();

            // update visiblity
            m_timeNode.gameObject.SetActive(Mode != DatePickerMode.Date);
            m_dateNode.gameObject.SetActive(Mode != DatePickerMode.Time);

            // show selected date
            if (Mode != DatePickerMode.Time)
            {
                SelectDropdownValue(m_yearDropdown,     SelectedDate.Year);
                SelectDropdownValue(m_monthDropdown,    SelectedDate.Month);
                SelectDropdownValue(m_dayDropdown,      SelectedDate.Day);
            }
            if (Mode != DatePickerMode.Date)
            {
                SelectDropdownValue(m_hourDropdown,     SelectedDate.Hour);
                SelectDropdownValue(m_minuteDropdown,   SelectedDate.Minute);
            }
        }

        #endregion

        #region Private methods

        private void ConfigureYearDropdown()
        {
            int startYear   = 1900, endYear = 2100;
            if (MinDate.HasValue)
            {
                startYear   = MinDate.Value.Year;
            }
            if (MaxDate.HasValue)
            {
                endYear     = MaxDate.Value.Year;
            }

            // configure components
            var     years   = ConvertIntegerToStringNames(startYear, endYear - startYear, "D4");
            m_yearDropdown.ClearOptions();
            m_yearDropdown.AddOptions(years);
        }

        private void ConfigureMonthDropdown()
        {
            // configure components
            var     months  = ConvertIntegerToStringNames(1, 12, "D2");
            m_monthDropdown.ClearOptions();
            m_monthDropdown.AddOptions(months);
        }

        private void ConfigureDayDropdown(int month, int year)
        {
            // get all the days
            var    days     = new List<string>();
            for (var date = new DateTime(year, month, 1); date.Month == month; date = date.AddDays(1))
            {
                days.Add(date.Day.ToString("D2"));       
            }

            m_dayDropdown.ClearOptions();
            m_dayDropdown.AddOptions(days);
        }

        private void ConfigureHourDropdown()
        {
            var     hours   = ConvertIntegerToStringNames(0, 24, "D2");
            m_hourDropdown.ClearOptions();
            m_hourDropdown.AddOptions(hours);
        }

        private void ConfigureMinuteDropdown()
        {
            var     minute  = ConvertIntegerToStringNames(0, 60, "D2");
            m_minuteDropdown.ClearOptions();
            m_minuteDropdown.AddOptions(minute);
        }

        #endregion

        #region UI callback methods

        public void OnSubmit()
        {
            // construct date 
            int     year    = 0; 
            int     month   = 1; 
            int     day     = 1; 
            int     hour    = 0;
            int     minute  = 0;
            if (Mode != DatePickerMode.Time)
            {
                year    = GetDropdownValue(m_yearDropdown);
                month   = GetDropdownValue(m_monthDropdown);
                day     = GetDropdownValue(m_dayDropdown);
            }
            if (Mode != DatePickerMode.Date)
            {
                hour    = GetDropdownValue(m_hourDropdown);
                minute  = GetDropdownValue(m_minuteDropdown);
            }
            // send result
            var     date    = new DateTime(year, month, day, hour, minute, 0, Kind);
            SendCompletionResult(date, null);

            // remove view
            DismissInternal();
        }

        public void OnDismiss()
        {
            Dismiss();
        }

        public void OnDropdownValueChange(Dropdown dropdown)
        {
            // reconfigure day dropdown on year/month selection change
            if (dropdown == m_yearDropdown ||
                dropdown == m_monthDropdown)
            {
                ConfigureDayDropdown(GetDropdownValue(m_monthDropdown), GetDropdownValue(m_yearDropdown));
            }
        }

        #endregion
    }
}