using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.CoreLibrary.NativePlugins.UnityUI
{
    public class UnityUIRenderer : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private     int         m_displayOrder;

        #endregion

        #region Static properties

        public static UnityUIRenderer ActiveRenderer { get; set; }

        #endregion

        #region Properties

        public int DisplayOrder
        {
            get => m_displayOrder;
            set => m_displayOrder = value;
        }

        #endregion
    }
}