using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

namespace VoxelBusters.EssentialKit.Extensions
{
    public class TMP_DatePickerInputField : Selectable, IPointerClickHandler, ISubmitHandler, ICancelHandler
    {
        #region Fields

        [SerializeField]
        private         TextMeshProUGUI         m_placeholder       = null;
        
        [SerializeField]
        private         TextMeshProUGUI         m_text              = null;

        [SerializeField]
        private         DateTimeKind            m_kind              = DateTimeKind.Local;

        [SerializeField]
        private         DatePickerMode          m_mode              = DatePickerMode.DateAndTime;

        [SerializeField]
        private         string                  m_displayFormat     = "dd MMM yyyy hh:mm tt";
                
        [SerializeField]
        private         UnityEvent              m_onValueChange     = new UnityEvent();

        private         DateTime?               m_minimumDate;

        private         DateTime?               m_maximumDate;

        private         DateTime?               m_initialDate;
        
        private         DateTime?               m_date;

        private         bool                    m_isSelected;
        
        #endregion

        #region Properties

        public DateTimeKind Kind
        {
            get
            {
                return m_kind;
            }
            set
            {
                m_kind  = value;
            }
        }

        public DatePickerMode Mode
        {
            get
            {
                return m_mode;
            }
            set
            {
                m_mode  = value;
            }
        }

        public string DisplayFormat
        {
            get
            {
                return m_displayFormat;
            }
            set
            {
                m_displayFormat = value;
            }
        }

        public DateTime? MinimumDate
        {
            get
            { 
                return m_minimumDate;
            }
            set
            {
                m_minimumDate   = value;
            }
        }

        public DateTime? MaximumDate
        {
            get
            { 
                return m_maximumDate;
            }
            set
            {
                m_maximumDate   = value;
            }
        }

        public DateTime? InitialDate
        {
            get
            { 
                return m_initialDate;
            }
            set
            {
                m_initialDate   = value;
            }
        }

        public DateTime? Date
        {
            get
            {
                return m_date;
            }
            set
            {
                if ((value == null) ||
                    ((MinimumDate == null || value >= MinimumDate) && (MaximumDate == null || value <= MaximumDate)))
                {
                    // copy new value
                    m_date      = value;

                    // update content
                    SetText(m_date.HasValue ? m_date.Value.ToString(m_displayFormat) : string.Empty);

                    // send event
                    if (m_onValueChange != null)
                    {
                        m_onValueChange.Invoke();
                    }
                }
            }
        }

        #endregion

        #region Events

        public UnityEvent OnValueChange
        {
            get
            {
                return m_onValueChange;
            }
        }

        #endregion

        #region Unity methods

        protected override void Awake()
        {
            base.Awake();

            // set default state
            m_isSelected    = false;
            SetText(null);
        }
            
        #endregion

        #region Public methods

        public void Show()
        {
            if (!IsActive() || !IsInteractable() || m_isSelected)
                return;

            // update activity status
            m_isSelected        = true;

            // create date picker object
            var     datePicker  = DatePicker.CreateInstance(m_mode)
                //.SetKind(m_kind)
                .SetMinimumDate(MinimumDate)
                .SetMaximumDate(MaximumDate)
                .SetInitialDate(Date.HasValue ? Date : InitialDate)
                .SetOnValueChange(OnDatePickerValueChange)
                .SetOnCloseCallback(OnDatePickerClose);

            // show date picker
            datePicker.Show();
        }

        #endregion

        #region Private methods

        private void SetText(string value)
        {
            // update state based on content
            bool    isEmpty = string.IsNullOrEmpty(value);
            m_placeholder.gameObject.SetActive(isEmpty);
            m_text.gameObject.SetActive(!isEmpty);

            // update content
            if (!isEmpty)
            {
                m_text.text = value;
            }
        }

        #endregion

        #region IPointerClickHandler implementation

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            Show();
        }

        #endregion

        #region ISubmitHandler implementation

        public virtual void OnSubmit(BaseEventData eventData)
        { }

        #endregion

        #region ICancelHandler implementation

        public virtual void OnCancel(BaseEventData eventData)
        { }

        #endregion

        #region UI callback methods

        private void OnDatePickerClose(DatePickerResult data)
        {
            // unset flag
            m_isSelected    = false;
        }

        private void OnDatePickerValueChange(DateTime? dateTime)
        {
            // update local value
            Date            = dateTime;
        }

        #endregion
    }
}