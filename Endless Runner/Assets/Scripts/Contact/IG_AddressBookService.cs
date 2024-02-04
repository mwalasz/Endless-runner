using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.EssentialKit;

public class IG_AddressBookService : MonoBehaviour
{
    public static IG_AddressBookService Instance;
    AddressBookContactsAccessStatus status;
    public IAddressBookContact[] contacts;
    [SerializeField] private Transform contactHolder;
    [SerializeField] private GameObject contactPrefab;

    [SerializeField] private GameObject contactPrefab2;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        bool canSendMail = MailComposer.CanSendMail();
        contactPrefab2.GetComponent<TextMeshProUGUI>().text = "Can send mail: " + canSendMail;
    }

    public void ReadContact()
    {
        status = AddressBook.GetContactsAccessStatus();
        if (status == AddressBookContactsAccessStatus.NotDetermined)
        {
            AddressBook.RequestContactsAccess(callback: OnRequestContactsAccessFinished);
        }
        else if (status == AddressBookContactsAccessStatus.Authorized)
        {
            AddressBook.ReadContacts(OnReadContactsFinished);
        }
        
    }

    private void OnReadContactsFinished(AddressBookReadContactsResult result, Error error)
    {
        if (error == null)
        {
            var contacts = result.Contacts;
            Debug.Log("Request to read contacts finished successfully.");
            Debug.Log("Total contacts fetched: " + contacts.Length);
            Debug.Log("Below are the contact details (capped to first 10 results only):");
            for (int iter = 0; iter < contacts.Length && iter < 10; iter++)
            {
                //Debug.Log(string.Format("[{0}]: {1}", iter, contacts[iter]));
                SetContact(contacts, iter);
            }
        }
        else
        {
            Debug.Log("Request to read contacts failed with error. Error: " + error);
        }
    }

    private void SetContact(IAddressBookContact[] contacts, int iter)
    {
        //Create an instance of the contactUI prefab and set the contact details
        ContactUI contactUI = Instantiate(contactPrefab, contactHolder).GetComponent<ContactUI>();
        bool hasEmail = contacts[iter].EmailAddresses.Length > 0;
        contactUI.SetContact(contacts[iter].FirstName + " " + contacts[iter].LastName, hasEmail, hasEmail ? contacts[iter].EmailAddresses[0] : null);
    }

    private void OnRequestContactsAccessFinished(AddressBookRequestContactsAccessResult result, Error error)
    {
        Debug.Log("Request for contacts access finished.");
        Debug.Log("Address book contacts access status: " + result.AccessStatus);

        if (result.AccessStatus == AddressBookContactsAccessStatus.Authorized)
        {
            AddressBook.ReadContacts(OnReadContactsFinished);
        }
    }
}
