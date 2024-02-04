using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace VoxelBusters.EssentialKit.AddressBookCore
{
    [StructLayout(LayoutKind.Sequential)]
    public struct NativeAddressBookContactData
    {
        #region Properties

        public IntPtr NativeObjectPtr { get; private set; }

        public IntPtr FirstNamePtr { get; private set; }
        
        public IntPtr MiddleNamePtr { get; private set; }

        public IntPtr LastNamePtr { get; private set; }

        public IntPtr ImageDataPtr { get; private set; }

        public int PhoneNumbersCount { get; private set; }
        
        public IntPtr PhoneNumbersPtr { get; private set; }

        public int EmailAddressesCount { get; private set; }
        
        public IntPtr EmailAddressesPtr { get; private set; }

        #endregion
    }
}