using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.EssentialKit.NativeUICore;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit
{
    public sealed class DatePicker : NativeFeatureBehaviour
    {
        #region Fields

        private     INativeDatePickerInterface                  m_nativeInterface       = null;

        private     DateTimeKind                                m_kind;

        private     DateTime?                                   m_minDate;

        private     DateTime?                                   m_maxDate;

        private     DateTime?                                   m_initialDate;

        private     DateTime?                                   m_selectedDate;

        #endregion

        #region Properties

        public DateTime? SelectedDate { get { return m_selectedDate; } }

        #endregion

        #region Delegates

        public delegate void ValueChangeCallback(DateTime? date);

        #endregion

        #region Events

        public ValueChangeCallback OnValueChange = delegate { };

        public Callback<DatePickerResult> OnCloseCallback = delegate { };
        
        #endregion

        #region Static methods

        /// <summary>
        /// Creates a new instance of the <see cref="DatePicker"/> class.
        /// </summary>
        /// <param name="mode">The picker mode to be used.</param>
        public static DatePicker CreateInstance(DatePickerMode mode = DatePickerMode.DateAndTime)
        {
            return CreateInstanceInternal<DatePicker>("DatePicker", mode);
        }

        #endregion

        #region Lifecycle methods

        protected override void AwakeInternal(object[] args)
        {
            base.AwakeInternal(args);

            // configure component
            var     mode        = (args == null) ? DatePickerMode.DateAndTime : (DatePickerMode)args[0];
            m_nativeInterface   = NativeUI.NativeInterface.CreateDatePicker(mode);

            // set default properties
            SetKind(DateTimeKind.Local);
            OnCloseCallback     = null;

            // register for events
            m_nativeInterface.OnClose  += HandleCloseInternalCallback;
        }

        protected override void DestroyInternal()
        {
            base.DestroyInternal();

            // unregister from event
            m_nativeInterface.OnClose  -= HandleCloseInternalCallback;
            
            // reset interface properties
            m_nativeInterface.Dispose();
        }

        #endregion

        #region Behaviour methods

        public override bool IsAvailable()
        {
            return NativeUI.NativeInterface.IsAvailable;
        }

        protected override string GetFeatureName()
        {
            return "Date Picker";
        }

        #endregion

        #region Getter methods

        public DateTimeKind GetKind()
        {
            return m_kind;
        }
            
        public DatePickerMode GetMode()
        {
            return m_nativeInterface.Mode;
        }

        public DateTime? GetMinimumDate()
        {
            return m_minDate;
        }

        public DateTime? GetMaximumDate()
        {
            return m_maxDate;
        }

        public DateTime? GetInitialDate()
        {
            return m_initialDate;
        }

        #endregion

        #region Setter methods

        public DatePicker SetKind(DateTimeKind value)
        {
            // cache local value
            m_kind  = value;

            try
            {
                // update native interface
                m_nativeInterface.SetKind(value);

                // reset all values to match specified format
                if (m_minDate != null)
                {
                    SetMinimumDate(m_minDate);
                }
                if (m_initialDate != null)
                {
                    SetInitialDate(m_initialDate);
                }
                if (m_maxDate != null)
                {
                    SetMaximumDate(m_maxDate);
                }
                if (m_selectedDate != null)
                {
                    SetSelectedDate(m_selectedDate);
                }
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(EssentialKitDomain.Default, exception);
            }

            return this;
        }

        public DatePicker SetMinimumDate(DateTime? value)
        {
            // save value
            m_minDate   = ChangeDateTimeToSuitableFormat(value);

            try
            {
                // update native interface
                m_nativeInterface.SetMinimumDate(m_minDate);

                // reset dependent fields
                SetInitialDate(m_initialDate);
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(EssentialKitDomain.Default, exception);
            }
        
            return this;
        }

        public DatePicker SetMaximumDate(DateTime? value)
        {
            // save value
            m_maxDate   = ChangeDateTimeToSuitableFormat(value);

            try
            {
                // update native interface
                m_nativeInterface.SetMaximumDate(m_maxDate);
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(EssentialKitDomain.Default, exception);
            }
        
            return this;
        }

        public DatePicker SetInitialDate(DateTime? value)
        {
            // save value
            m_initialDate       = ChangeDateTimeToSuitableFormat(value);
            if (m_minDate != null)
            {
                if ((m_initialDate == null) || (m_initialDate < m_minDate))
                {
                    m_initialDate   = m_minDate;
                }
            }

            try
            {
                // update native interface
                m_nativeInterface.SetInitialDate(m_initialDate);
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(EssentialKitDomain.Default, exception);
            }

            // store modified value
            if (m_selectedDate == null)
            {
                SetSelectedDate(m_initialDate);
            }
        
            return this;
        }

        private void SetSelectedDate(DateTime? value)
        {
            // update local copy
            if (value != null)
            {
                m_selectedDate  = ChangeDateTimeToSuitableFormat(value);
                OnValueChange(value.Value);
            }
        }

        public DatePicker SetOnValueChange(ValueChangeCallback callback)
        {
            // validate arguments
            if (null == callback)
            {
                DebugLogger.LogWarning(EssentialKitDomain.Default, "Callback is null.");
                return this;
            }

            // save callback reference
            OnValueChange   = callback;
        
            return this;
        }

        public DatePicker SetOnCloseCallback(Callback<DatePickerResult> callback)
        {
            // validate arguments
            if (null == callback)
            {
                DebugLogger.LogWarning(EssentialKitDomain.Default, "Callback is null.");
                return this;
            }

            // save callback reference
            OnCloseCallback = callback;
        
            return this;
        }

        public void Show()
        {
            try
            {
                // present view
                m_nativeInterface.Show();
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(EssentialKitDomain.Default, exception);
            }
        }

        #endregion

        #region Private methods

        private void RegisterForEvents()
        {
            m_nativeInterface.OnClose   += HandleCloseInternalCallback;
        }

        private void UnregisterFromEvents()
        {
            m_nativeInterface.OnClose   -= HandleCloseInternalCallback;
        }

        // boundary case to ensure that all values are in same format
        private DateTime? ChangeDateTimeToSuitableFormat(DateTime? dateTime)
        {
            if (dateTime != null)
            {
                var     value   = dateTime.Value;
                if (value.Kind != m_kind)
                {
                    if (DateTimeKind.Local == m_kind)
                    {
                        return value.ToLocalTime();
                    }
                    else if (DateTimeKind.Utc == m_kind)
                    {
                        return value.ToUniversalTime();
                    }
                }
            }

            return dateTime;
        }

        #endregion

        #region Event callback methods

        private void HandleCloseInternalCallback(DateTime? selectedDate, Error error)
        {
            SetSelectedDate(selectedDate);

            // send result to caller object
            var     result      = new DatePickerResult(m_selectedDate);
            CallbackDispatcher.InvokeOnMainThread(OnCloseCallback, result);

            // release
            Destroy(gameObject);
        }

        #endregion
    }
}