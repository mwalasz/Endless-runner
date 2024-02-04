#if UNITY_ANDROID
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.EssentialKit.Common.Android;

namespace VoxelBusters.EssentialKit.Common.Android
{
    public class NativeLoadAssetListener : AndroidJavaProxy
    {
        #region Delegates

        public delegate void OnFailureDelegate(string error);
        public delegate void OnSuccessDelegate(NativeBytesWrapper data);

        #endregion

        #region Public callbacks

        public OnFailureDelegate  onFailureCallback;
        public OnSuccessDelegate  onSuccessCallback;

        #endregion


        #region Constructors

        public NativeLoadAssetListener() : base("com.voxelbusters.essentialkit.utilities.common.interfaces.ILoadAssetListener")
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

        public void onFailure(string error)
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Proxy : Callback] : " + "onFailure"  + " " + "[" + "error" + " : " + error +"]");
#endif
            if(onFailureCallback != null)
            {
                onFailureCallback(error);
            }
        }
        public void onSuccess(AndroidJavaObject data)
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Proxy : Callback] : " + "onSuccess"  + " " + "[" + "data" + " : " + data +"]");
#endif
            if(onSuccessCallback != null)
            {
                onSuccessCallback(new NativeBytesWrapper(data));
            }
        }

        #endregion
    }
}
#endif