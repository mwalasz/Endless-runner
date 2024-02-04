using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;

public class ContactHandle : MonoBehaviour
{
    [SerializeField] private GameObject contactPrefab;
    // Start is called before the first frame update
    void Start()
    {
        IG_AddressBookService.Instance.ReadContact();
    }

    public void UpdateContact()
    {
        Debug.Log("Update Contact");
        var contacts = IG_AddressBookService.Instance.contacts;
        if (contacts == null)
        {
            Debug.Log("No contacts found");
            return;
        }
        foreach (var contact in IG_AddressBookService.Instance.contacts)
        {
            ContactUI contactUI = Instantiate(contactPrefab, transform).GetComponent<ContactUI>();
            bool hasEmail = contact.EmailAddresses.Length > 0;
            //contactUI.SetContact(contact.FirstName + " " + contact.LastName, hasEmail);
        }
    }



    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Escape");
            UpdateContact();
        }
    }

    
}
