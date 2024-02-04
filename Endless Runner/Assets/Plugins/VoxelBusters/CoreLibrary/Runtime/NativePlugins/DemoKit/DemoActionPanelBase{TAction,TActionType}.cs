using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.CoreLibrary.NativePlugins.DemoKit
{
    public abstract class DemoActionPanelBase<TAction, TActionType> : DemoPanel where TAction : DemoActionBehaviour<TActionType> where TActionType : struct, System.IConvertible
    {
        #region Constants

        private     const   string      kLogCreateInstance = "Create instance by calling {0})";

        #endregion

        #region Fields

        private     ConsoleRect         m_consoleRect       = null;

        private     TAction[]           m_actions	        = null;

        #endregion

        #region Unity methods

        protected virtual void Awake()
        {
            // set properties
            m_consoleRect   = GetComponentInChildren<ConsoleRect>();
            m_actions       = GetComponentsInChildren<TAction>();

            // init
            SetActionCallbacks();
        }

        protected virtual void Start() 
        { }

        protected virtual void OnEnable()
        { }

        protected virtual void OnDisable()
        { }

        #endregion

        #region Base methods

        public override void Rebuild()
        {
            SetActionCallbacks();
        }

        #endregion

        #region Private methods

        protected virtual string GetCreateInstanceCodeSnippet()
        {
            throw VBException.NotImplemented();
        }

        protected TAction FindActionOfType(TActionType actionType)
        {
            return Array.Find(m_actions, (item) => EqualityComparer<TActionType>.Default.Equals(actionType, item.ActionType));
        }

        protected void Log(string message, bool append = true)
        {
            m_consoleRect.Log(message, append);
        }

        protected void LogMissingInstance(bool append = true)
        {
            m_consoleRect.Log(string.Format(kLogCreateInstance, GetCreateInstanceCodeSnippet()), append);
        }

        protected bool AssertPropertyIsValid(string property, string value)
        {
            return AssertPropertyIsValid(property, () => string.IsNullOrEmpty(value));
        }

        protected bool AssertPropertyIsValid(string property, Func<bool> condition)
        {
            if (condition())
            {
                m_consoleRect.Log($"Property \"{property}\" value is invalid.", append: true);
                return false;
            }
            return true;
        }

        private void SetActionCallbacks()
        {
            // set button property
            foreach (TAction actionButton in m_actions)
            {
                var selectEvent = actionButton.OnSelect;
                selectEvent.RemoveAllListeners();
                selectEvent.AddListener(OnActionSelect);
            }
        }

        #endregion

        #region UI callback methods

        public void OnActionSelect(Selectable selectable)
        {
            TAction     selectedAction  = Array.Find(m_actions, (item) => selectable == item.Selectable);
            if (selectedAction)
            {
                OnActionSelectInternal(selectedAction);
            }
        }

        protected virtual void OnActionSelectInternal(TAction selectedAction)
        {}

        #endregion
    }
}