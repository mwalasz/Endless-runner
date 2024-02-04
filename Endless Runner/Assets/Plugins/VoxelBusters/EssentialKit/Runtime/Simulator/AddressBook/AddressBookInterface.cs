using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace VoxelBusters.EssentialKit.AddressBookCore.Simulator
{
    public sealed class AddressBookInterface : NativeAddressBookInterfaceBase, INativeAddressBookInterface
    {
        #region Constructors

        public AddressBookInterface()
            : base(isAvailable: true)
        { }

        #endregion

        #region Base methods

        public override AddressBookContactsAccessStatus GetContactsAccessStatus()
        {
            return AddressBookSimulator.Instance.GetContactsAccessStatus();
        }

        public override void RequestContactsAccess(RequestContactsAccessInternalCallback callback)
        {
            AddressBookSimulator.Instance.RequestContactsAccess((accessStatus, error) => callback(accessStatus, error));
        }

        public override void ReadContacts(ReadContactsInternalCallback callback)
        {
            AddressBookSimulator.Instance.ReadContacts((contacts, error) => callback(contacts, error));
        }

        #endregion
    }
}