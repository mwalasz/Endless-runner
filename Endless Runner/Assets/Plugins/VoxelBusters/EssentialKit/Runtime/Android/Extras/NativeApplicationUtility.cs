#if UNITY_ANDROID
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins.Android;
using VoxelBusters.EssentialKit.Common.Android;
namespace VoxelBusters.EssentialKit.ExtrasCore.Android
{
    public class NativeApplicationUtility : NativeAndroidJavaObjectWrapper
    {
        #region Static properties

         private static AndroidJavaClass m_nativeClass;

        #endregion
        #region Private properties
        private NativeActivity Activity
        {
            get;
            set;
        }
        #endregion

        #region Constructor

        public NativeApplicationUtility(NativeContext context) : base(Native.kClassName, (object)context.NativeObject)
        {
            Activity    = new NativeActivity(context);
        }

        #endregion
        #region Static methods
        private static AndroidJavaClass GetClass()
        {
            if (m_nativeClass == null)
            {
                m_nativeClass = new AndroidJavaClass(Native.kClassName);
            }
            return m_nativeClass;
        }
        #endregion
        #region Public methods

        public string GetFeatureName()
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Class : NativeApplicationUtility][Method : GetFeatureName]");
#endif
            return Call<string>(Native.Method.kGetFeatureName);
        }
        public void OpenApplicationSettings()
        {
            Activity.RunOnUiThread(() => {
#if NATIVE_PLUGINS_DEBUG_ENABLED
                DebugLogger.Log("[Class : NativeApplicationUtility][Method(RunOnUiThread) : OpenApplicationSettings]");
#endif
                Call(Native.Method.kOpenApplicationSettings);
            });
        }
        public void OpenGooglePlayStoreLink(string packageName)
        {
            Activity.RunOnUiThread(() => {
#if NATIVE_PLUGINS_DEBUG_ENABLED
                DebugLogger.Log("[Class : NativeApplicationUtility][Method(RunOnUiThread) : OpenGooglePlayStoreLink]");
#endif
                Call(Native.Method.kOpenGooglePlayStoreLink, new object[] { packageName } );
            });
        }

        #endregion

        internal class Native
        {
            internal const string kClassName = "com.voxelbusters.essentialkit.extras.ApplicationUtility";

            internal class Method
            {
                internal const string kOpenApplicationSettings = "openApplicationSettings";
                internal const string kOpenGooglePlayStoreLink = "openGooglePlayStoreLink";
                internal const string kGetFeatureName = "getFeatureName";
            }

        }
    }
}
#endif