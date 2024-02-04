#if UNITY_ANDROID
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins.Android;
using VoxelBusters.EssentialKit.Common.Android;
namespace VoxelBusters.EssentialKit.NetworkServicesCore.Android
{
    public class NativeNetworkConnectivity : NativeAndroidJavaObjectWrapper
    {
        #region Static properties

         private static AndroidJavaClass m_nativeClass;

        #endregion

        #region Constructor

        public NativeNetworkConnectivity(NativeContext context) : base(Native.kClassName, (object)context.NativeObject)
        {
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
            DebugLogger.Log("[Class : NativeNetworkConnectivity][Method : GetFeatureName]");
#endif
            return Call<string>(Native.Method.kGetFeatureName);
        }
        public void SetListener(NativeNetworkChangeListener listener)
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Class : NativeNetworkConnectivity][Method : SetListener]");
#endif
            Call(Native.Method.kSetListener, new object[] { listener } );
        }
        public void Start(NativeNetworkPollSettings settings)
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Class : NativeNetworkConnectivity][Method : Start]");
#endif
            Call(Native.Method.kStart, new object[] { settings.NativeObject } );
        }
        public void Stop()
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Class : NativeNetworkConnectivity][Method : Stop]");
#endif
            Call(Native.Method.kStop);
        }

        #endregion

        internal class Native
        {
            internal const string kClassName = "com.voxelbusters.essentialkit.networkservices.NetworkConnectivity";

            internal class Method
            {
                internal const string kSetListener = "setListener";
                internal const string kGetFeatureName = "getFeatureName";
                internal const string kStart = "start";
                internal const string kStop = "stop";
            }

        }
    }
}
#endif