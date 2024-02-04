//
//  NativePluginsContactsWrapper.h
//  Native Plugins
//
//  Created by Ashwin kumar on 22/01/19.
//  Copyright (c) 2019 Voxel Busters Interactive LLP. All rights reserved.
//

#import <Contacts/Contacts.h>
#import "NPAddressBookDataTypes.h"

@interface NativePluginsContactsWrapper : NSObject

+ (void)setRequestContactsAccessCallback:(RequestContactsAccessNativeCallback)callback;
+ (void)setReadContactsNativeCallback:(ReadContactsNativeCallback)callback;

+ (CNAuthorizationStatus)getAuthorizationStatus;
+ (void)requestContactsAccess:(void*)tagPtr;
+ (void)readContacts:(void*)tagPtr;

@end
