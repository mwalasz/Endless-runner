using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.EssentialKit.AddressBookCore
{
    public delegate void RequestContactsAccessInternalCallback(AddressBookContactsAccessStatus accessStatus, Error error);

    public delegate void GetContactsAccessStatusInternalCallback(AddressBookContactsAccessStatus accessStatus);

    public delegate void ReadContactsInternalCallback(IAddressBookContact[] contacts, Error error);
}