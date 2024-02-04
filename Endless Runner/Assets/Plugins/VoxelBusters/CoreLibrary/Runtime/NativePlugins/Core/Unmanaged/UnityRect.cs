using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace VoxelBusters.CoreLibrary.NativePlugins
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct UnityRect
    {
        #region Properties

        public float X { get; set; }

        public float Y { get; set; }

        public float Width { get; set; }

        public float Height { get; set; }

        #endregion

        #region Operators

        public static implicit operator Rect(UnityRect nativeRect)
        {
            return new Rect(nativeRect.X, nativeRect.Y, nativeRect.Width, nativeRect.Height);
        }

        public static explicit operator UnityRect(Rect rect)
        {
            return new UnityRect()
            {
                X       = rect.x,
                Y       = rect.y,
                Width   = rect.width,
                Height  = rect.height,
            };
        }

        #endregion
    }
}