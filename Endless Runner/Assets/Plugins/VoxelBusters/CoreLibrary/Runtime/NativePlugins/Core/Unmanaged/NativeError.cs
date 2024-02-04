using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace VoxelBusters.CoreLibrary.NativePlugins
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct NativeError
    {
        #region Properties

        public int Code { get; set; }

        public string Description { get; set; }

        #endregion

        #region Constructors

        public NativeError(int code, IntPtr description)
        {
            Code = code;
            Description = MarshalUtility.ToString(description);
        }

        #endregion
    }
}
