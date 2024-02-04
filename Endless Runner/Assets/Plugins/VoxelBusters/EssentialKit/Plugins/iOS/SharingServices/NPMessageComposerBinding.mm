//
//  NPMessageComposerBinding.mm
//  Native Plugins
//
//  Created by Ashwin kumar on 22/01/19.
//  Copyright (c) 2019 Voxel Busters Interactive LLP. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <MessageUI/MessageUI.h>
#import "NPSharingServicesDataTypes.h"
#import "NPSharingServicesManager.h"
#import "NPKit.h"

#pragma mark - Native binding methods

NPBINDING DONTSTRIP bool NPMessageComposerCanSendText()
{
    return [MFMessageComposeViewController canSendText];
}

NPBINDING DONTSTRIP bool NPMessageComposerCanSendAttachments()
{
    return [MFMessageComposeViewController canSendAttachments];
}

NPBINDING DONTSTRIP bool NPMessageComposerCanSendSubject()
{
    return [MFMessageComposeViewController canSendSubject];
}

NPBINDING DONTSTRIP void NPMessageComposerRegisterCallback(MessageComposerClosedNativeCallback closedCallback)
{
    // save references
    [NPSharingServicesManager setMessageComposerClosedCallback:closedCallback];
}

NPBINDING DONTSTRIP void* NPMessageComposerCreate()
{
    // create composer
    MFMessageComposeViewController* composerController      = [[MFMessageComposeViewController alloc] init];
    [composerController setMessageComposeDelegate:[NPSharingServicesManager sharedManager]];
    return NPRetainWithOwnershipTransfer(composerController);
}

NPBINDING DONTSTRIP void NPMessageComposerShow(void* nativePtr)
{
    MFMessageComposeViewController* composerController      = (__bridge MFMessageComposeViewController*)nativePtr;
    [UnityGetGLViewController() presentViewController:composerController animated:YES completion:^{
        NSLog(@"[NativePlugins] Showing message composer.");
    }];
}

NPBINDING DONTSTRIP void NPMessageComposerSetRecipients(void* nativePtr, const char** recipients, int count)
{
    MFMessageComposeViewController* composerController      = (__bridge MFMessageComposeViewController*)nativePtr;
    NSArray*                        recipientsNativeArray   = NPCreateArrayOfNSString(recipients, count);
    [composerController setRecipients:recipientsNativeArray];
}

NPBINDING DONTSTRIP void NPMessageComposerSetSubject(void* nativePtr, const char* value)
{
    MFMessageComposeViewController* composerController      = (__bridge MFMessageComposeViewController*)nativePtr;
    [composerController setSubject:NPCreateNSStringFromCString(value)];
}

NPBINDING DONTSTRIP void NPMessageComposerSetBody(void* nativePtr, const char* value)
{
    MFMessageComposeViewController* composerController      = (__bridge MFMessageComposeViewController*)nativePtr;
    [composerController setBody:NPCreateNSStringFromCString(value)];
}

NPBINDING DONTSTRIP void NPMessageComposerAddScreenshot(void* nativePtr, const char* fileName)
{
    NSData*                         screenshotData          = NPCaptureScreenshotAsData(UIImageEncodeTypePNG);
    NSString*                       typeId                  = (NSString*)kUTTypePNG;
    MFMessageComposeViewController* composerController      = (__bridge MFMessageComposeViewController*)nativePtr;
    [composerController addAttachmentData:screenshotData
                           typeIdentifier:typeId
                                 filename:NPCreateNSStringFromCString(fileName)];
}

NPBINDING DONTSTRIP void NPMessageComposerAddAttachment(void* nativePtr, NPUnityAttachment data)
{
    NSData*                         fileData                = [NSData dataWithBytes:data.dataArrayPtr length:data.dataArrayLength];
    NSString*                       mimeType                = NPCreateNSStringFromCString((const char*)data.mimeTypePtr);
    NSString*                       fileName                = NPCreateNSStringFromCString((const char*)data.fileNamePtr);
    MFMessageComposeViewController* composerController      = (__bridge MFMessageComposeViewController*)nativePtr;
    [composerController addAttachmentData:fileData
                           typeIdentifier:NPConvertMimeTypeToUTType(mimeType)
                                 filename:fileName];
}
