#if UNITY_ANDROID
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins.Android;
using VoxelBusters.EssentialKit.Common.Android;

namespace VoxelBusters.EssentialKit.NativeUICore.Android
{
    public class NativeDatePickerListener : AndroidJavaProxy
    {
        #region Delegates

        public delegate void OnCancelDelegate();
        public delegate void OnSuccessDelegate(int year, int month, int dayOfMonth);

        #endregion

        #region Public callbacks

        public OnCancelDelegate  onCancelCallback;
        public OnSuccessDelegate  onSuccessCallback;

        #endregion


        #region Constructors

        public NativeDatePickerListener() : base("com.voxelbusters.essentialkit.uiviews.IUiViews$IDatePickerListener")
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

        public void onCancel()
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Proxy : Callback] : " + "onCancel" );
#endif
            if(onCancelCallback != null)
            {
                onCancelCallback();
            }
        }
        public void onSuccess(int year, int month, int dayOfMonth)
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Proxy : Callback] : " + "onSuccess"  + " " + "[" + "year" + " : " + year +"]" + " " + "[" + "month" + " : " + month +"]" + " " + "[" + "dayOfMonth" + " : " + dayOfMonth +"]");
#endif
            if(onSuccessCallback != null)
            {
                onSuccessCallback(year, month, dayOfMonth);
            }
        }

        #endregion
    }
}
#endif