#if UNITY_IOS || UNITY_TVOS
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace VoxelBusters.EssentialKit.AddressBookCore.iOS
{
    internal static class AddressBookBinding
    {
        [DllImport("__Internal")]
        public static extern void NPAddressBookRegisterCallbacks(RequestContactsAccessNativeCallback requestContactsAccessCallback, ReadContactsNativeCallback readContactsCallback);
        
        [DllImport("__Internal")]
        public static extern CNAuthorizationStatus NPAddressBookGetAuthorizationStatus();
        
        [DllImport("__Internal")]
        public static extern void NPAddressBookRequestContactsAccess(IntPtr tagPtr);
        
        [DllImport("__Internal")]
        public static extern void NPAddressBookReadContacts(IntPtr tagPtr);
        
        [DllImport("__Internal")]
        public static extern void NPAddressBookReset();
    }
}
#endif