//
//  NativePluginsContactsWrapper.mm
//  Native Plugins
//
//  Created by Ashwin kumar on 22/01/19.
//  Copyright (c) 2019 Voxel Busters Interactive LLP. All rights reserved.
//

#import "NativePluginsContactsWrapper.h"
#import "NPKit.h"

// static fields
static RequestContactsAccessNativeCallback  _requestContactsAccessCallback  = nil;
static ReadContactsNativeCallback           _readContactsCallback           = nil;

static CNContactStore*                      _contactStore                   = nil;

@implementation NativePluginsContactsWrapper

+ (void)setRequestContactsAccessCallback:(RequestContactsAccessNativeCallback)callback
{
    _requestContactsAccessCallback  = callback;
}

+ (void)setReadContactsNativeCallback:(ReadContactsNativeCallback)callback
{
    _readContactsCallback   = callback;
}

+ (CNContactStore*)getContactStore
{
    static dispatch_once_t onceToken;
    dispatch_once(&onceToken, ^{
        _contactStore = [[CNContactStore alloc] init];
    });
    return _contactStore;
}

+ (CNAuthorizationStatus)getAuthorizationStatus
{
    return [CNContactStore authorizationStatusForEntityType:CNEntityTypeContacts];
}

+ (void)requestContactsAccess:(void*)tagPtr
{
    // ask user permission if not provided yet!
    CNAuthorizationStatus   authStatus  = [NativePluginsContactsWrapper getAuthorizationStatus];
    if (CNAuthorizationStatusNotDetermined == authStatus)
    {
        [[NativePluginsContactsWrapper getContactStore] requestAccessForEntityType:CNEntityTypeContacts
                                                                 completionHandler:^(BOOL granted, NSError* __nullable error) {
                                                                     // send callback
                                                                     CNAuthorizationStatus  newStatus   = [NativePluginsContactsWrapper getAuthorizationStatus];
                                                                     char*                  cError      = NPCreateCStringFromNSError(error);
                                                                     _requestContactsAccessCallback(newStatus, cError, tagPtr);
                                                                 }];
    }
    else
    {
        // send callback
        _requestContactsAccessCallback(authStatus, nil, tagPtr);
    }
}

+ (void)readContacts:(void*)tagPtr
{
    // read contacts information from database
    CNContactStore*         contactStore        = [NativePluginsContactsWrapper getContactStore];
    NSMutableArray*         contactsList        = [NSMutableArray array];
    bool                    finished            = false;
    NSError*                error               = nil;
    CNContactFetchRequest*  fetchRequest        = [[CNContactFetchRequest alloc] initWithKeysToFetch:@[CNContactGivenNameKey,
                                                                                                       CNContactMiddleNameKey,
                                                                                                       CNContactFamilyNameKey,
                                                                                                       CNContactImageDataKey,
                                                                                                       CNContactPhoneNumbersKey,
                                                                                                       CNContactEmailAddressesKey]];
    [fetchRequest setUnifyResults:YES];
    [fetchRequest setSortOrder:CNContactSortOrderGivenName];
    
    do
    {
        finished    = [contactStore enumerateContactsWithFetchRequest:fetchRequest
                                                                error:&error
                                                           usingBlock:^(CNContact* _Nonnull contact, BOOL * _Nonnull stop) {
                                                               // add to array
                                                               [contactsList addObject:contact];
                                                           }];
        
    } while (!finished);
    
    // check whether read operation status and send appropriate data using callback
    if (error)
    {
        // send error info
        char*   cError  = NPCreateCStringFromNSError(error);
        _readContactsCallback(nil, 0, cError, tagPtr);
        return;
    }
    else
    {
        // transform data to unity format
        int                             totalContacts   = (int)[contactsList count];
        NativeAddressBookContactData    contactsArray[totalContacts];
        for (int iter = 0; iter < totalContacts; iter++)
        {
            CNContact*                  currentContact  = [contactsList objectAtIndex:iter];
            
            // copy info
            NativeAddressBookContactData*   nativeData  = &contactsArray[iter];
            nativeData->CopyProperties(currentContact);
        }
        
        // send data using callback
        _readContactsCallback(contactsArray, totalContacts, nil, tagPtr);
    }
}

@end
