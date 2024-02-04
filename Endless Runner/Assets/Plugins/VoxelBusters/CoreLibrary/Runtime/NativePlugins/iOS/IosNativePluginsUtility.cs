#if UNITY_IOS || UNITY_TVOS
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.InteropServices;
using System.Globalization;
using AOT;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.CoreLibrary.NativePlugins.iOS
{
    public static class IosNativePluginsUtility
    {
        #region Constants

        private const string kZuluFormat = "yyyy-MM-dd HH:mm:ss zzz";

        #endregion

        #region Native binding methods
        
        [DllImport("__Internal")]
        private static extern void NPUtilityRegisterCallbacks(LoadImageNativeCallback loadImageCallback);
        
        [DllImport("__Internal")]
        private static extern void NPUtilityLoadImage(IntPtr nativeDataPtr, IntPtr tagPtr);

        [DllImport("__Internal")]
        private static extern void NPUtilityTakeScreenshot(IntPtr tagPtr);

        [DllImport("__Internal")]
        private static extern void NPUtilityRetainObject(IntPtr nativePtr);

        [DllImport("__Internal")]
        private static extern void NPUtilityReleaseObject(IntPtr nativePtr);

        [DllImport("__Internal")]
        private static extern void NPUtilityOpenSettings();

        [DllImport("__Internal")]
        private static extern void NPUtilityFreeCPointerObject(IntPtr nativePtr);

        [DllImport("__Internal")]
        private static extern void NPUtilitySetLastTouchPosition(float posX, float posY);

        #endregion

        #region Delegates

        public delegate void LoadImageCallback(byte[] imageData, Error error);

        private delegate void LoadImageNativeCallback(IntPtr byteArrayPtr, int byteLength, IntPtr tagPtr);

        #endregion

        #region Constructors

        static IosNativePluginsUtility()
        {
            // initialise
            NPUtilityRegisterCallbacks(loadImageCallback: HandleLoadImageCallbackInternal);
        }

        #endregion

        #region Reference management methods

        public static void RetainNativeObject(IntPtr nativePtr)
        {
            DebugLogger.Log(CoreLibraryDomain.NativePlugins, $"Retaining native object pointer: {nativePtr}.");
            NPUtilityRetainObject(nativePtr);
        }

        public static void ReleaseNativeObject(IntPtr nativePtr)
        {
            DebugLogger.Log(CoreLibraryDomain.NativePlugins, $"Releasing native object pointer: {nativePtr}.");
            NPUtilityReleaseObject(nativePtr);
        }

        public static void FreeCPointerObject(IntPtr nativePtr)
        {
            NPUtilityFreeCPointerObject(nativePtr);
        }

        #endregion

        #region Image methods

        public static void LoadImage(IntPtr nativePtr, LoadImageCallback callback)
        {
            // check arguments
            Assert.IsArgNotNull(callback, "callback");

            // save callback as handle
            var     callbackPtr     = MarshalUtility.GetIntPtr(callback);
            NPUtilityLoadImage(nativePtr, callbackPtr);
        }

        public static void TakeScreenshot(LoadImageCallback callback)
        {
            // check arguments
            Assert.IsArgNotNull(callback, "callback");

            // make request
            var     callbackPtr     = MarshalUtility.GetIntPtr(callback);
            NPUtilityTakeScreenshot(callbackPtr);
        }

        #endregion

        #region Date methods

        public static DateTime ParseDateTimeStringInUTCFormat(this string value)
        {
            if (value != null)
            {
                return DateTime.ParseExact(value, kZuluFormat, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
            }

            return default(DateTime);
        }

        public static string ToZuluFormatString(this DateTime dateTime)
        {
            if (dateTime.Kind == DateTimeKind.Utc)
            {
                return dateTime.ToString(kZuluFormat);
            }
            else
            {
                return dateTime.ToUniversalTime().ToZuluFormatString();
            }
        }

        #endregion

        #region Misc methods

        // When you open the URL built from this string, the system launches the Settings app and displays the app’s custom settings, if it has any.
        public static void OpenApplicationSettings()
        {
            NPUtilityOpenSettings();
        }

        public static void SetLastTouchPosition(Vector2 position)
        {
            var     invertedPosition    = UnityEngineUtility.InvertScreenPosition(position, invertX: false, invertY: true);
            NPUtilitySetLastTouchPosition(invertedPosition.x, invertedPosition.y);
        }

        public static void SetLastTouchPosition()
		{
			Vector2 _touchPosition	= Vector2.zero;

#if UNITY_EDITOR
			_touchPosition			= Input.mousePosition;
#else
			if (Input.touchCount > 0)
			{
				_touchPosition		= Input.GetTouch(0).position;
				_touchPosition.y	= Screen.height - _touchPosition.y;
			}
#endif
			// Set popover position
			SetLastTouchPosition(_touchPosition);
		}

        #endregion

        #region Native callback methods

        [MonoPInvokeCallback(typeof(LoadImageNativeCallback))]
        private static void HandleLoadImageCallbackInternal(IntPtr dataArrayPtr, int dataLength, IntPtr tagPtr)
        {
            // get handle from pointer
            var     handle      = GCHandle.FromIntPtr(tagPtr);
            var     callback    = (LoadImageCallback)handle.Target;

            try
            {
                if (IntPtr.Zero == dataArrayPtr)
                {
                    // send result
                    callback(null, new Error(description: "Could not load texture data."));
                }
                else
                {
                    // create texture from raw data
                    var     byteArray   = new byte[dataLength];
                    Marshal.Copy(dataArrayPtr, byteArray, 0, dataLength);

                    // send result
                    callback(byteArray, null);
                }
            }
            finally
            {
                // release handle
                handle.Free();
            }
        }

        #endregion
    }
}
#endif