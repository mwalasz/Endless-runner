//
//  NPMailComposerDelegate.mm
//  Native Plugins
//
//  Created by Ashwin kumar on 22/01/19.
//  Copyright (c) 2019 Voxel Busters Interactive LLP. All rights reserved.
//

#import "NPSharingServicesManager.h"
#import "NPKit.h"
#import "NPSLComposeViewControllerWrapper.h"
#import "NPWhatsAppShareComposer.h"

static  NPSharingServicesManager*               _sharedManager                  = nil;
static  MailComposerClosedNativeCallback        _mailComposerClosedCallback     = nil;
static  MessageComposerClosedNativeCallback     _messageComposerClosedCallback  = nil;

@implementation NPSharingServicesManager

+ (NPSharingServicesManager*)sharedManager
{
    if (nil == _sharedManager)
    {
        _sharedManager  = [[NPSharingServicesManager alloc] init];
    }
    return _sharedManager;
}

+ (void)setMailComposerClosedCallback:(MailComposerClosedNativeCallback)callback;
{
    _mailComposerClosedCallback     = callback;
}

+ (void)setMessageComposerClosedCallback:(MessageComposerClosedNativeCallback)callback
{
    _messageComposerClosedCallback  = callback;
}

+ (bool)isSocialShareComposerAvailable:(NPSocialShareComposerType)composerType
{
    switch (composerType)
    {
        case NPSocialShareComposerTypeFacebook:
            return [NPSLComposeViewControllerWrapper isServiceTypeAvailable:SLServiceTypeFacebook];
            
        case NPSocialShareComposerTypeTwitter:
            return [NPSLComposeViewControllerWrapper isServiceTypeAvailable:SLServiceTypeTwitter];
            
        case NPSocialShareComposerTypeWhatsApp:
            return [NPWhatsAppShareComposer IsServiceAvailable];
            
        default:
            return NO;
    }
}

+ (id<NPSocialShareComposer>)createShareComposer:(NPSocialShareComposerType)composerType
{
    switch (composerType)
    {
        case NPSocialShareComposerTypeFacebook:
            return [[NPSLComposeViewControllerWrapper alloc] initWithServiceType:SLServiceTypeFacebook];
            
        case NPSocialShareComposerTypeTwitter:
            return [[NPSLComposeViewControllerWrapper alloc] initWithServiceType:SLServiceTypeTwitter];

        case NPSocialShareComposerTypeWhatsApp:
            return [[NPWhatsAppShareComposer alloc] init];
            
        default:
            return NULL;
    }
}

#pragma mark - MFMailComposeViewControllerDelegate methods

- (void)mailComposeController:(MFMailComposeViewController *)controller didFinishWithResult:(MFMailComposeResult)result error:(nullable NSError *)error
{
    NSLog(@"[NativePlugins] Dismissing mail composer. Result code: %ld.", (long)result);

    // send callback
    void*   nativePtr   = (void*)(__bridge CFTypeRef)controller;
    _mailComposerClosedCallback(nativePtr, result, NPCreateCStringFromNSError(error));
    
    // dismiss view controller
    [UnityGetGLViewController() dismissViewControllerAnimated:YES completion:nil];
}

#pragma mark - MFMessageComposeViewControllerDelegate methods

- (void)messageComposeViewController:(MFMessageComposeViewController *)controller didFinishWithResult:(MessageComposeResult)result
{
    NSLog(@"[NativePlugins] Dismissing message composer. Result code: %ld.", (long)result);

    // send callback
    void*   nativePtr   = (void*)(__bridge CFTypeRef)controller;
    _messageComposerClosedCallback(nativePtr, result);
    
    // dismiss view controller
    [UnityGetGLViewController() dismissViewControllerAnimated:YES completion:nil];
}

@end
