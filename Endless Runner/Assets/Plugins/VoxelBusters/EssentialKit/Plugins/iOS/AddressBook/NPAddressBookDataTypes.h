//
//  NPAddressBookDataTypes.h
//  Native Plugins
//
//  Created by Ashwin kumar on 22/01/19.
//  Copyright (c) 2019 Voxel Busters Interactive LLP. All rights reserved.
//

#import <Contacts/Contacts.h>

// callback signatures
typedef void (*RequestContactsAccessNativeCallback)(CNAuthorizationStatus status, const char* error, void* tagPtr);
typedef void (*ReadContactsNativeCallback)(void* contactsPtr, int count, const char* error, void* tagPtr);

// custom datatypes
struct NPUnityAddressBookContact
{
    // data members
    void*           nativeObjectPtr;
    void*           firstNamePtr;
    void*           middleNamePtr;
    void*           lastNamePtr;
    void*           imageDataPtr;
    int             phoneNumberCount;
    void*           phoneNumbersPtr;
    int             emailAddressCount;
    void*           emailAddressesPtr;
    
    // methods
    void CopyProperties(CNContact* contact);
    
    // destructors
    ~NPUnityAddressBookContact();
};
typedef NPUnityAddressBookContact NativeAddressBookContactData;
