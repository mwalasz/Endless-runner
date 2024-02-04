using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.CoreLibrary.NativePlugins
{
    internal static class MarshalUtility
    {
        #region Marshalling methods

        public static string ToString(IntPtr stringPtr)
        {
            if (IntPtr.Zero == stringPtr)
            {
                return null;
            }

            return Marshal.PtrToStringAuto(stringPtr);
        }

        public static IntPtr GetIntPtr(object value)
        {
            if (null == value)
            {
                return IntPtr.Zero;
            }

            return GCHandle.ToIntPtr(value: GCHandle.Alloc(value));
        }

        public static void FreeUnmanagedStringArray(IntPtr unmanagedArrayPtr, int count)
        {
            DebugLogger.Log(CoreLibraryDomain.NativePlugins, $"Releasing unmanaged array: {unmanagedArrayPtr}.");

            // release each strings allocated in unmanaged space
            var     unmanagedArrayHandle    = GCHandle.FromIntPtr(unmanagedArrayPtr);
            var     dataArray               = (IntPtr[])unmanagedArrayHandle.Target;
            for (int iter = 0; iter < count; iter++)
            {
                Marshal.FreeHGlobal(dataArray[iter]);
            }

            // release handle
            unmanagedArrayHandle.Free();
        }

        public static IntPtr CreateUnmanagedArray(IntPtr[] managedArray)
        {
            int     arrayLength     = managedArray.Length;
            int     size            = Marshal.SizeOf(managedArray[0]) * arrayLength;
            var     unmanagedPtr    = Marshal.AllocHGlobal(size);

            // copy
            Marshal.Copy(managedArray, 0, unmanagedPtr, arrayLength);

            return unmanagedPtr;
        }

        public static void ReleaseUnmanagedArray(IntPtr unmanagedArrayPtr)
        {
            Marshal.FreeHGlobal(unmanagedArrayPtr);
        }

        public static IntPtr[] CreateManagedArray(IntPtr arrayPtr, int length)
        {
            // check whether array is valid
            if (arrayPtr == IntPtr.Zero)
            {
                return null;
            }

            // copy data to managed array
            var     managedArray    = new IntPtr[length];
            Marshal.Copy(arrayPtr, managedArray, 0, length);

            return managedArray;
        }

        public static string[] CreateStringArray(IntPtr arrayPtr, int length)
        {
            if (IntPtr.Zero == arrayPtr)
            {
                return null;
            }

            // create array
            var     managedArray    = new IntPtr[length];
            Marshal.Copy(arrayPtr, managedArray, 0, length);

            var     stringArray     = new string[length];
            for (int iter = 0; iter < length; iter++)
            {
                stringArray[iter]   = ToString(managedArray[iter]);
            }

            return stringArray;
        }

        #endregion

        #region Structures

        public static T PtrToStructure<T>(IntPtr ptr) where T : struct
        {
            return (T)Marshal.PtrToStructure(ptr, typeof(T));
        }

        public static TOutput[] ConvertNativeArrayItems<TInput, TOutput>(IntPtr arrayPtr, int length, Converter<TInput, TOutput> converter, bool includeNullObjects) 
            where TInput : struct
            where TOutput : class 
        {
            if (IntPtr.Zero == arrayPtr)
            {
                return null;
            }
           
            // create original data array from native data
            var     outputObjects           = new List<TOutput>(length);
            int     sizeOfInputObject       = Marshal.SizeOf(typeof(TInput));
            long    arrayPtrAddr            = arrayPtr.ToInt64();
            int     offset                  = 0;
            for (int iter = 0; iter < length; iter++)
            {
                var     inputObject         = PtrToStructure<TInput>(new IntPtr(arrayPtrAddr + offset));
                var     outputObject        = converter(inputObject);
                if (EqualityComparer<TOutput>.Default.Equals(outputObject, default(TOutput)) && !includeNullObjects)
                {
                    DebugLogger.LogWarning(CoreLibraryDomain.NativePlugins, $"Failed to convert object with data {inputObject}.");
                    continue;
                }

                // add object to list
                outputObjects.Add(outputObject);

                // update pointer
                offset  += sizeOfInputObject;
            }

            return outputObjects.ToArray();
        }

        public static TOutput[] ConvertNativeArrayItems<TOutput>(IntPtr arrayPtr, int length, Converter<IntPtr, TOutput> converter, bool includeNullObjects)
            where TOutput : class 
        {
            // check whether array is valid
            if (arrayPtr == IntPtr.Zero)
            {
                return null;
            }

            // copy data to managed array
            var     managedArray        = new IntPtr[length];
            Marshal.Copy(arrayPtr, managedArray, 0, length);

            // convert items to specified type using converter method
            var     outputObjects       = new List<TOutput>(length);
            for (int iter = 0; iter < length; iter++)
            {
                var     nativePtr       = managedArray[iter];
                var     outputObject    = converter(nativePtr);
                if (EqualityComparer<TOutput>.Default.Equals(outputObject, default(TOutput)) && !includeNullObjects)
                {
                    DebugLogger.LogWarning(CoreLibraryDomain.NativePlugins, $"Failed to convert object with data {nativePtr}.");
                    continue;
                }

                // add object to list
                outputObjects.Add(outputObject);
            }

            return outputObjects.ToArray();
        }

        #endregion
    }
}