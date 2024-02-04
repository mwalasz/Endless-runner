#if UNITY_ANDROID
using UnityEngine;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.CoreLibrary.NativePlugins.Android
{
    public class NativeRect : NativeAndroidJavaObjectWrapper
    {
        #region Static properties

         private static AndroidJavaClass m_nativeClass;

        #endregion
        #region Public properties

        public int Bottom
        {
            get
            {
                return Get<int>("bottom");
            }

            set
            {
                Set<int>("bottom", value);
            }
        }


        public int Left
        {
            get
            {
                return Get<int>("left");
            }

            set
            {
                Set<int>("left", value);
            }
        }


        public int Right
        {
            get
            {
                return Get<int>("right");
            }

            set
            {
                Set<int>("right", value);
            }
        }


        public int Top
        {
            get
            {
                return Get<int>("top");
            }

            set
            {
                Set<int>("top", value);
            }
        }


        public const int CONTENTS_FILE_DESCRIPTOR = 1;

        public const int PARCELABLE_WRITE_RETURN_VALUE = 1;

        #endregion

        #region Constructor

        // Wrapper constructors
        public NativeRect(AndroidJavaObject androidJavaObject) : base(Native.kClassName, androidJavaObject)
        {
        }

        public NativeRect(NativeAndroidJavaObjectWrapper wrapper) : base(wrapper)
        {
        }

        public NativeRect() : base(Native.kClassName)
        {
        }

#if NATIVE_PLUGINS_DEBUG_ENABLED
        ~NativeRect()
        {
            DebugLogger.Log("Disposing NativeRect");
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
        public static bool Intersects(NativeRect arg0, NativeRect arg1)
        {
            return GetClass().CallStatic<bool>(Native.Method.kIntersects, arg0.NativeObject, arg1.NativeObject);
        }
        public static NativeRect UnflattenFromString(string arg0)
        {
            AndroidJavaObject nativeObj = GetClass().CallStatic<AndroidJavaObject>(Native.Method.kUnflattenFromString, arg0);
            if(nativeObj != null)
            {
                NativeRect data  = new  NativeRect(nativeObj);
                return data;
            }
            else
            {
                return default(NativeRect);
            }
        }

        #endregion
        #region Public methods

        public int CenterX()
        {
            return Call<int>(Native.Method.kCenterX);
        }
        public int CenterY()
        {
            return Call<int>(Native.Method.kCenterY);
        }
        public bool Contains(int arg0, int arg1)
        {
            return Call<bool>(Native.Method.kContains, arg0, arg1);
        }
        public bool Contains(int arg0, int arg1, int arg2, int arg3)
        {
            return Call<bool>(Native.Method.kContains, arg0, arg1, arg2, arg3);
        }
        public bool Contains(NativeRect arg0)
        {
            return Call<bool>(Native.Method.kContains, arg0.NativeObject);
        }
        public int DescribeContents()
        {
            return Call<int>(Native.Method.kDescribeContents);
        }
        public bool Equals(NativeObject arg0)
        {
            return Call<bool>(Native.Method.kEquals, arg0.NativeObject);
        }
        public float ExactCenterX()
        {
            return Call<float>(Native.Method.kExactCenterX);
        }
        public float ExactCenterY()
        {
            return Call<float>(Native.Method.kExactCenterY);
        }
        public string FlattenToString()
        {
            return Call<string>(Native.Method.kFlattenToString);
        }
        public int HashCode()
        {
            return Call<int>(Native.Method.kHashCode);
        }
        public int Height()
        {
            return Call<int>(Native.Method.kHeight);
        }
        public void Inset(int arg0, int arg1)
        {
            Call(Native.Method.kInset, arg0, arg1);
        }
        public bool Intersect(NativeRect arg0)
        {
            return Call<bool>(Native.Method.kIntersect, arg0.NativeObject);
        }
        public bool Intersect(int arg0, int arg1, int arg2, int arg3)
        {
            return Call<bool>(Native.Method.kIntersect, arg0, arg1, arg2, arg3);
        }
        public bool Intersects(int arg0, int arg1, int arg2, int arg3)
        {
            return Call<bool>(Native.Method.kIntersects, arg0, arg1, arg2, arg3);
        }
        public bool IsEmpty()
        {
            return Call<bool>(Native.Method.kIsEmpty);
        }
        public void Offset(int arg0, int arg1)
        {
            Call(Native.Method.kOffset, arg0, arg1);
        }
        public void OffsetTo(int arg0, int arg1)
        {
            Call(Native.Method.kOffsetTo, arg0, arg1);
        }
        public void ReadFromParcel(NativeParcel arg0)
        {
            Call(Native.Method.kReadFromParcel, arg0.NativeObject);
        }
        public void Set(int arg0, int arg1, int arg2, int arg3)
        {
            Call(Native.Method.kSet, arg0, arg1, arg2, arg3);
        }
        public void Set(NativeRect arg0)
        {
            Call(Native.Method.kSet, arg0.NativeObject);
        }
        public void SetEmpty()
        {
            Call(Native.Method.kSetEmpty);
        }
        public bool SetIntersect(NativeRect arg0, NativeRect arg1)
        {
            return Call<bool>(Native.Method.kSetIntersect, arg0.NativeObject, arg1.NativeObject);
        }
        public void Sort()
        {
            Call(Native.Method.kSort);
        }
        public string ToShortString()
        {
            return Call<string>(Native.Method.kToShortString);
        }
        public new string ToString()
        {
            return Call<string>(Native.Method.kToString);
        }
        public void Union(int arg0, int arg1, int arg2, int arg3)
        {
            Call(Native.Method.kUnion, arg0, arg1, arg2, arg3);
        }
        public void Union(NativeRect arg0)
        {
            Call(Native.Method.kUnion, arg0.NativeObject);
        }
        public void Union(int arg0, int arg1)
        {
            Call(Native.Method.kUnion, arg0, arg1);
        }
        public int Width()
        {
            return Call<int>(Native.Method.kWidth);
        }
        public void WriteToParcel(NativeParcel arg0, int arg1)
        {
            Call(Native.Method.kWriteToParcel, arg0.NativeObject, arg1);
        }

        #endregion

        internal class Native
        {
            internal const string kClassName = "android.graphics.Rect";

            internal class Method
            {
                internal const string kToString = "toString";
                internal const string kContains = "contains";
                internal const string kHashCode = "hashCode";
                internal const string kSetEmpty = "setEmpty";
                internal const string kOffsetTo = "offsetTo";
                internal const string kExactCenterX = "exactCenterX";
                internal const string kExactCenterY = "exactCenterY";
                internal const string kSetIntersect = "setIntersect";
                internal const string kWriteToParcel = "writeToParcel";
                internal const string kToShortString = "toShortString";
                internal const string kUnflattenFromString = "unflattenFromString";
                internal const string kIsEmpty = "isEmpty";
                internal const string kCenterX = "centerX";
                internal const string kCenterY = "centerY";
                internal const string kIntersects = "intersects";
                internal const string kIntersect = "intersect";
                internal const string kReadFromParcel = "readFromParcel";
                internal const string kFlattenToString = "flattenToString";
                internal const string kHeight = "height";
                internal const string kOffset = "offset";
                internal const string kEquals = "equals";
                internal const string kDescribeContents = "describeContents";
                internal const string kWidth = "width";
                internal const string kUnion = "union";
                internal const string kInset = "inset";
                internal const string kSort = "sort";
                internal const string kSet = "set";
            }

        }
    }
}
#endif