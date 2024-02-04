using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace VoxelBusters.CoreLibrary.NativePlugins
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct UnityColor
    {
        #region Properties

        public float Red { get; set; }

        public float Green { get; set; }

        public float Blue { get; set; }

        public float Alpha { get; set; }

        #endregion

        #region Operators

        public static implicit operator Color(UnityColor nativeColor)
        {
            return new Color(nativeColor.Red, nativeColor.Green, nativeColor.Blue, nativeColor.Alpha);
        }

        public static explicit operator UnityColor(Color color)
        {
            return new UnityColor()
            {
                Red     = color.r,
                Green   = color.g,
                Blue    = color.b,
                Alpha   = color.a,
            };
        }

        #endregion
    }
}
