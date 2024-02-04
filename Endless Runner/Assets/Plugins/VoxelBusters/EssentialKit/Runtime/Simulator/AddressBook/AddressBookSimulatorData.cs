using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.EssentialKit.AddressBookCore.Simulator
{
    [Serializable]
    internal sealed class AddressBookSimulatorData 
    {
        #region Fields

        [SerializeField]
        private     AddressBookContactsAccessStatus     m_contactsAccessStatus      = AddressBookContactsAccessStatus.NotDetermined;

        #endregion

        #region Properties

        public AddressBookContactsAccessStatus ContactsAccessStatus
        {
            get
            {
                return m_contactsAccessStatus;
            }
            set
            {
                m_contactsAccessStatus  = value;
            }
        }

        #endregion
    }
}