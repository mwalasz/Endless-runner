//
//  NPMailComposerBinding.mm
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

NPBINDING DONTSTRIP bool NPMailComposerCanSendMail()
{
    return [MFMailComposeViewController canSendMail];
}

NPBINDING DONTSTRIP void NPMailComposerRegisterCallback(MailComposerClosedNativeCallback closedCallback)
{
    [NPSharingServicesManager setMailComposerClosedCallback:closedCallback];
}

NPBINDING DONTSTRIP void* NPMailComposerCreate()
{
    // create composer
    MFMailComposeViewController*    composerController      = [[MFMailComposeViewController alloc] init];
    [composerController setMailComposeDelegate:[NPSharingServicesManager sharedManager]];
    
    return NPRetainWithOwnershipTransfer(composerController);
}

NPBINDING DONTSTRIP void NPMailComposerShow(void* nativePtr)
{
    MFMailComposeViewController*     composerController     = (__bridge MFMailComposeViewController*)nativePtr;
    [UnityGetGLViewController() presentViewController:composerController animated:YES completion:^{
        NSLog(@"[NativePlugins] Showing mail composer.");
    }];
}

NPBINDING DONTSTRIP void NPMailComposerSetRecipients(void* nativePtr, NPMailRecipientType recipientType, const char** recipients, int count)
{
    // convert to native representation
    NSArray*    recipientsNativeArray   = NPCreateArrayOfNSString(recipients, count);

    // set value
    MFMailComposeViewController*     composerController      = (__bridge MFMailComposeViewController*)nativePtr;
    switch (recipientType)
    {
        case NPMailRecipientTypeTo:
            [composerController setToRecipients:recipientsNativeArray];
            break;
            
        case NPMailRecipientTypeCc:
            [composerController setCcRecipients:recipientsNativeArray];
            break;
            
        case NPMailRecipientTypeBcc:
            [composerController setBccRecipients:recipientsNativeArray];
            break;
            
        default:
            break;
    }
}

NPBINDING DONTSTRIP void NPMailComposerSetSubject(void* nativePtr, const char* value)
{
    MFMailComposeViewController*     composerController      = (__bridge MFMailComposeViewController*)nativePtr;
    [composerController setSubject:NPCreateNSStringFromCString(value)];
}

NPBINDING DONTSTRIP void NPMailComposerSetBody(void* nativePtr, const char* value, bool isHtml)
{
    MFMailComposeViewController*     composerController      = (__bridge MFMailComposeViewController*)nativePtr;
    [composerController setMessageBody:NPCreateNSStringFromCString(value) isHTML:isHtml];
}

NPBINDING DONTSTRIP void NPMailComposerAddScreenshot(void* nativePtr, const char* fileName)
{
    MFMailComposeViewController*     composerController      = (__bridge MFMailComposeViewController*)nativePtr;
    [composerController addAttachmentData:NPCaptureScreenshotAsData(UIImageEncodeTypePNG)
                                 mimeType:kMimeTypePNG
                                 fileName:NPCreateNSStringFromCString(fileName)];
}

NPBINDING DONTSTRIP void NPMailComposerAddAttachment(void* nativePtr, NPUnityAttachment data)
{
    MFMailComposeViewController*     composerController      = (__bridge MFMailComposeViewController*)nativePtr;
    [composerController addAttachmentData:[NSData dataWithBytes:data.dataArrayPtr length:data.dataArrayLength]
                                 mimeType:NPCreateNSStringFromCString((const char*)data.mimeTypePtr)
                                 fileName:NPCreateNSStringFromCString((const char*)data.fileNamePtr)];
}
