using System;
using UnityEngine;

namespace VoxelBusters.CoreLibrary.Editor.NativePlugins.Build
{
    [Serializable]
    public class PBXCapability
    {
        #region Fields

        [SerializeField]
        private     PBXCapabilityType                   m_type;   
        
        [SerializeField]
        private     PBXAssociatedDomainsEntitlement     m_associatedDomainsEntitlement;

        #endregion

        #region Properties

        public PBXCapabilityType Type => m_type;

        public PBXAssociatedDomainsEntitlement AssociatedDomainsEntitlement => m_associatedDomainsEntitlement;

        #endregion

        #region Create methods

        public static PBXCapability iCloud()
        {
            return new PBXCapability()
            {
                m_type  = PBXCapabilityType.iCloud,
            };
        }

        public static PBXCapability InAppPurchase()
        {
            return new PBXCapability()
            {
                m_type  = PBXCapabilityType.InAppPurchase,
            };
        }

        public static PBXCapability GameCenter()
        {
            return new PBXCapability()
            {
                m_type  = PBXCapabilityType.GameCenter,
            };
        }

        public static PBXCapability PushNotifications()
        {
            return new PBXCapability()
            {
                m_type  = PBXCapabilityType.PushNotifications,
            };
        }

        public static PBXCapability AssociatedDomains(string[] domains)
        {
            return new PBXCapability()
            {
                m_type                          = PBXCapabilityType.AssociatedDomains,
                m_associatedDomainsEntitlement  = new PBXAssociatedDomainsEntitlement(domains),
            };
        }

        #endregion
    }
}