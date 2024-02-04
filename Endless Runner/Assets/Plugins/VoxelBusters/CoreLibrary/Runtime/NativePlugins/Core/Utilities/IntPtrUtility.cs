using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace VoxelBusters.CoreLibrary.NativePlugins
{
    internal static class IntPtrUtility
    {
        public static string AsString(this IntPtr ptr)
        {
            return MarshalUtility.ToString(ptr);
        }

        public static T AsStruct<T>(this IntPtr ptr) where T : struct
        {
            return MarshalUtility.PtrToStructure<T>(ptr);
        }
    }
}