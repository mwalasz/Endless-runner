//
//  NPAddressBookDataTypes.mm
//  Native Plugins
//
//  Created by Ashwin kumar on 22/01/19.
//  Copyright (c) 2019 Voxel Busters Interactive LLP. All rights reserved.
//

#import "NPAddressBookDataTypes.h"
#import "NPKit.h"

static NSCharacterSet* kPhoneNumExcludedCharacterSet    = [[NSCharacterSet characterSetWithCharactersInString:@"0123456789+"] invertedSet];

void NPUnityAddressBookContact::CopyProperties(CNContact* contact)
{
    // copy phone numbers
    NSArray*    phoneNumbers        = contact.phoneNumbers;
    int         phoneNumberCount    = 0;
    char**      phoneNumbersCArray  = nil;
    if (phoneNumbers != NULL)
    {
        phoneNumberCount            = (int)[phoneNumbers count];
        phoneNumbersCArray          = (char**)calloc(phoneNumberCount, sizeof(char*));
        for (int iter = 0; iter < phoneNumberCount; iter++)
        {
            CNLabeledValue<CNPhoneNumber*>* phoneNumber     = [phoneNumbers objectAtIndex:iter];
            NSString* rawFormatNumber                       = [[phoneNumber value] stringValue];
            NSString* formattedNumber                       = [[rawFormatNumber componentsSeparatedByCharactersInSet:kPhoneNumExcludedCharacterSet] componentsJoinedByString:@""];
            
            // add to c-array
            phoneNumbersCArray[iter]    = NPCreateCStringFromNSString(formattedNumber);
        }
    }
    
    // copy email addresses
    NSArray*    emailAddresses          = contact.emailAddresses;
    int         emailCount              = 0;
    char**      emailCArray             = nil;
    if (contact.emailAddresses != NULL)
    {
        emailCount                      = (int)[emailAddresses count];
        emailCArray                     = (char**)calloc(emailCount, sizeof(char*));
        for (int iter = 0; iter < emailCount; iter++)
        {
            CNLabeledValue<NSString*>*  emailValue      = [emailAddresses objectAtIndex:iter];
            NSString*                   emailStr        = [emailValue value];
            
            // add to c-array
            emailCArray[iter]           = NPCreateCStringFromNSString(emailStr);
        }
    }
    
    // set properties
    this->nativeObjectPtr       = (__bridge void*)contact;
    this->firstNamePtr          = (void*)NPCreateCStringFromNSString(contact.givenName);
    this->middleNamePtr         = (void*)NPCreateCStringFromNSString(contact.middleName);
    this->lastNamePtr           = (void*)NPCreateCStringFromNSString(contact.familyName);
    this->imageDataPtr          = (__bridge void*)contact.imageData;
    this->phoneNumberCount      = phoneNumberCount;
    this->phoneNumbersPtr       = (void*)phoneNumbersCArray;
    this->emailAddressCount     = emailCount;
    this->emailAddressesPtr     = (void*)emailCArray;
}

NPUnityAddressBookContact::~NPUnityAddressBookContact()
{
    // release c objects
    free(phoneNumbersPtr);
    free(emailAddressesPtr);
}
