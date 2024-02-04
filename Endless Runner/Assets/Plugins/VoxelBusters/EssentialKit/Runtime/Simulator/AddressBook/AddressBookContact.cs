using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.EssentialKit.AddressBookCore.Simulator
{
    internal sealed class AddressBookContact : AddressBookContactBase, IAddressBookContact
    {
        #region Fields

        private     string      m_firstName;

        private     string      m_lastName;

        private     string      m_middleName;

        private     string[]    m_phoneNumbers;

        private     string[]    m_emailAddresses;

        #endregion

        #region Constructors

        public AddressBookContact(string firstName, string middleName = "", string lastName = "", string[] phoneNumbers = null, string[] emailAddresses = null)
        {
            // set properties
            m_firstName         = firstName;
            m_middleName        = middleName;
            m_lastName          = lastName;
            m_phoneNumbers      = phoneNumbers ?? new string[0];
            m_emailAddresses    = emailAddresses ?? new string[0];
        }

        ~AddressBookContact()
        {
            Dispose(false);
        }

        #endregion

        #region Base methods

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
            callback(null, new Error(description: "Not supported!"));
        }

        #endregion
    }
}