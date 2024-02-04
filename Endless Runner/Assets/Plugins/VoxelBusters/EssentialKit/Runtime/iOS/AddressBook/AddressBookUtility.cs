#if UNITY_IOS || UNITY_TVOS
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit.AddressBookCore.iOS
{
    internal static class AddressBookUtility
    {
        #region Converter methods

        public static AddressBookContactsAccessStatus ConvertToAddressBookContactsAccessStatus(CNAuthorizationStatus status)
        {
            switch (status)
            {
                case CNAuthorizationStatus.CNAuthorizationStatusNotDetermined:
                    return AddressBookContactsAccessStatus.NotDetermined;

                case CNAuthorizationStatus.CNAuthorizationStatusRestricted:
                    return AddressBookContactsAccessStatus.Restricted;

                case CNAuthorizationStatus.CNAuthorizationStatusDenied:
                    return AddressBookContactsAccessStatus.Denied;

                case CNAuthorizationStatus.CNAuthorizationStatusAuthorized:
                    return AddressBookContactsAccessStatus.Authorized;

                default:
                    throw VBException.SwitchCaseNotImplemented(status);
            }
        }

        public static AddressBookContact[] ConvertNativeDataArrayToContactsArray(IntPtr contactsPtr, int length)
        {
            if (IntPtr.Zero == contactsPtr)
            {
                return null;
            }
           
            // create original data array from native data
            AddressBookContact[]    contacts            = new AddressBookContact[length];
            int                     sizeOfNativeData    = Marshal.SizeOf(typeof(NativeAddressBookContactData));
            int                     offset              = 0;
            for (int iter = 0; iter < length; iter++)
            {
                NativeAddressBookContactData    unmanagedItem       = MarshalUtility.PtrToStructure<NativeAddressBookContactData>(new IntPtr(contactsPtr.ToInt64() + offset));
                contacts[iter]                                      = new AddressBookContact(unmanagedItem);

                // move pointer
                offset  += sizeOfNativeData;
            }
            return contacts;
        }

        #endregion
    }
}
#endif