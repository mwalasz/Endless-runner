using System;
using UnityEngine;

namespace VoxelBusters.CoreLibrary.NativePlugins
{
    /// <summary>
    /// A date or time specified in terms of units (such as year, month, day, hour, and minute) to be evaluated in a calendar system and time zone.
    /// </summary>
    [Serializable]
    public class DateComponents 
    {
        #region Fields

        [SerializeField]
        private     Calendar    m_calendar;

        [SerializeField]
        private     int         m_year;
        
        [SerializeField]
        private     int         m_month;
        
        [SerializeField]
        private     int         m_day;

        [SerializeField]
        private     int         m_hour;
        
        [SerializeField]
        private     int         m_minute;
        
        [SerializeField]
        private     int         m_second;
        
        [SerializeField]
        private     int         m_nanosecond;

        [SerializeField]
        private     int         m_weekday;
        
        [SerializeField]
        private     int         m_weekOfMonth;
        
        [SerializeField]
        private     int         m_weekOfYear;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the calendar.
        /// </summary>
        /// <value>The calendar.</value>
        public Calendar Calendar
        {
            get => m_calendar;
            set => m_calendar = value;
        }

        /// <summary>
        /// Gets or sets the year.
        /// </summary>
        /// <value>The year.</value>
        public int Year
        {
            get => m_year;
            set => m_year = value;
        }

        /// <summary>
        /// Gets or sets the month.
        /// </summary>
        /// <value>The month.</value>
        public int Month
        {
            get => m_month;
            set => m_month = value;
        }

        /// <summary>
        /// Gets or sets the day.
        /// </summary>
        /// <value>The day.</value>
        public int Day
        {
            get => m_day;
            set => m_day = value;
        }

        /// <summary>
        /// Gets or sets the hour.
        /// </summary>
        /// <value>The hour.</value>
        public int Hour
        {
            get => m_hour;
            set => m_hour = value;
        }

        /// <summary>
        /// Gets or sets the minute.
        /// </summary>
        /// <value>The minute.</value>
        public int Minute
        {
            get => m_minute;
            set => m_minute = value;
        }

        /// <summary>
        /// Gets or sets the second.
        /// </summary>
        /// <value>The second.</value>
        public int Second
        {
            get => m_second;
            set => m_second = value;
        }

        /// <summary>
        /// Gets or sets the nanosecond.
        /// </summary>
        /// <value>The nanosecond.</value>
        public int Nanosecond
        {
            get => m_nanosecond;
            set => m_nanosecond = value;
        }

        /// <summary>
        /// Gets or sets the weekday.
        /// </summary>
        /// <value>The weekday.</value>
        public int Weekday
        {
            get => m_weekday;
            set => m_weekday = value;
        }

        /// <summary>
        /// Gets or sets the week of month.
        /// </summary>
        /// <value>The week of month.</value>
        public int WeekOfMonth
        {
            get => m_weekOfMonth;
            set => m_weekOfMonth = value;
        }

        /// <summary>
        /// Gets or sets the week of year.
        /// </summary>
        /// <value>The week of year.</value>
        public int WeekOfYear
        {
            get => m_weekOfYear;
            set => m_weekOfYear = value;
        }

        #endregion

        #region Constructors

        public DateComponents()
        {
            // set default values
            m_calendar      = (Calendar)0;
            m_year          = 0;
            m_month         = 0;
            m_day           = 0;
            m_hour          = 0;
            m_minute        = 0;
            m_second        = 0;
            m_nanosecond    = 0;
            m_weekday       = 0;
            m_weekOfMonth   = 0;
            m_weekOfYear    = 0;
        }

        #endregion
    }
}