using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace VoxelBusters.CoreLibrary.NativePlugins
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct NativeSize
    {
        #region Properties

        public float Width { get; set; }

        public float Height { get; set; }

        #endregion

        #region Constructors

        public NativeSize(float width, float height)
        {
            Width = width;
            Height = height;
        }

        #endregion
    }
}
