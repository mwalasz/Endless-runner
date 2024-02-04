#if UNITY_ANDROID
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace VoxelBusters.CoreLibrary.NativePlugins.Android
{
    public static class JNIUtility
    {
        public delegate T NativeJavaObjectConverter<T>(AndroidJavaObject nativeObject);

        public static List<T> GetList<T>(this AndroidJavaObject nativeObject, NativeJavaObjectConverter<T> converter)
        {
            if (nativeObject == null)
                return null;

            int size = nativeObject.Call<int>("size");

            List<T> list = new List<T>();
            for (int eachIndex = 0; eachIndex < size; eachIndex++)
            {
                AndroidJNI.PushLocalFrame(128);
                using (AndroidJavaObject eachNativeObject = nativeObject.Call<AndroidJavaObject>("get", eachIndex))
                {
                    T newObject = converter(eachNativeObject);
                    list.Add(newObject);
                }
                AndroidJNI.PopLocalFrame(IntPtr.Zero);
            }

            return list;
        }


        public static List<T> GetList<T>(this AndroidJavaObject nativeObject)
        {
            if (nativeObject == null)
                return null;
                
            T[] array = AndroidJNIHelper.ConvertFromJNIArray<T[]>(nativeObject.GetRawObject());
            return new List<T>(array);
        }

        public static string GetString(this AndroidJavaObject javaObject)
        {
            if (javaObject == null)
                return null;

            return javaObject.Call<string>("toString");
        }

        public static T Get<T>(this AndroidJavaObject nativeObject, string getterName)
        {
            if (nativeObject == null)
                return default(T);

            T value = nativeObject.Call<T>(getterName);
            return value;
        }

        public static T GetArray<T>(this AndroidJavaObject nativeObject)
        {

            if (nativeObject == null)
                return default(T);

            T value = AndroidJNIHelper.ConvertFromJNIArray<T>(nativeObject.GetRawObject());
            return value;
        }

        public static T GetEnum<T>(this AndroidJavaObject nativeObject) where T:struct
        {
            int value = nativeObject.Call<int>("ordinal");
            return (T)(object)value;
        }

        public static Color GetColor(this AndroidJavaObject nativeObject)
        {
            float red       = nativeObject.Call<int>("getRed") / 255;
            float green     = nativeObject.Call<int>("getRed") / 255;
            float blue      = nativeObject.Call<int>("getRed") / 255;
            float alpha     = nativeObject.Call<int>("alpha")  / 255.0f;

            return new Color(red, green, blue, alpha);
        }
    }
}
#endif