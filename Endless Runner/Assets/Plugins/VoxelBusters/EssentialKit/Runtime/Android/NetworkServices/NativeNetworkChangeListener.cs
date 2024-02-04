#if UNITY_ANDROID
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins.Android;
using VoxelBusters.EssentialKit.Common.Android;

namespace VoxelBusters.EssentialKit.NetworkServicesCore.Android
{
    public class NativeNetworkChangeListener : AndroidJavaProxy
    {
        #region Delegates

        public delegate void OnChangeDelegate(bool isConnected);

        #endregion

        #region Public callbacks

        public OnChangeDelegate  onChangeCallback;

        #endregion


        #region Constructors

        public NativeNetworkChangeListener() : base("com.voxelbusters.essentialkit.networkservices.INetworkServices$INetworkChangeListener")
        {
        }

        #endregion


        #region Public methods
#if NATIVE_PLUGINS_DEBUG_ENABLED
        public override AndroidJavaObject Invoke(string methodName, AndroidJavaObject[] javaArgs)
        {
            DebugLogger.Log("**************************************************");
            DebugLogger.Log("[Generic Invoke : " +  methodName + "]" + " Args Length : " + (javaArgs != null ? javaArgs.Length : 0));
            if(javaArgs != null)
            {
                System.Text.StringBuilder builder = new System.Text.StringBuilder();

                foreach(AndroidJavaObject each in javaArgs)
                {
                    if(each != null)
                    {
                        builder.Append(string.Format("[Type : {0} Value : {1}]", each.Call<AndroidJavaObject>("getClass").Call<string>("getName"), each.Call<string>("toString")));
                        builder.Append("\n");
                    }
                    else
                    {
                        builder.Append("[Value : null]");
                        builder.Append("\n");
                    }
                }

                DebugLogger.Log(builder.ToString());
            }
            DebugLogger.Log("-----------------------------------------------------");
            return base.Invoke(methodName, javaArgs);
        }
#endif

        public void onChange(bool isConnected)
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Proxy : Callback] : " + "onChange"  + " " + "[" + "isConnected" + " : " + isConnected +"]");
#endif
            if(onChangeCallback != null)
            {
                onChangeCallback(isConnected);
            }
        }

        #endregion
    }
}
#endif