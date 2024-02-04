#if UNITY_ANDROID
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins.Android;
using VoxelBusters.EssentialKit.Common.Android;

namespace VoxelBusters.EssentialKit.NetworkServicesCore.Android
{
    public class NativeNetworkPollSettings : NativeAndroidJavaObjectWrapper
    {
        #region Static properties

         private static AndroidJavaClass m_nativeClass;

        #endregion
        #region Constructor

        // Default constructor
        public NativeNetworkPollSettings(AndroidJavaObject androidJavaObject) : base(Native.kClassName, androidJavaObject)
        {
        }
        public NativeNetworkPollSettings(NativeAndroidJavaObjectWrapper wrapper) : base(wrapper)
        {
        }
        public NativeNetworkPollSettings() : base(Native.kClassName)
        {
        }

#if NATIVE_PLUGINS_DEBUG_ENABLED
        ~NativeNetworkPollSettings()
        {
            DebugLogger.Log("Disposing NativeNetworkPollSettings");
        }
#endif
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

        public long GetConnectionTimeOutPeriod()
        {
            return Call<long>(Native.Method.kGetConnectionTimeOutPeriod);
        }
        public string GetIpAddress()
        {
            return Call<string>(Native.Method.kGetIpAddress);
        }
        public int GetMaxRetryCount()
        {
            return Call<int>(Native.Method.kGetMaxRetryCount);
        }
        public int GetPortNumber()
        {
            return Call<int>(Native.Method.kGetPortNumber);
        }
        public float GetTimeGapBetweenPolls()
        {
            return Call<float>(Native.Method.kGetTimeGapBetweenPolls);
        }
        public void SetConnectionTimeOutPeriod(long timeOutPeriod)
        {
            Call(Native.Method.kSetConnectionTimeOutPeriod, timeOutPeriod);
        }
        public void SetIpAddress(string ipAddress)
        {
            Call(Native.Method.kSetIpAddress, ipAddress);
        }
        public void SetMaxRetryCount(int retryCount)
        {
            Call(Native.Method.kSetMaxRetryCount, retryCount);
        }
        public void SetPortNumber(int port)
        {
            Call(Native.Method.kSetPortNumber, port);
        }
        public void SetTimeGapBetweenPolls(float timeGap)
        {
            Call(Native.Method.kSetTimeGapBetweenPolls, timeGap);
        }

        #endregion

        internal class Native
        {
            internal const string kClassName = "com.voxelbusters.essentialkit.networkservices.NetworkPollSettings";

            internal class Method
            {
                internal const string kSetIpAddress = "setIpAddress";
                internal const string kGetIpAddress = "getIpAddress";
                internal const string kGetTimeGapBetweenPolls = "getTimeGapBetweenPolls";
                internal const string kGetPortNumber = "getPortNumber";
                internal const string kSetTimeGapBetweenPolls = "setTimeGapBetweenPolls";
                internal const string kSetPortNumber = "setPortNumber";
                internal const string kGetConnectionTimeOutPeriod = "getConnectionTimeOutPeriod";
                internal const string kSetConnectionTimeOutPeriod = "setConnectionTimeOutPeriod";
                internal const string kGetMaxRetryCount = "getMaxRetryCount";
                internal const string kSetMaxRetryCount = "setMaxRetryCount";
            }

        }
    }
}
#endif