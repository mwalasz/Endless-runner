using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit.AddressBookCore
{
    internal class NullAddressBookInterface : NativeAddressBookInterfaceBase, INativeAddressBookInterface
    {
        #region Constructors

        public NullAddressBookInterface()
            : base(isAvailable: false)
        { }

        #endregion

        #region Private static methods

        private static void LogNotSupported()
        {
            Diagnostics.LogNotSupported("AddressBook");
        }

        #endregion

        #region Base class methods

        public override AddressBookContactsAccessStatus GetContactsAccessStatus()
        {
            LogNotSupported();

            return AddressBookContactsAccessStatus.Restricted;
        }

        public override void RequestContactsAccess(RequestContactsAccessInternalCallback callback)
        {
            LogNotSupported();

            callback(AddressBookContactsAccessStatus.Restricted, Diagnostics.kFeatureNotSupported);
        }

        public override void ReadContacts(ReadContactsInternalCallback callback)
        {
            LogNotSupported();

            callback(null, Diagnostics.kFeatureNotSupported);
        }

        #endregion
    }
}