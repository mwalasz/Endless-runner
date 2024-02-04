using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.EssentialKit
{
    [Serializable]
    public class AddressBookUnitySettings : SettingsPropertyGroup
    {
        #region Fields

        [SerializeField]
        [Tooltip("The default image used for contact.")]
        private         Texture2D           m_defaultImage;

        #endregion

        #region Properties

        public Texture2D DefaultImage => m_defaultImage;

        #endregion

        #region Constructors

        public AddressBookUnitySettings(bool isEnabled = true)
            : base(isEnabled: isEnabled, name: NativeFeatureType.kAddressBook)
        { 
            // set properties
            m_defaultImage      = null;
        }

        #endregion
    }
}