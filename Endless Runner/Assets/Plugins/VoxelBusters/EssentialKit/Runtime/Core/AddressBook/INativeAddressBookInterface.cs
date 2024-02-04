using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit.AddressBookCore
{
    public interface INativeAddressBookInterface : INativeFeatureInterface
    {
        #region Methods

        AddressBookContactsAccessStatus GetContactsAccessStatus();
        
        void RequestContactsAccess(RequestContactsAccessInternalCallback callback);    
        
        void ReadContacts(ReadContactsInternalCallback callback);

        #endregion
    }
}