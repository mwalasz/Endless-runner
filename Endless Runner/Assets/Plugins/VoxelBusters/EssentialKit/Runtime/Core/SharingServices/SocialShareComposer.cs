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
    /// The SocialShareComposer class provides an interface to compose a post for supported social networking services.
    /// </summary>
    /// <example>
    /// The following code example shows how to create composer for Facebook
    /// <code>
    /// using UnityEngine;
    /// using System.Collections;
    /// using VoxelBusters.EssentialKit;
    /// 
    /// public class ExampleClass : MonoBehaviour 
    /// {
    ///     public void Start()
    ///     {
    ///         SocialShareComposer newComposer = SocialShareComposer.CreateInstance(SocialShareComposerType.Facebook);
    ///         newComposer.AddText("Example");
    ///         newComposer.AddScreenshot();
    ///         newComposer.SetCompletionCallback(OnShareComposerClosed);
    ///         newComposer.Show();
    ///     }
    /// 
    ///     private void OnShareComposerClosed(SocialShareComposerResult result, Error error)
    ///     {
    ///         // add your code
    ///     }
    /// }
    /// </code>
    /// </example>
    public sealed class SocialShareComposer : NativeFeatureBehaviour
    {
        #region Fields

        private     INativeSocialShareComposer                  m_nativeComposer        = null;

        private     EventCallback<SocialShareComposerResult>    m_callback              = null;

        #endregion

        #region Create methods

        /// <summary>
        /// Initializes a new instance of the <see cref="SocialShareComposer"/> class.
        /// </summary>
        /// <param name="composerType">Composer type.</param>
        public static SocialShareComposer CreateInstance(SocialShareComposerType composerType)
        {
            return CreateInstanceInternal<SocialShareComposer>("SocialShareComposer", composerType);
        }

        #endregion

        #region Static methods

        public static bool IsComposerAvailable(SocialShareComposerType composerType)
        {
            try
            {
                var     sharingInterface    = SharingServices.NativeInterface;
                return sharingInterface.IsSocialShareComposerAvailable(composerType);
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
            var     composerType        = (args == null) ? SocialShareComposerType.Facebook : (SocialShareComposerType)args[0];
            m_nativeComposer            = sharingInterface.CreateSocialShareComposer(composerType);
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
            return "Social Share Composer";
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Adds the initial text to be posted.
        /// </summary>
        /// <param name="value">The text to add to the post.</param>
        public void SetText(string value)
        {
            // validate arguments
            Assert.IsArgNotNull(value, "value");

            try
            {
                // make request
                m_nativeComposer.SetText(value);
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(EssentialKitDomain.Default, exception);
            }
        }

        /// <summary>
        /// Creates a screenshot and adds it to the post.
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
        /// Adds an image to the post.
        /// </summary>
        /// <param name="image">The image to add to the post.</param>
        public void AddImage(Texture2D image, TextureEncodingFormat textureEncodingFormat = TextureEncodingFormat.JPG)
        {
            // validate arguments
            Assert.IsArgNotNull(image, "image");

            try
            {
                // convert image to raw format
                string  mimeType;
                byte[]  data        = image.Encode(textureEncodingFormat, out mimeType);

                // submit data
                m_nativeComposer.AddImage(data);
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(EssentialKitDomain.Default, exception);
            }
        }

        /// <summary>
        /// Adds an image to the post.
        /// </summary>
        /// <param name="imageData">The image to add to the post.</param>
        public void AddImage(byte[] imageData)
        {
            // validate arguments
            Assert.IsArgNotNull(imageData, "imageData");

            try
            {
                // make request
                m_nativeComposer.AddImage(imageData);
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(EssentialKitDomain.Default, exception);
            }
        }

        /// <summary>
        /// Adds a URL to the post.
        /// </summary>
        /// <param name="url">The URL to add to the post.</param>
        public void AddURL(URLString url)
        {
            // validate arguments
            Assert.IsTrue(url.IsValid, "Specified url is invalid.");

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
        /// Specify the action to execute after the share sheet is dismissed.
        /// </summary>
        /// <param name="callback">The action to be called on completion.</param>
        public void SetCompletionCallback(EventCallback<SocialShareComposerResult> callback)
        {
            // validate arguments
            Assert.IsArgNotNull(callback, "callback");

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

        private void HandleOnClose(SocialShareComposerResultCode resultCode, Error error)
        {
            // send result to caller object
            var     result  = new SocialShareComposerResult(resultCode);
            CallbackDispatcher.InvokeOnMainThread(m_callback, result, error);

            // release native object
            Destroy(gameObject);
        }

        #endregion
    }
}