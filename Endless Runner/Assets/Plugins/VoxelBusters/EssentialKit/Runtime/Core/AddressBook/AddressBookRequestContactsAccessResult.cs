using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.EssentialKit
{
    /// <summary>
    /// This class contains the information retrieved when <see cref="AddressBook.RequestContactsAccess(bool, EventCallback{AddressBookRequestContactsAccessResult})"/> operation is completed.
    /// </summary>
    public class AddressBookRequestContactsAccessResult
    {
        #region Properties

        /// <summary>
        /// Returns the permission granted to access address book.
        /// </summary>
        public AddressBookContactsAccessStatus AccessStatus { get; private set; }

        #endregion

        #region Constrcutors

        internal AddressBookRequestContactsAccessResult(AddressBookContactsAccessStatus accessStatus)
        {
            // Set properties
            AccessStatus    = accessStatus;
        }

        #endregion
    }
}
