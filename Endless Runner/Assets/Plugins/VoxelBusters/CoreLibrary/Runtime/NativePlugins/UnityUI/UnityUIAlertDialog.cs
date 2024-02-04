using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.CoreLibrary.NativePlugins.UnityUI
{
    public abstract class UnityUIAlertDialog : MonoBehaviour, IUnityUIAlertDialog
    {
        #region Fields

        private     List<AlertAction>   m_actionButtons     = new List<AlertAction>();

        private     Callback<int>       m_callback;

        #endregion

        #region Properties

        public bool IsShowing { get; private set; }

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

        #region IUnityUIAlertDialog implementation

        public string Title
        {
            get;
            set;
        }

        public string Message
        {
            get;
            set;
        }

        public void AddActionButton(string title)
        {
            m_actionButtons.Add(new AlertAction(title));
        }

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
            // update visibility status
            IsShowing   = false; 

            // destroy object
            gameObject.SetActive(false);
            Destroy(gameObject);
        }

        public void SetCompletionCallback(Callback<int> callback)
        {
            m_callback  = callback;
        }

        #endregion

        #region Properties

        protected int GetActionButtonCount()
        {
            return m_actionButtons.Count;
        }

        protected AlertAction GetActionButtonAtIndex(int index)
        {
            return m_actionButtons[index];
        }

        protected void SendCompletionResult(int selectedButtonIndex)
        {
            CallbackDispatcher.InvokeOnMainThread(m_callback, selectedButtonIndex);
        }

        #endregion

        #region Nested types

        protected class AlertAction
        {
            public string Title { get; private set; }

            public AlertAction(string title)
            {
                // set properties
                Title       = title;
            }
        }

        #endregion
    }
}