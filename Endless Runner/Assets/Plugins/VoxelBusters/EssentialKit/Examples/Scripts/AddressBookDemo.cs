using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins.DemoKit;

// internal namespace
namespace VoxelBusters.EssentialKit.Demo
{
    public class AddressBookDemo : DemoActionPanelBase<AddressBookDemoAction, AddressBookDemoActionType>
    {
        #region Base class methods

        protected override void OnActionSelectInternal(AddressBookDemoAction selectedAction)
        {
            switch (selectedAction.ActionType)
            {
                case AddressBookDemoActionType.GetContactsAccessStatus:
                    var     status  = AddressBook.GetContactsAccessStatus();
                    Log("Address book permission status: " + status);
                    break;

                case AddressBookDemoActionType.RequestContactsAccess:
                    AddressBook.RequestContactsAccess(callback: OnRequestContactsAccessFinish);
                    break;

                case AddressBookDemoActionType.ReadContacts:
                    AddressBook.ReadContacts(OnReadContactsFinish);
                    break;

                case AddressBookDemoActionType.ReadContactsWithUserPermission:
                    AddressBook.ReadContactsWithUserPermission(OnReadContactsFinish);
                    break;

                case AddressBookDemoActionType.ResourcePage:
                    ProductResources.OpenResourcePage(NativeFeatureType.kAddressBook);
                    break;

                default:
                    break;
            }
        }

        #endregion

        #region Plugin callback methods

        private void OnRequestContactsAccessFinish(AddressBookRequestContactsAccessResult result, Error error)
        {
            Log("Request for contacts access finished.");
            Log("Address book contacts access status: " + result.AccessStatus);
        }

        private void OnReadContactsFinish(AddressBookReadContactsResult result, Error error)
        {
            if (error == null)
            {
                var     contacts    = result.Contacts;
                Log("Request to read contacts finished successfully.");
                Log("Total contacts fetched: " + contacts.Length);
                Log("Below are the contact details (capped to first 10 results only):");
                for (int iter = 0; iter < contacts.Length && iter < 10; iter++)
                {
                    Log(string.Format("[{0}]: {1}", iter, contacts[iter]));
                }
            }
            else
            {
                Log("Request to read contacts failed with error. Error: " + error);
            }
        }

        #endregion
    }
}
