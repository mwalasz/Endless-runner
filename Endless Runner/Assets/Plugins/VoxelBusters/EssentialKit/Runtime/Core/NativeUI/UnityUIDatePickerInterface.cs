using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;

using UnityObject = UnityEngine.Object;

namespace VoxelBusters.EssentialKit.NativeUICore
{
    public class UnityUIDatePickerInterface : NativeDatePickerInterfaceBase
    {
        #region Fields

        private     DateTime?           m_minDate;

        private     DateTime?           m_maxDate;

        private     DateTime?           m_initialDate;

        private     DateTimeKind        m_kind;

        private     UnityUIDatePicker   m_datePicker;
        
        private     RectTransform       m_parent;

        private     UnityUIDatePicker   m_datePickerPrefab;

        #endregion

        #region Constructors

        public UnityUIDatePickerInterface(DatePickerMode mode, UnityUIDatePicker datePickerPrefab, RectTransform parent)
            : base(mode)
        {
            // check arguments
            Assert.IsArgNotNull(datePickerPrefab, "datePickerPrefab");
            Assert.IsArgNotNull(parent, "parent");

            // set properties
            m_initialDate       = null;
            m_maxDate           = null;
            m_kind              = DateTimeKind.Local;
            m_datePickerPrefab  = datePickerPrefab;
            m_parent            = parent;
        }

        ~UnityUIDatePickerInterface()
        {
            Dispose(false);
        }

        #endregion

        #region Base class implementation

        public override void SetKind(DateTimeKind value)
        {
            m_kind          = value;
        }

        public override void SetMinimumDate(DateTime? value)
        { 
            m_minDate       = value;

            // set default initial date
            if (value != null)
            {
                if ((m_initialDate == null) || (m_initialDate < m_minDate))
                {
                    m_initialDate = (m_minDate != null) ? m_minDate : DateTime.Now;
                }
            }
        }

        public override void SetMaximumDate(DateTime? value)
        { 
            m_maxDate       = value;
        }

        public override void SetInitialDate(DateTime? value)
        { 
            m_initialDate   = value ?? DateTime.Now;
        }

        public override void Show()
        {
            // check whether the dialog is already presented
            if (m_datePicker != null)
            {
                return;
            }

            // create object using prefab
            m_datePicker                = UnityObject.Instantiate(m_datePickerPrefab, m_parent, false);
            m_datePicker.Mode           = Mode;
            m_datePicker.MinDate        = m_minDate;
            m_datePicker.MaxDate        = m_maxDate;
            m_datePicker.InitialDate    = m_initialDate;
            m_datePicker.Kind           = m_kind;
            m_datePicker.SetCompletionCallback(HandleCompletionCallback);
            m_datePicker.Show();
        }

        protected override void Dispose(bool disposing)
        {
            // check whether object is released
            if (IsDisposed) 
            {
                return;
            }

            if (disposing)
            {
                // destroy gameobject
                if (m_datePicker != null)
                {
                    UnityObject.Destroy(m_datePicker.gameObject);
                }
            }

            base.Dispose(disposing);
        }

        #endregion

        #region Private methods

        private void HandleCompletionCallback(DateTime? result, Error error)
        {
            SendCloseEvent(result, error);
        }

        #endregion
    }
}