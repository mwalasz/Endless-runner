using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit.AddressBookCore
{
    public abstract class NativeAddressBookInterfaceBase : NativeFeatureInterfaceBase, INativeAddressBookInterface
    {
        #region Constructors

        protected NativeAddressBookInterfaceBase(bool isAvailable)
            : base(isAvailable)
        { }

        #endregion

        #region INativeAddressBookInterface implementation

        public abstract AddressBookContactsAccessStatus GetContactsAccessStatus();

        public abstract void RequestContactsAccess(RequestContactsAccessInternalCallback callback);    
        
        public abstract void ReadContacts(ReadContactsInternalCallback callback);

        #endregion
    }
}