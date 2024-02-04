using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.EssentialKit
{
    [Serializable]
    public class UtilityUnitySettings : SettingsPropertyGroup
    {

        #region Fields

        [SerializeField]
        private bool m_usesStoreRatingUtility;

        #endregion


        #region Properties

        public bool UsesStoreRatingUtility => m_usesStoreRatingUtility;

        #endregion

        #region Constructors

        public UtilityUnitySettings(bool isEnabled = true, bool usesStoreRatingUtility = true)
            : base(isEnabled: isEnabled, name: NativeFeatureType.kExtras)
        {
            m_usesStoreRatingUtility = usesStoreRatingUtility;
        }

        #endregion
    }
}