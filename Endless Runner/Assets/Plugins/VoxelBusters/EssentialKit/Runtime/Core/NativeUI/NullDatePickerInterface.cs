using System;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins;
using UnityObject = UnityEngine.Object;

namespace VoxelBusters.EssentialKit.NativeUICore
{
    public class NullDatePickerInterface : NativeDatePickerInterfaceBase
    {
        #region Fields

        private DateTime? m_minDate;

        private DateTime? m_maxDate;

        private DateTime? m_initialDate;

        private DateTimeKind m_kind;

        #endregion

        #region Constructors

        public NullDatePickerInterface(DatePickerMode mode)
            : base(mode)
        {
            // set properties
            m_initialDate   = null;
            m_maxDate       = null;
            m_kind          = DateTimeKind.Utc;
        }

        ~NullDatePickerInterface()
        {
            Dispose(false);
        }

        #endregion

        #region Private static methods

        private static void LogNotSupported()
        {
            Diagnostics.LogNotSupported("Date Picker");
        }

        #endregion

        #region Base class implementation

        public override void SetKind(DateTimeKind value)
        {
            m_kind = value;
        }

        public override void SetMinimumDate(DateTime? value)
        {
            m_minDate = value;
        }

        public override void SetMaximumDate(DateTime? value)
        {
            m_maxDate = value;
        }

        public override void SetInitialDate(DateTime? value)
        {
            m_initialDate = value;
        }

        public override void Show()
        {
            LogNotSupported();
            SendCloseEvent(null, new Error("Feature not supported on this platform"));
        }

        protected override void Dispose(bool disposing)
        {
            // check whether object is released
            if (IsDisposed)
            {
                return;
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}