using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace VoxelBusters.CoreLibrary.NativePlugins
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct UnityCircularRegion
    {
        #region Properties

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public float Radius { get; set; }

        public IntPtr RegionIdPtr { get; set; }

        #endregion

        #region Operator methods

        public static implicit operator UnityCircularRegion(CircularRegion circularRegion)
        {
            return new UnityCircularRegion()
            {
                Latitude        = circularRegion.Center.Latitude,
                Longitude       = circularRegion.Center.Longitude,
                Radius          = circularRegion.Radius,
                RegionIdPtr     = circularRegion.RegionId == null ? IntPtr.Zero : Marshal.StringToHGlobalAuto(circularRegion.RegionId),
            };
        }

        public static implicit operator CircularRegion(UnityCircularRegion circularRegion)
        {
            return new CircularRegion()
            {
                Center          = new LocationCoordinate() { Latitude = circularRegion.Latitude, Longitude = circularRegion.Longitude },
                Radius          = circularRegion.Radius,
                RegionId        = MarshalUtility.ToString(circularRegion.RegionIdPtr),
            };
        }

        #endregion
    }
}