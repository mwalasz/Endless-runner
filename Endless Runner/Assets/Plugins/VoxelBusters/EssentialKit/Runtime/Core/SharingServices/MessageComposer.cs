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
    /// The MessageComposer class provides a standard interface for composing and sending SMS or MMS messages.
    /// </summary>
    /// <description>
    /// <para>
    /// Before presenting the interface, populate the fields with the set of initial recipients and the message you want to send. 
    /// After presenting the interface, the user can edit your initial values before sending the message.
    /// </para>
    /// </description>
    /// <example>
    /// The following code example shows how to compose text message.
    /// <code>
    /// using UnityEngine;
    /// using System.Collections;
    /// using VoxelBusters.EssentialKit;
    /// 
    /// public class ExampleClass : MonoBehaviour 
    /// {
    ///     public void Start()
    ///     {
    ///         if (MessageComposer.CanSendText())
    ///         {
    ///             // create new instance and populate fields
    ///             MessageComposer newComposer = MessageComposer.CreateInstance();
    ///             newComposer.SetBody("Lorem ipsum dolor sit amet");
    ///             newComposer.SetCompletionCallback(OnMessageComposerClosed);
    ///             newComposer.Show();
    ///         }
    ///         else
    ///         {
    ///             // device doesn't support sending emails
    ///         }
    ///     }
    /// 
    ///     private void OnMessageComposerClosed(MessageComposerResult result, Error error)
    ///     {
    ///         // add your code
    ///     }
    /// }
    /// </code>
    /// </example>
    public sealed class MessageComposer : NativeFeatureBehaviour
    {
        #region Fields

        private     INativeMessageComposer                  m_nativeComposer        = null;

        private     EventCallback<MessageComposerResult>    m_callback              = null;

        #endregion

        #region Create methods

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageComposer"/> class.
        /// </summary>
        public static MessageComposer CreateInstance()
        {
            return CreateInstanceInternal<MessageComposer>("MessageComposer");
        }

        #endregion

        #region Static methods

        /// <summary>
        /// Returns a Boolean value indicating whether the current device is capable of sending text messages.
        /// </summary>
        /// <returns><c>true</c>, if the device can send text messages, <c>false</c> otherwise.</returns>
        public static bool CanSendText()
        {
            try
            {
                var     sharingInterface    = SharingServices.NativeInterface;
                return sharingInterface.CanSendText();
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(EssentialKitDomain.Default, exception);
                return false;
            }
        }

        /// <summary>
        /// Returns a Boolean value indicating whether or not messages can include attachments.
        /// </summary>
        /// <returns><c>true</c>, if the device can send attachments in MMS or iMessage messages, <c>false</c> otherwise.</returns>
        public static bool CanSendAttachments()
        {
            try
            {
                var     sharingInterface    = SharingServices.NativeInterface;
                return sharingInterface.CanSendAttachments();
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(EssentialKitDomain.Default, exception);
                return false;
            }
        }

        /// <summary>
        /// Returns a Boolean value indicating whether or not messages can include subject lines.
        /// </summary>
        /// <returns><c>true</c>, if the device can include subject lines in messages, <c>false</c> otherwise.</returns>
        public static bool CanSendSubject()
        {
            try
            {
                var     sharingInterface    = SharingServices.NativeInterface;
                return sharingInterface.CanSendSubject();
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

            // set properties
            var     sharingInterface    = SharingServices.NativeInterface;
            m_nativeComposer            = sharingInterface.CreateMessageComposer();
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
            return "Message Composer";
        }

        #endregion

        #region Setter methods

        /// <summary>
        /// Sets the initial recipients of the message..
        /// </summary>
        /// <param name="values">An array of string values containing the initial recipients of the message.</param>
        public void SetRecipients(params string[] values)
        {
            // validate arguments
            Assert.IsArgNotNull(values, "values");

            try
            {
                // make request
                m_nativeComposer.SetRecipients(values);
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(EssentialKitDomain.Default, exception);
            }
        }

        /// <summary>
        /// Sets the initial subject of the message.
        /// </summary>
        /// <param name="value">The initial subject for a message.</param>
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
        /// Sets the initial content of the message.
        /// </summary>
        /// <param name="value">The initial content in the body of a message.</param>
        public void SetBody(string value)
        {
            // validate arguments
            Assert.IsArgNotNull(value, "value");

            try
            {
                // make request
                m_nativeComposer.SetBody(value);
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(EssentialKitDomain.Default, exception);
            }
        }

        /// <summary>
        /// Captures a screenshot and adds it as an attachment of the message.
        /// </summary>
        /// <param name="fileName">The preferred filename to associate with the image.</param>
        public void AddScreenshot(string fileName)
        {
            // validate arguments
            Assert.IsArgNotNull(fileName, "fileName");

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
        /// Adds specified image as an attachment of the message.
        /// </summary>
        /// <param name="image">The image that has to be added as an attachment.</param>
        /// <param name="fileName">The preferred filename to associate with the image.</param>
        /// <param name="textureEncodingFormat">Texture encoding format.</param>
        public void AddImage(Texture2D image, string fileName, TextureEncodingFormat textureEncodingFormat = TextureEncodingFormat.JPG)
        { 
            // validate arguments
            Assert.IsArgNotNull(image, "image");
            Assert.IsArgNotNull(fileName, "fileName");

            try
            {
                // convert image to raw format
                string  mimeType;
                var     data        = image.Encode(textureEncodingFormat, out mimeType);

                // submit data
                m_nativeComposer.AddAttachmentData(data, mimeType, fileName);
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(EssentialKitDomain.Default, exception);
            }
        }

        /// <summary>
        /// Adds the specified data as an attachment of the message.
        /// </summary>
        /// <param name="data">The data of the file that has to be added as an attachment.</param>
        /// <param name="mimeType">The MIME type of the specified data.</param>
        /// <param name="fileName">The filename of the specified data.</param>
        public void AddAttachment(byte[] data, string mimeType, string fileName)
        {
            // validate arguments
            Assert.IsArgNotNull(data, "data");
            Assert.IsArgNotNull(mimeType, "mimeType");
            Assert.IsArgNotNull(fileName, "fileName");

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
        public void SetCompletionCallback(EventCallback<MessageComposerResult> callback)
        {
            // validate arguments
            Assert.IsArgNotNull(callback, "callback");

            // save callback reference
            m_callback  = callback;
        }

        /// <summary>
        /// Shows the message composer interface with values initially set.
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

		private void HandleComposerCloseInternalCallback(MessageComposerResultCode resultCode, Error error)
        {
            // send result to caller object
            var     result  = new MessageComposerResult(resultCode);
            CallbackDispatcher.InvokeOnMainThread(m_callback, result, error);

            // release object
            Destroy(gameObject);
        }

        #endregion
    }
}