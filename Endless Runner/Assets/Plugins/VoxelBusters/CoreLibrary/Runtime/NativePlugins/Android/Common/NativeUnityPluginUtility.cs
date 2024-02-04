#if UNITY_ANDROID
using UnityEngine;
using System.Collections.Generic;
using System;

namespace VoxelBusters.CoreLibrary.NativePlugins.Android
{
    public class NativeUnityPluginUtility
    {
        private static AndroidJavaObject    s_context           = null;
        private static NativeContext        s_nativeContext     = null;
        private static NativeActivity       s_nativeActivity    = null;
        private static NativeViewGroup      s_decorRootView     = null;

        public static NativeActivity GetActivity()
        {
            if(s_nativeContext == null)
            {
                s_nativeActivity = new NativeActivity(GetUnityActivity());
            }
            return s_nativeActivity;
        }

        public static NativeContext GetContext()
        {
            if (s_nativeContext == null)
            {
                s_nativeContext = new NativeContext(GetUnityActivity());
            }
            return s_nativeContext;
        }

        public static To[] Map<From, To>(List<From> fromList)
        {
            List<To> list = new List<To>();
            foreach (From each in fromList)
            {
                list.Add((To)Activator.CreateInstance(typeof(To), new object[] { each }));
            }

            return list.ToArray();
        }

        public static NativeViewGroup GetDecorRootView()
        {
            if(s_decorRootView == null)
            {
                AndroidJavaObject activity = GetUnityActivity();
                AndroidJavaObject window = activity.Call<AndroidJavaObject>("getWindow");
                AndroidJavaObject decorView = window.Call<AndroidJavaObject>("getDecorView");
                AndroidJavaObject rootView = decorView.Call<AndroidJavaObject>("getRootView");
                s_decorRootView = new NativeViewGroup(rootView);
            }

            return s_decorRootView;
        }

        private static AndroidJavaObject GetUnityActivity()
        {
            if (s_context == null)
            {
                AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                s_context = jc.GetStatic<AndroidJavaObject>("currentActivity");
            }
            return s_context;
        }

        //Get root view group
    }
}
#endif
      