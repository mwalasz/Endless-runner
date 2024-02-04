using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.CoreLibrary.NativePlugins
{
    /// <summary>
    /// A circular geographic region, specified as a center point and radius.
    /// </summary>
    [Serializable]
    public struct CircularRegion
    {
        #region Fields

        [SerializeField]
        private         LocationCoordinate          m_center;
        
        [SerializeField]
        private         float                       m_radius;
        
        [SerializeField]
        private         string                      m_regionId;

        #endregion

        #region Properties

        /// <summary>
        /// The center point of the geographic region to monitor.
        /// </summary>
        public LocationCoordinate Center
        {
            get => m_center;
            set => m_center = value;
        }

        /// <summary>
        /// The distance (measured in meters) from the center point of the geographic region to the edge of the circular boundary.
        /// </summary>
        public float Radius
        {
            get => m_radius;
            set => m_radius = value;
        }

        /// <summary>
        /// The identifier for the region object.
        /// </summary>
        public string RegionId
        {
            get => m_regionId;
            set => m_regionId = value;
        }

        #endregion
    }
}