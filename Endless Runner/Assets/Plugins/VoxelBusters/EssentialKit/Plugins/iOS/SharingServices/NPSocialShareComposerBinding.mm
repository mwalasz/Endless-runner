//
//  NPSocialShareComposerBinding.mm
//  Native Plugins
//
//  Created by Ashwin kumar on 22/01/19.
//  Copyright (c) 2019 Voxel Busters Interactive LLP. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <Social/Social.h>
#import "NPSharingServicesDataTypes.h"
#import "NPSharingServicesManager.h"
#import "NPWhatsAppShareComposer.h"
#import "NPSLComposeViewControllerWrapper.h"
#import "NPKit.h"

#pragma mark - Callback definitions

// static properties
SocialShareComposerClosedNativeCallback _shareComposerClosedCallback;

#pragma mark - Native binding methods

NPBINDING DONTSTRIP bool NPSocialShareComposerIsComposerAvailable(NPSocialShareComposerType composerType)
{
    return [NPSharingServicesManager isSocialShareComposerAvailable:composerType];
}

NPBINDING DONTSTRIP void NPSocialShareComposerRegisterCallback(SocialShareComposerClosedNativeCallback closedCallback)
{
    // save references
    _shareComposerClosedCallback    = closedCallback;
}

NPBINDING DONTSTRIP void* NPSocialShareComposerCreate(NPSocialShareComposerType composerType)
{
    id<NPSocialShareComposer>   composer    = [NPSharingServicesManager createShareComposer:composerType];
    void*                       nativePtr   = NPRetainWithOwnershipTransfer(composer);
    [composer setCompletionHandler:^(SLComposeViewControllerResult result) {
        NSLog(@"[NativePlugins] Dismissing share composer. Result: %ld", (long)result);
        _shareComposerClosedCallback(nativePtr, result);
    }];
    return nativePtr;
}

NPBINDING DONTSTRIP void NPSocialShareComposerShow(void* nativePtr, float posX, float posY)
{
    NSLog(@"[NativePlugins] Showing share composer.");

    id<NPSocialShareComposer>   composer    = (__bridge id<NPSocialShareComposer>)nativePtr;
    [composer showAtPosition:CGPointMake(posX, posY)];
}

NPBINDING DONTSTRIP void NPSocialShareComposerAddText(void* nativePtr, const char* value)
{
    id<NPSocialShareComposer>   composer    = (__bridge id<NPSocialShareComposer>)nativePtr;
    [composer addText:NPCreateNSStringFromCString(value)];
}

NPBINDING DONTSTRIP void NPSocialShareComposerAddScreenshot(void* nativePtr)
{
    id<NPSocialShareComposer>   composer    = (__bridge id<NPSocialShareComposer>)nativePtr;
    [composer addImage:NPCaptureScreenshotAsImage()];
}

NPBINDING DONTSTRIP void NPSocialShareComposerAddImage(void* nativePtr, void* dataArrayPtr, int dataArrayLength)
{
    id<NPSocialShareComposer>   composer    = (__bridge id<NPSocialShareComposer>)nativePtr;
    [composer addImage:NPCreateImage(dataArrayPtr, dataArrayLength)];
}

NPBINDING DONTSTRIP void NPSocialShareComposerAddURL(void* nativePtr, const char* value)
{
    id<NPSocialShareComposer>   composer    = (__bridge id<NPSocialShareComposer>)nativePtr;
    [composer addURL:NPCreateNSURLFromCString(value)];
}
