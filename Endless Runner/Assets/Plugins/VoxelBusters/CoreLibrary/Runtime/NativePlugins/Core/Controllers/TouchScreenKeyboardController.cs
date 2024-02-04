using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityTouchScreenKeyboard = UnityEngine.TouchScreenKeyboard;

namespace VoxelBusters.CoreLibrary.NativePlugins
{
    public class TouchScreenKeyboardController : MonoBehaviour
    {
        #region Static fields

        [ClearOnReload]
        private     static      TouchScreenKeyboardController   s_sharedInstance        = null;

        #endregion

        #region Fields

#if UNITY_EDITOR
        private     bool        m_simulateInEditor              = false;
        private     float       m_editorKeyboardHeightRatio     = 0.4f;
#endif

        #endregion

        #region Static properties

        public static bool IsSupported => UnityTouchScreenKeyboard.isSupported;

        public static bool IsVisible { get; private set; }

        #endregion

        #region Static events

        [ClearOnReload]
        public static event Callback OnKeyboardDidShow = null;

        [ClearOnReload]
        public static event Callback OnKeyboardWillHide = null;

        #endregion

        #region Static methods

        public static int GetKeyboardHeight(bool includeInput)
        {
#if UNITY_EDITOR
            return (s_sharedInstance && s_sharedInstance.m_simulateInEditor) ? (int)(Screen.height * s_sharedInstance.m_editorKeyboardHeightRatio) : 0;
#elif UNITY_IOS
            return (int)TouchScreenKeyboard.area.height;
#elif UNITY_ANDROID
            using (AndroidJavaClass unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                AndroidJavaObject unityPlayer = unityClass.GetStatic<AndroidJavaObject>("currentActivity").Get<AndroidJavaObject>("mUnityPlayer");
                AndroidJavaObject view = unityPlayer.Call<AndroidJavaObject>("getView");
                AndroidJavaObject dialog = unityPlayer.Get<AndroidJavaObject>("mSoftInputDialog");
                if (view == null || dialog == null)
                    return 0;
                var decorHeight = 0;
                if (includeInput)
                {
                    AndroidJavaObject decorView = dialog.Call<AndroidJavaObject>("getWindow").Call<AndroidJavaObject>("getDecorView");
                    if (decorView != null)
                        decorHeight = decorView.Call<int>("getHeight");
                }
                using (AndroidJavaObject rect = new AndroidJavaObject("android.graphics.Rect"))
                {
                    view.Call("getWindowVisibleDisplayFrame", rect);
                    return Screen.height - rect.Call<int>("height") + decorHeight;
                }
            }
#else
            return 0;
#endif
        }

        private static bool IsKeyboardVisibleInternal()
        {
#if UNITY_EDITOR
            return (s_sharedInstance != null) && s_sharedInstance.m_simulateInEditor;
#elif UNITY_IOS || UNITY_ANDROID
            return UnityTouchScreenKeyboard.visible;
#else
            return false;
#endif
        }

        private static bool IsKeyboardActiveInternal()
        {
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                return GetKeyboardHeight(includeInput: false) > 0;
            }

            return IsKeyboardVisibleInternal();
        }

        #endregion

        #region Unity methods

        private void Awake()
        {
            // set shared instance
            s_sharedInstance    = this;
#if UNITY_EDITOR
            IsVisible           = (Application.isEditor && m_simulateInEditor);
#endif
        }

        private void Start()
        {
            // check whether this feature is supported
            if (!IsSupported)
            {
                enabled         = false;
            }
        }

        private void Update()
        {
            if (IsKeyboardVisibleInternal())
            {
                if (!IsVisible && IsKeyboardActiveInternal())
                {
                    IsVisible   = true;

                    SendKeyboardDidShow();
                }
            }
            else if (IsVisible)
            {
                IsVisible       = false;

                SendKeyboardWillHide();
            }
        }

        private void OnDestroy()
        {
            // reset shared instance
            if (s_sharedInstance == this)
            {
                s_sharedInstance    = null;
            }
        }

        #endregion

        #region Private methods

        private void SendKeyboardDidShow()
        {
            if (OnKeyboardDidShow != null)
            {
                OnKeyboardDidShow.Invoke();
            }
        }

        private void SendKeyboardWillHide()
        {
            if (OnKeyboardWillHide != null)
            {
                OnKeyboardWillHide.Invoke();
            }
        }

        #endregion
    }
}