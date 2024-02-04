using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.EssentialKit
{
    public partial class NetworkServicesUnitySettings
    {
        [Serializable]
        public class Address
        {
            #region Fields

            [SerializeField, DefaultValue("8.8.8.8")]
            [Tooltip("IPV4 format address.")]
            private     string      m_ipv4;

            [SerializeField, DefaultValue("0:0:0:0:0:FFFF:0808:0808")]
            [Tooltip("IPV6 format address.")]
            private     string      m_ipv6;

            #endregion

            #region Properties

            public string IpV4 => PropertyHelper.GetValueOrDefault(
                    instance: this,
                    fieldAccess: (field) => field.m_ipv4,
                    value: m_ipv4);

            public string IpV6 => PropertyHelper.GetValueOrDefault(
                    instance: this,
                    fieldAccess: (field) => field.m_ipv6,
                    value: m_ipv6);

            #endregion

            #region Constructors

            public Address(string ipv4 = null, string ipv6 = null)
            {
                // set properties
                m_ipv4  = PropertyHelper.GetValueOrDefault(
                    instance: this,
                    fieldAccess: (field) => field.m_ipv4,
                    value: ipv4);
                m_ipv6  = PropertyHelper.GetValueOrDefault(
                    instance: this,
                    fieldAccess: (field) => field.m_ipv6,
                    value: ipv6);
            }

            #endregion
        }
    }
}