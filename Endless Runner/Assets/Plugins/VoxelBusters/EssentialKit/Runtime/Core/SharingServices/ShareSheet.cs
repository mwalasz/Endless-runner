using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins;
using VoxelBusters.EssentialKit.SharingServicesCore;

namespace VoxelBusters.EssentialKit
{
    /// <summary>
    /// The ShareSheet class provides an interface to access standard services from your app.
    /// </summary>
    /// <description>
    /// <para>
    /// The system provides several standard services, such as copying items to the pasteboard, posting content to social media sites, sending items via email or SMS, and more. 
    /// </para>
    /// </description>
    /// <example>
    /// The following code example shows how to use share sheet.
    /// <code>
    /// using UnityEngine;
    /// using System.Collections;
    /// using VoxelBusters.EssentialKit;
    /// 
    /// public class ExampleClass : MonoBehaviour 
    /// {
    ///     public void Start()
    ///     {
    ///         ShareSheet  newComposer = ShareSheet.CreateInstance();
    ///         newComposer.AddText("Example");
    ///         newComposer.AddScreenshot();
    ///         newComposer.SetCompletionCallback(OnShareSheetClosed);
    ///         newComposer.Show();
    ///     }
    /// 
    ///     private void OnShareSheetClosed(ShareSheetResult result, Error error)
    ///     {
    ///         // add your code
    ///     }
    /// }
    /// </code>
    /// </example>
    public sealed class ShareSheet : NativeFeatureBehaviour
    {
        #region Fields

        private     INativeShareSheet                   m_nativeComposer        = null;

        private     EventCallback<ShareSheetResult>     m_callback              = null;

        #endregion

        #region Create methods

        /// <summary>
        /// Initializes a new instance of the <see cref="ShareSheet"/> class.
        /// </summary>
        public static ShareSheet CreateInstance()
        {
            return CreateInstanceInternal<ShareSheet>("ShareSheet");
        }

        #endregion

        #region Lifecycle methods

        protected override void AwakeInternal(object[] args)
        {
            base.AwakeInternal(args);

            // set properties
            var     sharingInterface    = SharingServices.NativeInterface;
            m_nativeComposer            = sharingInterface.CreateShareSheet();
            m_callback                  = null;

            // register for events
            m_nativeComposer.OnClose   += HandleOnClose;
        }

        protected override void DestroyInternal()
        {
            base.DestroyInternal();

            // unregister from event
            m_nativeComposer.OnClose   -= HandleOnClose;
            
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
            return "Share Sheet";
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Adds the initial text to the share sheet.
        /// </summary>
        /// <param name="value">The text to add.</param>
        public void AddText(string value)
        {
            // validate arguments
            if (null == value)
            {
                DebugLogger.LogWarning(EssentialKitDomain.Default, "Text value is null.");
                return;
            }

            try
            {
                // make request
                m_nativeComposer.AddText(value);
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(EssentialKitDomain.Default, exception);
            }
        }

        /// <summary>
        /// Creates a screenshot and adds it to the share sheet.
        /// </summary>
        public void AddScreenshot()
        {
            try
            {
                // make request
                m_nativeComposer.AddScreenshot();
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(EssentialKitDomain.Default, exception);
            }
        }

        /// <summary>
        /// Adds the specified image to the share sheet.
        /// </summary>
        /// <param name="image">The image to add.</param>
        public void AddImage(Texture2D image, TextureEncodingFormat textureEncodingFormat = TextureEncodingFormat.JPG)
        {
            // validate arguments
            if (null == image)
            {
                DebugLogger.LogWarning(EssentialKitDomain.Default, "Image is null.");
                return;
            }

            try
            {
                // convert image to raw format
                string  mimeType;
                byte[]  data        = image.Encode(textureEncodingFormat, out mimeType);

                // submit data
                m_nativeComposer.AddImage(data, mimeType);
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(EssentialKitDomain.Default, exception);
            }
        }

        /// <summary>
        /// Adds the specified image to the share sheet.
        /// </summary>
        /// <param name="imageData">The image to add to the post.</param>
        public void AddImage(byte[] imageData, string mimeType)
        {
            // validate arguments
            if (null == imageData)
            {
                DebugLogger.LogWarning(EssentialKitDomain.Default, "Image data array is null.");
                return;
            }

            try
            {
                // make request
                m_nativeComposer.AddImage(imageData, mimeType);
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(EssentialKitDomain.Default, exception);
            }
        }

        /// <summary>
        /// Adds the URL to the share sheet.
        /// </summary>
        /// <param name="url">The URL to add.</param>
        public void AddURL(URLString url)
        {
            // validate arguments
            if (false == url.IsValid)
            {
                DebugLogger.LogWarning(EssentialKitDomain.Default, "Specified url is invalid.");
                return;
            }

            try
            {
                // make request
                m_nativeComposer.AddURL(url);
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(EssentialKitDomain.Default, exception);
            }
        }


        /// <summary>
        /// Adds the attachment to the share sheet.
        /// </summary>
        /// <param name="url">The data to add.</param>
        public void AddAttachment(byte[] data, string mimeType, string filename)
        {
            m_nativeComposer.AddAttachment(data, mimeType, filename);
        }

        /// <summary>
        /// Specify the action to execute after the share sheet is dismissed.
        /// </summary>
        /// <param name="callback">The action to be called on completion.</param>
        public void SetCompletionCallback(EventCallback<ShareSheetResult> callback)
        {
            // validate arguments
            if (null == callback)
            {
                DebugLogger.LogWarning(EssentialKitDomain.Default, "Callback is null.");
                return;
            }

            // save callback reference
            m_callback  = callback;
        }

        /// <summary>
        /// Shows the share sheet interface, anchored at screen position (0, 0).
        /// </summary>
        public void Show()
        {
            Show(Vector2.zero);
        }

        /// <summary>
        /// Shows the share sheet interface, anchored to given position.
        /// </summary>
        /// <param name="screenPosition">The position (in the coordinate system of screen) at which to anchor the share sheet menu. This property is used in iOS platform only.</param>
        public void Show(Vector2 screenPosition)
        {
            try
            {
                // present view
                m_nativeComposer.Show(screenPosition);
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

        private void HandleOnClose(ShareSheetResultCode resultCode, Error error)
        {
            // send result to caller object
            var     result  = new ShareSheetResult(resultCode);
            CallbackDispatcher.InvokeOnMainThread(m_callback, result, error);

            // release native object
            Destroy(gameObject);
        }

        #endregion
    }
}