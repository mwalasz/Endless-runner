#if UNITY_ANDROID
using UnityEngine;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.CoreLibrary.NativePlugins.Android
{
    public class NativePointF : NativeAndroidJavaObjectWrapper
    {
        #region Static properties

         private static AndroidJavaClass m_nativeClass;

        #endregion
        #region Public properties

        public float X
        {
            get
            {
                return Get<float>("x");
            }

            set
            {
                Set<float>("x", value);
            }
        }


        public float Y
        {
            get
            {
                return Get<float>("y");
            }

            set
            {
                Set<float>("y", value);
            }
        }


        public const int CONTENTS_FILE_DESCRIPTOR = 1;

        public const int PARCELABLE_WRITE_RETURN_VALUE = 1;

        #endregion

        #region Constructor

        // Wrapper constructors
        public NativePointF(AndroidJavaObject androidJavaObject) : base(Native.kClassName, androidJavaObject)
        {
        }

        public NativePointF(NativeAndroidJavaObjectWrapper wrapper) : base(wrapper)
        {
        }

        public NativePointF() : base(Native.kClassName)
        {
        }

#if NATIVE_PLUGINS_DEBUG_ENABLED
        ~NativePointF()
        {
            DebugLogger.Log("Disposing NativePointF");
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
        public static float Length(float arg0, float arg1)
        {
            return GetClass().CallStatic<float>(Native.Method.kLength, arg0, arg1);
        }

        #endregion
        #region Public methods

        public int DescribeContents()
        {
            return Call<int>(Native.Method.kDescribeContents);
        }
        public bool Equals(NativeObject arg0)
        {
            return Call<bool>(Native.Method.kEquals, arg0.NativeObject);
        }
        public int HashCode()
        {
            return Call<int>(Native.Method.kHashCode);
        }
        public void ReadFromParcel(NativeParcel arg0)
        {
            Call(Native.Method.kReadFromParcel, arg0.NativeObject);
        }
        public new string ToString()
        {
            return Call<string>(Native.Method.kToString);
        }
        public void WriteToParcel(NativeParcel arg0, int arg1)
        {
            Call(Native.Method.kWriteToParcel, arg0.NativeObject, arg1);
        }

        #endregion

        internal class Native
        {
            internal const string kClassName = "android.graphics.PointF";

            internal class Method
            {
                internal const string kToString = "toString";
                internal const string kHashCode = "hashCode";
                internal const string kWriteToParcel = "writeToParcel";
                internal const string kReadFromParcel = "readFromParcel";
                internal const string kLength = "length";
                internal const string kEquals = "equals";
                internal const string kDescribeContents = "describeContents";
            }

        }
    }
}
#endif