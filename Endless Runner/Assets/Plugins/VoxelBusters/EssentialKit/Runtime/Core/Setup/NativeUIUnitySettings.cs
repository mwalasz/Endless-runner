using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.EssentialKit
{
    [Serializable]
    public partial class NativeUIUnitySettings : SettingsPropertyGroup
    {
        #region Properties

        [SerializeField]
        [Tooltip("Custom assets references.")]
        private     UnityUICollection       m_customUICollection;

        #endregion

        #region Properties

        public UnityUICollection CustomUICollection => m_customUICollection;

        #endregion

        #region Constructors

        public NativeUIUnitySettings(bool isEnabled = true, UnityUICollection customUICollection = null)
            : base(isEnabled: isEnabled, name: NativeFeatureType.kNativeUI)
        {
            // set properties
            m_customUICollection    = customUICollection ?? new UnityUICollection();
        }

        #endregion
    }
}