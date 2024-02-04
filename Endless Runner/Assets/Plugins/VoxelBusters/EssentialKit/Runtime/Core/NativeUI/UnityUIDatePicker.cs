using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.EssentialKit.NativeUICore
{
    public abstract class UnityUIDatePicker : MonoBehaviour, IUnityUIDatePicker
    {
        #region Fields

        private     EventCallback<DateTime?>    m_callback;

        #endregion

        #region Properties

        public bool IsShowing { get; private set; }

        #endregion

        #region Static methods

        protected DateTime GetCurrentDateTime(DateTimeKind kind)
        {
            return ((kind == DateTimeKind.Local) ? DateTime.Now : DateTime.UtcNow);
        }

        #endregion

        #region Unity methods

        protected virtual void Awake()
        { }

        protected virtual void Start()
        {
            if (!IsShowing)
            {
                gameObject.SetActive(false);
            }
        }

        #endregion

        #region Public methods

        public DatePickerMode Mode { get; set; }

        public DateTime? MinDate { get; set; }

        public DateTime? MaxDate { get; set; }

        public DateTime? InitialDate { get; set; }

        public DateTimeKind Kind { get; set; }

        public DateTime SelectedDate { get; set; }
        
        public virtual void Show()
        { 
            // update visibility status
            IsShowing   = true;

            // update object state
            if (!gameObject.activeSelf)
            {
                gameObject.SetActive(true);
            }
        }

        public virtual void Dismiss()
        {
            // send result
            SendCompletionResult(null, new Error("User cancelled."));

            DismissInternal();
        }

        public void SetCompletionCallback(EventCallback<DateTime?> callback)
        {
            // store reference
            m_callback  = callback;
        }

        #endregion

        #region Private methods

        protected void DismissInternal()
        {
            // update visibility status
            IsShowing   = false; 

            // destroy object
            gameObject.SetActive(false);
            Destroy(gameObject);
        }

        protected void SendCompletionResult(DateTime? result, Error error)
        {
            CallbackDispatcher.InvokeOnMainThread(m_callback, result, error);
        }

        #endregion
    }
}