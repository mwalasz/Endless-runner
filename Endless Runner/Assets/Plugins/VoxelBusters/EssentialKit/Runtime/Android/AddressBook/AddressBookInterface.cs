#if UNITY_ANDROID
using System.Collections.Generic;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins.Android;

namespace VoxelBusters.EssentialKit.AddressBookCore.Android
{
    public sealed class AddressBookInterface : NativeAddressBookInterfaceBase
    {
#region Private fields

        private NativeAddressBook m_instance;

#endregion

#region Constructors

        public AddressBookInterface() : base(isAvailable: true)
        {

            m_instance = new NativeAddressBook(NativeUnityPluginUtility.GetContext());
        }

#endregion


#region Public methods

        public override AddressBookContactsAccessStatus GetContactsAccessStatus()
        {
            return m_instance.IsAuthorized() ? AddressBookContactsAccessStatus.Authorized : AddressBookContactsAccessStatus.Denied;
        }

        public override void ReadContacts(ReadContactsInternalCallback callback)
        {
            m_instance.ReadContacts(new NativeReadContactsListener()
            {
                onSuccessCallback = (NativeList<NativeAddressBookContact> nativeList) =>
                {
                    AddressBookContact[] contacts = NativeUnityPluginUtility.Map<NativeAddressBookContact, AddressBookContact>(nativeList.Get());
                    callback(contacts, null);
                },
                onFailureCallback = (string error) =>
                {
                    callback(null, new Error(error));
                }
            });
        }

        public override void RequestContactsAccess(RequestContactsAccessInternalCallback callback)
        {
            m_instance.RequestPermission(new NativeRequestContactsPermissionListener()
            {
                onSuccessCallback = () =>
                {
                    callback(AddressBookContactsAccessStatus.Authorized, null);
                },
                onFailureCallback = (string error) =>
                {
                    callback(AddressBookContactsAccessStatus.Denied, new Error(error));
                }
            });
        }

        #endregion
    }
}
#endif