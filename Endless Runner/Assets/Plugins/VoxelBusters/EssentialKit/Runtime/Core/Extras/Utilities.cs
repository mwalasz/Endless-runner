using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.EssentialKit.ExtrasCore;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit
{
    /// <summary>
    /// Provides a cross-platform interface to access commonly used native features.
    /// </summary>
    public static class Utilities
    {
        #region Static fields

        [ClearOnReload]
        private     static  INativeUtilityInterface     s_nativeInterface;

        #endregion

        #region Static methods

        public static void Initialize()
        {
            // Configure interface
            s_nativeInterface       = NativeFeatureActivator.CreateInterface<INativeUtilityInterface>(ImplementationSchema.Extras, true);
        }

        /// <summary>
        /// Creates a request to open the store review window.
        /// </summary>
        public static void RequestStoreReview()
        {
            try
            {
                s_nativeInterface.RequestStoreReview();
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(EssentialKitDomain.Default, exception);
            }
        }

        /// <summary>
        /// Opens the app store website page associated with this app.
        /// </summary>
        public static void OpenAppStorePage()
        {
            // validate argument
            var     settings    = EssentialKitSettings.Instance.ApplicationSettings;
            string  appId       = settings.GetAppStoreIdForActivePlatform();
            OpenAppStorePage(appId);
        }


        /// <summary>
        /// Opens the app store page associated with the specified application id.
        /// </summary>
        /// <description>
        /// For iOS platform, id is the value that identifies your app on App Store. 
        /// And on Android, it will be same as app's bundle identifier (com.example.test).
        /// </description>
        /// <param name="applicationIds">An array of string values, that holds app id's of each supported platform.</param>
        /// <example>
        /// The following code example shows how to open store link.
        /// <code>
        /// using UnityEngine;
        /// using System.Collections;
        /// using VoxelBusters.EssentialKit;
        /// 
        /// public class ExampleClass : MonoBehaviour 
        /// {
        ///     public void OpenStorePage ()
        ///     {
        ///         Utility.OpenStoreLink(PlatformValue.Android("com.example.app"), PlatformValue.IOS("ios-app-id"));
        ///     }
        /// }
        /// </code>
        /// </example>
        public static void OpenAppStorePage(params RuntimePlatformConstant[] applicationIds)
        {
            // validate arguments
            Assert.IsNotNullOrEmpty(applicationIds, "applicationIds");

            try
            {
                var     targetValue  = RuntimePlatformConstantUtility.FindConstantForActivePlatform(applicationIds);
                if (targetValue == null)
                {
                    DebugLogger.LogWarning(EssentialKitDomain.Default, "Application id not found for current platform.");
                    return;
                }

                s_nativeInterface.OpenAppStorePage(targetValue.Value);
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(EssentialKitDomain.Default, exception);
            }
        }

        /// <summary>
        /// Opens the app store website page associated with the specified application id.
        /// </summary>
        /// <param name="applicationId">Application id.</param>
        public static void OpenAppStorePage(string applicationId)
        {
            // validate arguments
            Assert.IsNotNullOrEmpty(applicationId, "Application id null/empty.");

            try
            {
                s_nativeInterface.OpenAppStorePage(applicationId);
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(EssentialKitDomain.Default, exception);
            }
        }

        public static void OpenApplicationSettings()
        {
            try
            {
                s_nativeInterface.OpenApplicationSettings();
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(EssentialKitDomain.Default, exception);
            }
        }

        #endregion
    }
}