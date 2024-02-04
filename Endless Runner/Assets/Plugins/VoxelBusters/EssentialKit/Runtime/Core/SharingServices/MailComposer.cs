using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins;
using VoxelBusters.EssentialKit.SharingServicesCore;

namespace VoxelBusters.EssentialKit
{
    /// <summary>
    /// The MailComposer class provides an interface to compose and send an email message.
    /// </summary>
    /// <description>
    /// <para>
    /// Use this composer to display a standard email interface inside your app. 
    /// Before presenting the interface, populate the fields with initial values for the subject, email recipients, body text, and attachments of the email. 
    /// After presenting the interface, the user can edit your initial values before sending the email.
    /// </para>
    /// </description>
    /// <example>
    /// The following code example shows how to compose mail.
    /// <code>
    /// using UnityEngine;
    /// using System.Collections;
    /// using VoxelBusters.EssentialKit;
    /// 
    /// public class ExampleClass : MonoBehaviour 
    /// {
    ///     public void Start()
    ///     {
    ///         if (MailComposer.CanSendMail())
    ///         {
    ///             // create new instance and populate fields
    ///             MailComposer    newComposer = MailComposer.CreateInstance();
    ///             newComposer.SetSubject("Example");
    ///             newComposer.SetBody("Lorem ipsum dolor sit amet");
    ///             newComposer.AddScreenshot("screenshot.jpg");
    ///             newComposer.SetCompletionCallback(OnMailComposerClosed);
    ///             newComposer.Show();
    ///         }
    ///         else
    ///         {
    ///             // device doesn't support sending emails
    ///         }
    ///     }
    /// 
    ///     private void OnMailComposerClosed(MailComposerResult result, Error error)
    ///     {
    ///         // add your code
    ///     }
    /// }
    /// </code>
    /// </example>
    public sealed class MailComposer : NativeFeatureBehaviour
    {
        #region Fields

        private     INativeMailComposer                     m_nativeComposer        = null;

        private     EventCallback<MailComposerResult>       m_callback              = null;

        #endregion

        #region Create methods

        /// <summary>
        /// Initializes a new instance of the <see cref="MailComposer"/> class.
        /// </summary>
        public static MailComposer CreateInstance()
        {
            return CreateInstanceInternal<MailComposer>("MailComposer");
        }

        #endregion

        #region Static methods

        /// <summary>
        /// Returns a Boolean indicating whether the current device is able to send email.
        /// </summary>
        /// <returns><c>true</c>, if the device is configured for sending email, <c>false</c> otherwise.</returns>
        public static bool CanSendMail()
        {
            try
            {
                var     sharingInterface    = SharingServices.NativeInterface;
                return sharingInterface.CanSendMail();
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(EssentialKitDomain.Default, exception);
                return false;
            }
        }

        #endregion

        #region Lifecycle methods

        protected override void AwakeInternal(object[] args)
        {
            base.AwakeInternal(args);

            // initialise component
            var     sharingInterface    = SharingServices.NativeInterface;
            m_nativeComposer            = sharingInterface.CreateMailComposer();
            m_callback                  = null;

            // register for events
            m_nativeComposer.OnClose   += HandleComposerCloseInternalCallback;
        }

        protected override void DestroyInternal()
        {
            base.DestroyInternal();

            // unregister from event
            m_nativeComposer.OnClose   -= HandleComposerCloseInternalCallback;
            
            // reset interface properties
            m_nativeComposer.Dispose();

            m_callback          = null;
        }

        #endregion

        #region Behaviour methods

        public override bool IsAvailable()
        {
            return SharingServices.NativeInterface.IsAvailable;
        }

        protected override string GetFeatureName()
        {
            return "Mail Composer";
        }

        #endregion

        #region Setter methods

        /// <summary>
        /// Sets the initial recipients to include in the email’s “To” field.
        /// </summary>
        /// <param name="values">An array of string values, each of which contains the email address of a single recipient.</param>
        public void SetToRecipients(params string[] values)
        {
            // validate arguments
            Assert.IsArgNotNull(values, "values");

            try
            {
                // make request
                m_nativeComposer.SetToRecipients(values);
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(EssentialKitDomain.Default, exception);
            }
        }

        /// <summary>
        /// Sets the initial recipients to include in the email’s “Cc” field.
        /// </summary>
        /// <param name="values">An array of string values, each of which contains the email address of a single recipient.</param>
        public void SetCcRecipients(params string[] values)
        {
            // validate arguments
            Assert.IsArgNotNull(values, "values");

            try
            {
                // make request
                m_nativeComposer.SetCcRecipients(values);
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(EssentialKitDomain.Default, exception);
            }
        }

        /// <summary>
        /// Sets the initial recipients to include in the email’s “Bcc” field.
        /// </summary>
        /// <param name="values">An array of string values, each of which contains the email address of a single recipient.</param>
        public void SetBccRecipients(params string[] values)
        {
            // validate arguments
            Assert.IsArgNotNull(values, "values");

            try
            {
                // make request
                m_nativeComposer.SetBccRecipients(values);
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(EssentialKitDomain.Default, exception);
            }
        }

        /// <summary>
        /// Sets the initial text for the subject line of the email.
        /// </summary>
        /// <param name="value">The text to display in the subject line.</param>
        public void SetSubject(string value)
        {
            // validate arguments
            Assert.IsArgNotNull(value, "value");

            try
            {
                // make request
                m_nativeComposer.SetSubject(value);
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(EssentialKitDomain.Default, exception);
            }
        }

        /// <summary>
        /// Sets the initial body text to include in the email.
        /// </summary>
        /// <param name="value">The initial body text of the message. The text is interpreted as either plain text or HTML depending on the value of the isHTML parameter..</param>
        /// <param name="isHtml">Specify YES if the body parameter contains HTML content or specify NO if it contains plain text.</param>
        public void SetBody(string value, bool isHtml = false)
        {
            // validate arguments
            Assert.IsArgNotNull(value, "value");

            try
            {
                // make request
                m_nativeComposer.SetBody(value, isHtml);
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(EssentialKitDomain.Default, exception);
            }
        }

        /// <summary>
        /// Captures a screenshot and adds it as an attachment of the email.
        /// </summary>
        /// <param name="fileName">The preferred filename to associate with the image.</param>
        public void AddScreenshot(string fileName)
        {
            // validate arguments
            Assert.IsNotNullOrEmpty(fileName, "fileName");

            try
            {
                // make request
                m_nativeComposer.AddScreenshot(fileName);
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(EssentialKitDomain.Default, exception);
            }
        }

        /// <summary>
        /// Adds specified image as an attachment of the email.
        /// </summary>
        /// <param name="image">The image that has to be added as an attachment.</param>
        /// <param name="fileName">The preferred filename to associate with the image.</param>
        /// <param name="textureEncodingFormat">Texture encoding format.</param>
        public void AddImage(Texture2D image, string fileName, TextureEncodingFormat textureEncodingFormat = TextureEncodingFormat.JPG)
        { 
            // validate arguments
            Assert.IsArgNotNull(image, "image");
            Assert.IsNotNullOrEmpty(fileName, "fileName");

            try
            {
                // convert image to raw format
                string  mimeType;
                byte[]  data        = image.Encode(textureEncodingFormat, out mimeType);

                // submit data
                m_nativeComposer.AddAttachmentData(data, mimeType, fileName);
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(EssentialKitDomain.Default, exception);
            }
        }

        /// <summary>
        /// Adds the specified data as an attachment of the email.
        /// </summary>
        /// <param name="data">The data of the file that has to be added as an attachment.</param>
        /// <param name="mimeType">The MIME type of the specified data.</param>
        /// <param name="fileName">The filename of the specified data.</param>
        public void AddAttachment(byte[] data, string mimeType, string fileName)
        {
            // validate arguments
            Assert.IsArgNotNull(data, "data");
            Assert.IsNotNullOrEmpty(mimeType, "mimeType");
            Assert.IsNotNullOrEmpty(fileName, "fileName");

            try
            {
                // make request
                m_nativeComposer.AddAttachmentData(data, mimeType, fileName);
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(EssentialKitDomain.Default, exception);
            }
        }

        /// <summary>
        /// Specify the action to execute after the composer is dismissed.
        /// </summary>
        /// <param name="callback">The action to be called on completion.</param>
        public void SetCompletionCallback(EventCallback<MailComposerResult> callback)
        {
            // validate arguments
            Assert.IsArgNotNull(callback, "callback");

            // save callback reference
            m_callback  = callback;
        }

        /// <summary>
        /// Shows the email composer interface with values initially set.
        /// </summary>
        public void Show()
        {
            try
            {
                // present view
                m_nativeComposer.Show();
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(EssentialKitDomain.Default, exception);
            }
        }

        #endregion

        #region Private methods

        #endregion

        #region Event callback methods

        private void HandleComposerCloseInternalCallback(MailComposerResultCode resultCode, Error error)
        {
            // send result to caller object
            var     result  = new MailComposerResult(resultCode);
            CallbackDispatcher.InvokeOnMainThread(m_callback, result, error);

            // release object
            Destroy(gameObject);
        }

        #endregion
    }
}