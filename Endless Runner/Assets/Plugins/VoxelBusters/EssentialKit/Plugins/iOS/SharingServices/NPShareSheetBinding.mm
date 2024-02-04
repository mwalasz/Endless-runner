//
//  NPShareSheetBinding.mm
//  Native Plugins
//
//  Created by Ashwin kumar on 22/01/19.
//  Copyright (c) 2019 Voxel Busters Interactive LLP. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "NPSharingServicesDataTypes.h"
#import "NPShareSheetWrapper.h"
#import "NPKit.h"

#pragma mark - Native binding methods

NPBINDING DONTSTRIP void NPShareSheetRegisterCallback(ShareSheetClosedNativeCallback closedCallback)
{
    [NPShareSheetWrapper setShareSheetClosedCallback:closedCallback];
}

NPBINDING DONTSTRIP void* NPShareSheetCreate()
{
    NPShareSheetWrapper*    shareSheet  = [[NPShareSheetWrapper alloc] init];
    return NPRetainWithOwnershipTransfer(shareSheet);
}

NPBINDING DONTSTRIP void NPShareSheetShow(void* nativePtr, float posX, float posY)
{
    NPShareSheetWrapper*    shareSheet  = (__bridge NPShareSheetWrapper*)nativePtr;
    [shareSheet showAtPosition:CGPointMake(posX, posY) withAnimation:YES];
}

NPBINDING DONTSTRIP void NPShareSheetAddText(void* nativePtr, const char* value)
{
    NPShareSheetWrapper*    shareSheet  = (__bridge NPShareSheetWrapper*)nativePtr;
    [shareSheet addItem:NPCreateNSStringFromCString(value)];
}

NPBINDING DONTSTRIP void NPShareSheetAddScreenshot(void* nativePtr)
{
    NPShareSheetWrapper*    shareSheet  = (__bridge NPShareSheetWrapper*)nativePtr;
    [shareSheet addItem:NPCaptureScreenshotAsImage()];
}

NPBINDING DONTSTRIP void NPShareSheetAddImage(void* nativePtr, void* dataArrayPtr, int dataArrayLength)
{
    NPShareSheetWrapper*    shareSheet  = (__bridge NPShareSheetWrapper*)nativePtr;
    [shareSheet addItem:NPCreateImage(dataArrayPtr, dataArrayLength)];
}

NPBINDING DONTSTRIP void NPShareSheetAddURL(void* nativePtr, const char* value)
{
    NPShareSheetWrapper*    shareSheet  = (__bridge NPShareSheetWrapper*)nativePtr;
    [shareSheet addItem:NPCreateNSURLFromCString(value)];
}

NPBINDING DONTSTRIP void NPShareSheetAddAttachment(void* nativePtr, NPUnityAttachment data)
{
    NPShareSheetWrapper*    shareSheet  = (__bridge NPShareSheetWrapper*)nativePtr;
    [shareSheet addItem:[NSData dataWithBytes:data.dataArrayPtr length:data.dataArrayLength]];
}
