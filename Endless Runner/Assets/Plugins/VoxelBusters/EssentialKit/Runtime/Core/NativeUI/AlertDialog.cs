using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.EssentialKit.NativeUICore;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit
{
    /// <summary>
    /// The AlertDialog class provides an interface to display an alert message to the user.
    /// </summary>
    /// <example>
    /// The following code example shows how to configure and present an alert dialog.
    /// <code>
    /// using UnityEngine;
    /// using System.Collections;
    /// using VoxelBusters.EssentialKit;
    /// 
    /// public class ExampleClass : MonoBehaviour 
    /// {
    ///     public void Start()
    ///     {
    ///         AlertDialog newDialog   = AlertDialog.CreateInstance();
    ///         newDialog.SetTitle(title);
    ///         newDialog.SetMessage(message);
    ///         newDialog.AddButton(button, OnAlertButtonClicked);
    ///         newDialog.Show();
    ///     }
    /// 
    ///     private void OnAlertButtonClicked()
    ///     {
    ///         // add your code
    ///     }
    /// }
    /// </code>
    /// </example>
    public partial class AlertDialog : NativeFeatureBehaviour
    {
        #region Fields

        private     INativeAlertDialogInterface     m_nativeDialog      = null;

        private     List<ButtonAction>              m_buttonActions;

        #endregion

        #region Properties

        /// <summary>
        /// The title of the alert.
        /// </summary>
        /// <value>The title of the alert.</value>
        public string Title
        {
            get
            {
                try
                {
                    return m_nativeDialog.GetTitle();
                }
                catch (Exception exception)
                {
                    DebugLogger.LogException(EssentialKitDomain.Default, exception);
                    return null;
                }
            }
            set
            {
                SetTitleInternal(value);
            }
        }

        /// <summary>
        /// The message of the alert.
        /// </summary>
        /// <value>The message of the alert.</value>
        public string Message
        {
            get
            {
                try
                {
                    return m_nativeDialog.GetMessage();
                }
                catch (Exception exception)
                {
                    DebugLogger.LogException(EssentialKitDomain.Default, exception);
                    return null;
                }
            }
            set
            {
                SetMessageInternal(value);
            }
        }

        #endregion

        #region Static methods

        /// <summary>
        /// Creates a new instance of the <see cref="AlertDialog"/> class.
        /// </summary>
        /// <param name="alertStyle">The alert style to be used.</param>
        public static AlertDialog CreateInstance(AlertDialogStyle alertStyle = AlertDialogStyle.Default)
        {
            return CreateInstanceInternal<AlertDialog>("AlertDialog", alertStyle);
        }

        #endregion

        #region Lifecycle methods

        protected override void AwakeInternal(object[] args)
        {
            base.AwakeInternal(args);

            // initialise properties
            var     nativeUIInterface   = NativeUI.NativeInterface;
            var     alertStyle          = (args == null) ? AlertDialogStyle.Default : (AlertDialogStyle)args[0];
            m_nativeDialog              = nativeUIInterface.CreateAlertDialog(alertStyle);
            m_buttonActions             = new List<ButtonAction>(capacity: 3);

            // register for events
            m_nativeDialog.OnButtonClick    += HandleButtonClickInternalCallback;
        }

        protected override void DestroyInternal()
        {
            base.DestroyInternal();

            if (m_nativeDialog != null)
            {
                // unregister from event
                m_nativeDialog.OnButtonClick    -= HandleButtonClickInternalCallback;
            
                // reset interface properties
                m_nativeDialog.Dispose();
            }
            if (m_buttonActions != null)
            {
                m_buttonActions.Clear();
            }
        }

        #endregion

        #region Behaviour methods

        public override bool IsAvailable()
        {
            return NativeUI.NativeInterface.IsAvailable;
        }

        protected override string GetFeatureName()
        {
            return "Alert Dialog";
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Adds an action button to the alert. Here, the default style is used.
        /// </summary>
        /// <param name="title">The title of the button.</param>
        /// <param name="callback">The method to execute when the user selects this button.</param>
        public void AddButton(string title, Callback callback)
        {
            // validate arguments
            if (null == title)
            {
                DebugLogger.LogWarning(EssentialKitDomain.Default, "Button title is null.");
                return;
            }

            // create button object and add it to the dialog
            m_buttonActions.Add(new ButtonAction() { title = title, onClick = callback });

            try
            {
                m_nativeDialog.AddButton(title, false);
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(EssentialKitDomain.Default, exception);
            }
        }

        /// <summary>
        /// Adds action button to the alert. This style type indicates the action cancels the operation and leaves things unchanged.
        /// </summary>
        /// <param name="title">The title of the button.</param>
        /// <param name="callback">The method to execute when the user selects this button.</param>
        public void AddCancelButton(string title, Callback callback)
        {
            if (null == title)
            {
                DebugLogger.LogWarning(EssentialKitDomain.Default, "Button title is null.");
                return;
            }

            // create button object and add it to the dialog
            m_buttonActions.Add(new ButtonAction() { title = title, onClick = callback });

            try
            {
                m_nativeDialog.AddButton(title, true);
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(EssentialKitDomain.Default, exception);
            }
        }

        /// <summary>
        /// Shows the alert dialog to the user.
        /// </summary>
        public void Show()
        {
            try
            {
                m_nativeDialog.Show();
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(EssentialKitDomain.Default, exception);
            }
        }

        /// <summary>
        /// Dismisses the alert dialog before user selects an action.
        /// </summary>
        public void Dismiss()
        {
            try
            {
                m_nativeDialog.Dismiss();

                Destroy(gameObject);
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(EssentialKitDomain.Default, exception);
            }
        }

        #endregion

        #region Private methods

        private void SetTitleInternal(string value)
        {
            // validate arguments
            if (null == value)
            {
                DebugLogger.LogWarning(EssentialKitDomain.Default, "Title value is null.");
                return;
            }

            try
            {
                m_nativeDialog.SetTitle(value);
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(EssentialKitDomain.Default, exception);
            }
        }

        private void SetMessageInternal(string value)
        {
            // validate arguments
            if (null == value)
            {
                DebugLogger.LogWarning(EssentialKitDomain.Default, "Message value is null.");
                return;
            }

            try
            { 
                m_nativeDialog.SetMessage(value);
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(EssentialKitDomain.Default, exception);
            }
        }

        #endregion

        #region Event callback methods

        private void HandleButtonClickInternalCallback(int selectedButtonIndex)
        {
            // invoke selected button callback
            var     onClickCallback     = m_buttonActions[selectedButtonIndex].onClick;
            CallbackDispatcher.InvokeOnMainThread(onClickCallback);

            // hide dialog
            Dismiss();
        }

        #endregion

        #region Nested types

        private class ButtonAction
        {
            public  string          title;

            public  Callback        onClick;
        }

        #endregion
    }
}