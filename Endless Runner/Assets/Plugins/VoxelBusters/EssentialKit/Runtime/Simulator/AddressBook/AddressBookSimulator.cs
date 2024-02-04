using System.IO;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins;
using VoxelBusters.CoreLibrary.Editor.NativePlugins;

namespace VoxelBusters.EssentialKit.AddressBookCore.Simulator
{
    public sealed class AddressBookSimulator : SingletonObject<AddressBookSimulator>
    {
	    #region Constants

	    // messages
        private 	const 	    string    					kUnauthorizedAccessError    = "Unauthorized access! Check permission before accessing contacts database.";

        private 	const 	    string    					kAlreadyAuthorizedError     = "Permission for accessing contacts is already provided. Please check access status before requesting access.";

        #endregion

        #region Fields

        private     readonly    AddressBookContact[]        m_contacts			        = null;

        private     readonly    AddressBookSimulatorData    m_simulatorData             = null;

        #endregion

        #region Delegates

        public delegate void RequestContactsAccessCallback(AddressBookContactsAccessStatus accessStatus, Error error);

        public delegate void ReadContactsCallback(IAddressBookContact[] contacts, Error error);
        
        #endregion

        #region Constructors

        private AddressBookSimulator()
        {
            // set properties
            m_contacts          = GetDummyContacts();
            m_simulatorData     = LoadData() ?? new AddressBookSimulatorData();
        }

        #endregion

        #region Database methods

        private AddressBookSimulatorData LoadData()
        {
            return SimulatorServices.GetObject<AddressBookSimulatorData>(NativeFeatureType.kAddressBook);
        }

        private void SaveData()
        {
            SimulatorServices.SetObject(NativeFeatureType.kAddressBook, m_simulatorData);
        }

        public static void Reset() 
        {
            SimulatorServices.RemoveObject(NativeFeatureType.kAddressBook);
        }

        #endregion

        #region Public static methods

        public AddressBookContactsAccessStatus GetContactsAccessStatus()
        {
            return m_simulatorData.ContactsAccessStatus;
        }

        public void RequestContactsAccess(RequestContactsAccessCallback callback)
        {
            // check whether required permission is already granted
            var     accessStatus    = GetContactsAccessStatus();
            if (AddressBookContactsAccessStatus.Authorized == accessStatus)
            {
                callback(AddressBookContactsAccessStatus.Authorized, new Error(description: kAlreadyAuthorizedError));
            }
            else
            {
                // show prompt to user asking for required permission
                var     applicationSettings = EssentialKitSettings.Instance.ApplicationSettings;
                var     usagePermission     = applicationSettings.UsagePermissionSettings.AddressBookUsagePermission;

                var     newAlertDialog      = new AlertDialogBuilder()
                    .SetTitle("Address Book Simulator")
                    .SetMessage(usagePermission.GetDescriptionForActivePlatform())
                    .AddButton("Authorise", () => 
                    { 
                        // save selection
                        m_simulatorData.ContactsAccessStatus    = AddressBookContactsAccessStatus.Authorized;
                        SaveData();

                        // send result
                        callback(AddressBookContactsAccessStatus.Authorized, null);
                    })
                    .AddCancelButton("Cancel", () => 
                    { 
                        // save selection
                        m_simulatorData.ContactsAccessStatus    = AddressBookContactsAccessStatus.Denied;
                        SaveData();
                        
                        // send result
                        callback(AddressBookContactsAccessStatus.Denied, null);
                    }).
                    Build();
                newAlertDialog.Show();
            }
        }

        public void ReadContacts(ReadContactsCallback callback)
        {
            // read status and fetch appropriate result
            var     accessStatus    = GetContactsAccessStatus();
            if (AddressBookContactsAccessStatus.Authorized == accessStatus)
            {
                callback(m_contacts, null);
            }
            else
            {
                callback(null, new Error(description: kUnauthorizedAccessError));
            }
        }

        #endregion

        #region Private static methods

        private AddressBookContact[] GetDummyContacts()
        {
            // create fake contacts
            int     randCount       = Random.Range(10, 20);
            var     newContacts     = new AddressBookContact[randCount];
            for (int iter = 0; iter < randCount; iter++)
            {
                newContacts[iter]   = new AddressBookContact(
                    firstName: Path.GetRandomFileName(),
                    lastName: Path.GetRandomFileName(),
                    phoneNumbers: new string[] { (9876543200 + iter).ToString() });
            }
            return newContacts;
        }

        #endregion
	}
}