#if UNITY_IOS || UNITY_TVOS
using System;
using VoxelBusters.CoreLibrary.NativePlugins;
using VoxelBusters.CoreLibrary.NativePlugins.iOS;
using VoxelBusters.EssentialKit;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.EssentialKit.AddressBookCore.iOS
{
    internal sealed class AddressBookContact : AddressBookContactBase, IAddressBookContact
    {
        #region Fields

        private     string                  m_firstName;

        private     string                  m_middleName;

        private     string                  m_lastName;

        private     string[]                m_phoneNumbers;

        private     string[]                m_emailAddresses;

        private     IosNativeObjectRef      m_image;

        #endregion

        #region Constructors

        public AddressBookContact(NativeAddressBookContactData data)
        {
            // set properties
            m_firstName             = MarshalUtility.ToString(data.FirstNamePtr);
            m_middleName            = MarshalUtility.ToString(data.MiddleNamePtr);
            m_lastName              = MarshalUtility.ToString(data.LastNamePtr);
            m_phoneNumbers          = MarshalUtility.CreateStringArray(data.PhoneNumbersPtr, data.PhoneNumbersCount);
            m_emailAddresses        = MarshalUtility.CreateStringArray(data.EmailAddressesPtr, data.EmailAddressesCount);
            m_image                 = (data.ImageDataPtr != IntPtr.Zero) ? new IosNativeObjectRef(data.ImageDataPtr) : null;
        }

        ~AddressBookContact()
        {
            Dispose(false);
        }

        #endregion

        #region Base implementation

        protected override string GetFirstNameInternal()
        {
            return m_firstName;
        }

        protected override string GetMiddleNameInternal()
        {
            return m_middleName;
        }

        protected override string GetLastNameInternal() 
        {
            return m_lastName;
        }

        protected override string[] GetPhoneNumbersInternal()
        {
            return m_phoneNumbers;
        }

        protected override string[] GetEmailAddressesInternal()
        {
            return m_emailAddresses;
        }

        protected override void LoadImageInternal(LoadImageInternalCallback callback)
        {
            // check whether image exists
            if (m_image == null)
            {
                callback(null, new Error(description: "Image not found."));
                return;
            }

            // fetch actual data
            IosNativePluginsUtility.LoadImage(m_image.Pointer, (imageData, error) => callback(imageData, error));
        }

        protected override void Dispose(bool disposing)
        {
            // check whether object is released
            if (IsDisposed)
            {
                return;
            }

            // release native reference
            if (m_image != null)
            {
                m_image.Dispose();
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}
#endif