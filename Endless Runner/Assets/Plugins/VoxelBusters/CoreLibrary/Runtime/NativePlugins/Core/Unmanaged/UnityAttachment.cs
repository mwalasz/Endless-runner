using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.InteropServices;

namespace VoxelBusters.CoreLibrary.NativePlugins
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct UnityAttachment
    {
        #region Properties

        public int DataArrayLength { get; set; }

        public IntPtr DataArrayPtr { get; set; }

        public IntPtr MimeTypePtr { get; set; }

        public IntPtr FileNamePtr { get; set; }

        #endregion
    }
}