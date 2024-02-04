#if UNITY_IOS || UNITY_TVOS
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using AOT;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit.AddressBookCore.iOS
{
    public sealed class AddressBookInterface : NativeAddressBookInterfaceBase, INativeAddressBookInterface
    {
        #region Constructors

        static AddressBookInterface()
        {
            AddressBookBinding.NPAddressBookRegisterCallbacks(requestContactsAccessCallback: HandleRequestContactsAccessCallbackInternal, readContactsCallback: HandleReadContactsCallbackInternal);
        }

        public AddressBookInterface()
            : base(isAvailable: true)
        { }

        #endregion

        #region Base class methods

        public override AddressBookContactsAccessStatus GetContactsAccessStatus()
        {
            var     authorizationStatus     = AddressBookBinding.NPAddressBookGetAuthorizationStatus();
            var     accessStatus            = AddressBookUtility.ConvertToAddressBookContactsAccessStatus(authorizationStatus);

            return accessStatus;
        }

        public override void RequestContactsAccess(RequestContactsAccessInternalCallback callback)
        {
            // make call
            var     tagPtr  = MarshalUtility.GetIntPtr(callback);
            AddressBookBinding.NPAddressBookRequestContactsAccess(tagPtr);
        }

        public override void ReadContacts(ReadContactsInternalCallback callback)
        {
            // make call
            var     tagPtr  = MarshalUtility.GetIntPtr(callback);
            AddressBookBinding.NPAddressBookReadContacts(tagPtr);
        }

        #endregion

        #region Native callback methods

        [MonoPInvokeCallback(typeof(RequestContactsAccessNativeCallback))]
        private static void HandleRequestContactsAccessCallbackInternal(CNAuthorizationStatus status, string error, IntPtr tagPtr)
        {
            var     tagHandle       = GCHandle.FromIntPtr(tagPtr);

            try
            {
                // send result
                var     accessStatus    = AddressBookUtility.ConvertToAddressBookContactsAccessStatus(status);
                var     errorObj        = Error.CreateNullableError(description: error);
                ((RequestContactsAccessInternalCallback)tagHandle.Target).Invoke(accessStatus, errorObj);
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(EssentialKitDomain.Default, exception);
            }
            finally
            {
                // release handle
                tagHandle.Free();
            }
        }

        [MonoPInvokeCallback(typeof(ReadContactsNativeCallback))]
        private static void HandleReadContactsCallbackInternal(IntPtr contactsPtr, int count, string error, IntPtr tagPtr)
        {
            var     tagHandle       = GCHandle.FromIntPtr(tagPtr);

            try
            {
                // send result
                var     contacts    = AddressBookUtility.ConvertNativeDataArrayToContactsArray(contactsPtr, count);
                var     errorObj    = Error.CreateNullableError(description: error);
                ((ReadContactsInternalCallback)tagHandle.Target).Invoke(contacts, errorObj);
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(EssentialKitDomain.Default, exception);
            }
            finally
            {
                // release handle
                tagHandle.Free();
            }
        }

        #endregion
    }
}
#endif