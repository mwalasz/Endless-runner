using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.EssentialKit
{
    public interface IAddressBookContactsEnumerator : IEnumerator
    {
        #region Properties

        int BlockSize { get; }

        int AvailableContactsCount { get; }

        #endregion

        #region Methods

        IAddressBookContact GetContact(int index);

        #endregion
    }
}