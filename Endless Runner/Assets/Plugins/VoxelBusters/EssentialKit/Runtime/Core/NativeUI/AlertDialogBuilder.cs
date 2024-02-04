using VoxelBusters.CoreLibrary;

namespace VoxelBusters.EssentialKit
{
    /// <summary>
    /// Builder class for <see cref="AlertDialog"/> objects. Provides a convenient way to set the various fields of a <see cref="AlertDialog"/>. 
    /// </summary>
    /// <example>
    /// The following code example shows how to configure and present an alert dialog using builder.
    /// <code>
    /// using UnityEngine;
    /// using System.Collections;
    /// using VoxelBusters.EssentialKit;
    /// 
    /// public class ExampleClass : MonoBehaviour 
    /// {
    ///     public void Start()
    ///     {
    ///         AlertDialog newDialog   = new Builder()
    ///             .SetTitle(title);
    ///             .SetMessage(message);
    ///             .AddButton(button, OnAlertButtonClicked);
    ///             .Build();
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
    public class AlertDialogBuilder
    {
        #region Fields

        private     AlertDialog     m_alertDialog;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Builder"/> class.
        /// </summary>
        /// <param name="alertStyle">The alert style to be used.</param>
        public AlertDialogBuilder(AlertDialogStyle alertStyle = AlertDialogStyle.Default)
        {
            // create new instance
            m_alertDialog = AlertDialog.CreateInstance(alertStyle);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Sets the title of the alert.
        /// </summary>
        /// <param name="value">The title of the alert.</param>
        public AlertDialogBuilder SetTitle(string value)
        {
            m_alertDialog.Title = value;

            return this;
        }

        /// <summary>
        /// Sets the message of the alert.
        /// </summary>
        /// <param name="value">The descriptive text that provides more details about the reason for the alert.</param>
        public AlertDialogBuilder SetMessage(string value)
        {
            m_alertDialog.Message = value;

            return this;
        }

        /// <summary>
        /// Adds an action button to the alert. Here, the default style is used.
        /// </summary>
        /// <param name="title">The title of the button.</param>
        /// <param name="callback">The method to execute when the user selects this button.</param>
        public AlertDialogBuilder AddButton(string title, Callback callback)
        {
            m_alertDialog.AddButton(title, callback);

            return this;
        }

        /// <summary>
        /// Adds action button to the alert. This style type indicates the action cancels the operation and leaves things unchanged.
        /// </summary>
        /// <param name="title">The title of the button.</param>
        /// <param name="callback">The method to execute when the user selects this button.</param>
        public AlertDialogBuilder AddCancelButton(string title, Callback callback)
        {
            m_alertDialog.AddCancelButton(title, callback);

            return this;
        }

        /// <summary>
        /// Combines all of the options that have been set and return a new <see cref="AlertDialog"/> object.
        /// </summary>
        /// <returns>The build.</returns>
        public AlertDialog Build()
        {
            try
            {
                return m_alertDialog;
            }
            finally
            {
                m_alertDialog   = null;
            }
        }

        #endregion
    }
}