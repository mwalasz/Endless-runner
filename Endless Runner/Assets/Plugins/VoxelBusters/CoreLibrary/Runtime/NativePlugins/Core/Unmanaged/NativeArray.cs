using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace VoxelBusters.CoreLibrary.NativePlugins
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct NativeArray
    {
        #region Properties

        public IntPtr Pointer { get; set; }

        public int Length { get; set; }

        #endregion

        #region Public methods

        public T[] GetStructArray<T>() where T : struct
        {
            if (Pointer == IntPtr.Zero)
            {
                return null;
            }

            T[] structArray = new T[Length];

            // copy data to managed array
            var managedArray = new IntPtr[Length];
            Marshal.Copy(Pointer, managedArray, 0, Length);

            for (int i = 0; i < Length; i++)
            {
                structArray[i] = MarshalUtility.PtrToStructure<T>(managedArray[i]);
            }
            return structArray;
        }

        public string[] GetStringArray()
        {
            // Marshal ptr to array
            return MarshalUtility.CreateStringArray(Pointer, Length);
        }

        #endregion
    }
}