#if UNITY_IOS || UNITY_TVOS
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.EssentialKit.AddressBookCore.iOS
{
	internal delegate void RequestContactsAccessNativeCallback(CNAuthorizationStatus status, string error, IntPtr tagPtr);

    internal delegate void ReadContactsNativeCallback(IntPtr contactsPtr, int count, string error, IntPtr tagPtr);
}
#endif