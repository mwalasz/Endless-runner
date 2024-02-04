#if UNITY_ANDROID
using System;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins.Android;
using VoxelBusters.EssentialKit.NativeUICore.Android;

namespace VoxelBusters.EssentialKit.NativeUICore.Android
{
    internal sealed class DateTimePicker : NativeDatePickerInterfaceBase, INativeDatePickerInterface
    {
        #region Fields

        private NativeDatePicker m_datePicker;
        private NativeTimePicker m_timePicker;

        private DateTime m_initialDate;
        private DateTimeKind m_kind;
        private DatePickerMode m_mode;

        #endregion

        #region Constructors

        public DateTimePicker(DatePickerMode mode) : base(mode)
        {
            m_mode = mode;
            NativeContext context = NativeUnityPluginUtility.GetContext();

            if(mode == DatePickerMode.Date || mode == DatePickerMode.DateAndTime)
            {
                m_datePicker = new NativeDatePicker(context);
            }

            if (mode == DatePickerMode.Time || mode == DatePickerMode.DateAndTime)
            {
                m_timePicker = new NativeTimePicker(context);
            }
        }

        ~DateTimePicker()
        {
            Dispose(false);
        }

        #endregion

        #region Base class methods

        public override void SetInitialDate(DateTime? value)
        {
            if (value != null)
            {
                DateTime dateTime = value.GetValueOrDefault();
                m_initialDate = dateTime;
                if (m_datePicker != null)
                {
                    m_datePicker.SetValue(dateTime.Year, dateTime.Month - 1, dateTime.Day);
                }

                if (m_timePicker != null)
                {
                    m_timePicker.SetValue(dateTime.Hour, dateTime.Minute);
                }
            }
        }

        public override void SetKind(DateTimeKind value)
        {
            m_kind = value;
            if(m_kind != DateTimeKind.Local)
            {
                DebugLogger.LogWarning("On Android always uses Local(epoch)");
            }
            
        }

        public override void SetMaximumDate(DateTime? value)
        {
            if (value != null && m_datePicker != null)
            {
                NativeDate nativeMinDate = new NativeDate();
                nativeMinDate.SetDateTime(value.GetValueOrDefault());
                m_datePicker.SetMaxValue(nativeMinDate);
            }

            if(m_timePicker != null)
            {
                DebugLogger.LogWarning("Not supported on Android");
            }
        }

        public override void SetMinimumDate(DateTime? value)
        {
            if (value != null && m_datePicker != null)
            {
                NativeDate nativeMinDate = new NativeDate();
                nativeMinDate.SetDateTime(value.GetValueOrDefault());
                m_datePicker.SetMinValue(nativeMinDate);
            }

            if (m_timePicker != null)
            {
                DebugLogger.LogWarning("Not supported on Android");
            }
        }

        public override void Show()
        {
            if (m_mode == DatePickerMode.Date || m_mode == DatePickerMode.DateAndTime)
            {
                m_datePicker.SetListener(new NativeDatePickerListener()
                {
                    onSuccessCallback = (int year, int month, int dayOfMonth) =>
                    {
                        if (m_mode == DatePickerMode.Date)
                        {
                            SendCloseEvent(new DateTime(year, month + 1, dayOfMonth), null);
                        }
                        else
                        {
                            m_timePicker.SetListener(new NativeTimePickerListener()
                            {
                                onSuccessCallback = (int hourOfDay, int minutes) =>
                                {
                                    DebugLogger.Log("Hour of day : " + hourOfDay + " Minute : " + minutes);
                                    DateTime dateTime = (m_initialDate != default(DateTime)) ? m_initialDate : DateTime.Now;
                                    SendCloseEvent(new DateTime(year, month, dayOfMonth, hourOfDay, minutes, 0, dateTime.Kind), null);
                                }

                            });
                            m_timePicker.Show();
                        }
                    },
                    onCancelCallback = () =>
                    {
                        SendCloseEvent(null, null);
                    }
                });
                m_datePicker.Show();
            }
            else
            {
                m_timePicker.SetListener(new NativeTimePickerListener()
                {
                    onSuccessCallback = (int hourOfDay, int minutes) =>
                    {
                        DebugLogger.Log("Hour of day : " + hourOfDay + " Minute : " + minutes);
                        DateTime dateTime = (m_initialDate != default(DateTime)) ? m_initialDate : DateTime.Now;
                        SendCloseEvent(new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, hourOfDay, minutes, 0, dateTime.Kind), null);
                    }

                });
                m_timePicker.Show();
            }
        }

        #endregion
    }
}
#endif