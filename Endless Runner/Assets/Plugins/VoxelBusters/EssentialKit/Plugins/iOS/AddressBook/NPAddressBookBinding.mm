//
//  NPAddressBookBinding.mm
//  Native Plugins
//
//  Created by Ashwin kumar on 22/01/19.
//  Copyright (c) 2019 Voxel Busters Interactive LLP. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "NPAddressBookDataTypes.h"
#import "NativePluginsContactsWrapper.h"
#import "NPKit.h"

#pragma mark - Native binding methods

NPBINDING DONTSTRIP void NPAddressBookRegisterCallbacks(RequestContactsAccessNativeCallback requestContactsAccessCallback, ReadContactsNativeCallback readContactsCallback)
{
    [NativePluginsContactsWrapper setRequestContactsAccessCallback:requestContactsAccessCallback];
    [NativePluginsContactsWrapper setReadContactsNativeCallback:readContactsCallback];
}

NPBINDING DONTSTRIP CNAuthorizationStatus NPAddressBookGetAuthorizationStatus()
{
    return [NativePluginsContactsWrapper getAuthorizationStatus];
}

NPBINDING DONTSTRIP void NPAddressBookRequestContactsAccess(NPIntPtr tagPtr)
{
    [NativePluginsContactsWrapper requestContactsAccess:tagPtr];
}

NPBINDING DONTSTRIP void NPAddressBookReadContacts(NPIntPtr tagPtr)
{
    [NativePluginsContactsWrapper readContacts:tagPtr];
}

NPBINDING DONTSTRIP void NPAddressBookReset()
{
    NSLog(@"Not implemented.");
}
