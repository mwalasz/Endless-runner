using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.EssentialKit
{
    /// <summary>
    /// An access status the user can grant for an app to access the contacts information.
    /// </summary>
    public enum AddressBookContactsAccessStatus
    {
        /// <summary> The user has not yet made a choice regarding whether this app can access the address book data. </summary>
        NotDetermined,

        /// <summary> The application is not authorized to access the address book data. </summary>
        Restricted,

        /// <summary> The user explicitly denied access to address book data for this application. </summary>
        Denied,

        /// <summary> The application is authorized to access address book data. </summary>
        Authorized
    }
}