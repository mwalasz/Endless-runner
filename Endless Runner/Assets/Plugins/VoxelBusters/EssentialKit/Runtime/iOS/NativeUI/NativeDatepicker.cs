#if UNITY_IOS || UNITY_TVOS
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using AOT;
using VoxelBusters.CoreLibrary.NativePlugins;
using VoxelBusters.CoreLibrary.NativePlugins.iOS;
using VoxelBusters.CoreLibrary;
using System.Runtime.InteropServices;

namespace VoxelBusters.EssentialKit.NativeUICore.iOS
{
    internal sealed class NativeDatePicker : NativeDatePickerInterfaceBase, INativeDatePickerInterface
    {
        #region Fields

        private DateTimeKind    m_kind;
        private DateTime?       m_minimumDate;
        private DateTime?       m_maximumDate;
        private DateTime?       m_initialDate;


        #endregion

        #region Constructors

        static NativeDatePicker()
        {
            // initialise component
            DatePickerControllerBinding.NPDatePickerControllerRegisterCallback(callback: HandleDatePickerControllerCallbackInternal);
        }

        public NativeDatePicker(DatePickerMode mode) : base(mode)
        {
        }

        ~NativeDatePicker()
        {
            Dispose(false);
        }

        #endregion

        #region Base class methods

        public override void SetKind(DateTimeKind value)
        {
            m_kind = value;
        }

        public override void SetMinimumDate(DateTime? value)
        {
            m_minimumDate = value;
        }

        public override void SetMaximumDate(DateTime? value)
        {
            m_maximumDate = value;
        }

        public override void SetInitialDate(DateTime? value)
        {
            m_initialDate = value;
        }

        public override void Show()
        {
            long initialEpoch = GetEpochInSeconds(m_initialDate, DateTimeOffset.Now.ToUnixTimeSeconds());
            long minimumEpoch = GetEpochInSeconds(m_minimumDate);
            long maximumEpoch = GetEpochInSeconds(m_maximumDate);

            DatePickerClosedInternalCallback callback = (DateTime? selectedDate, Error error) =>
            {
                SendCloseEvent(selectedDate, error);
            };

            IntPtr tagPtr = MarshalUtility.GetIntPtr(callback);
            DatePickerControllerBinding.NPDatePickerControllerShow(DatePickerUtility.ConvertToUIDatePickerMode(Mode), initialEpoch, minimumEpoch, maximumEpoch, tagPtr);
        }

        protected override void Dispose(bool disposing)
        {
            // check whether object is released
            if (IsDisposed)
            {
                return;
            }

            // release all unmanaged type objects
            var     nativePtr   = AddrOfNativeObject();
            if (nativePtr != IntPtr.Zero)
            {
                NativeInstanceMap.RemoveInstance(nativePtr);
            }

            base.Dispose(disposing);
        }

        #endregion

        #region Native callback methods

        [MonoPInvokeCallback(typeof(DatePickerControllerNativeCallback))]
        private static void HandleDatePickerControllerCallbackInternal(long selectedValue, IntPtr tagPtr)
        {
            var tagHandle = GCHandle.FromIntPtr(tagPtr);
            try
            {
                DateTime? selectedTime = (selectedValue == -1) ? null : (DateTime?)DateTimeOffset.FromUnixTimeSeconds(selectedValue).DateTime;
                ((DatePickerClosedInternalCallback)(tagHandle.Target))(selectedTime, null);
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(EssentialKitDomain.Default, exception);
            }
            finally
            {
                // release handle
                tagHandle.Free();
            }
        }

        #endregion

        #region Utility methods

        private long GetEpochInSeconds(DateTime? dateTime, long defualtValue = -1)
        {
            if (dateTime == null)
                return defualtValue;

            TimeSpan timeSpan = dateTime.Value - new DateTime(1970, 1, 1);
            long epoch = (int)timeSpan.TotalSeconds;
            return epoch;
        }

        #endregion
    }
}
#endif