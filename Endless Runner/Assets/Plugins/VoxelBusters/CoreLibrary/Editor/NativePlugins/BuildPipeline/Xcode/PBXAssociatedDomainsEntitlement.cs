using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.CoreLibrary.Editor.NativePlugins.Build
{
    [Serializable]
    public class PBXAssociatedDomainsEntitlement
    {
        #region Fields
        
        [SerializeField]
        private     string[]        m_domains;

        #endregion

        #region Properties

        public string[] Domains => m_domains;

        #endregion

        #region Constructors

        public PBXAssociatedDomainsEntitlement(string[] domains)
        {
            // set properties
            m_domains   = domains;
        }

        #endregion
    }
}