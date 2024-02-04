//
//  NPMailComposerDelegate.h
//  Native Plugins
//
//  Created by Ashwin kumar on 22/01/19.
//  Copyright (c) 2019 Voxel Busters Interactive LLP. All rights reserved.
//

#import <MessageUI/MessageUI.h>
#import "NPSharingServicesDataTypes.h"

@interface NPSharingServicesManager : NSObject<MFMailComposeViewControllerDelegate, MFMessageComposeViewControllerDelegate>

+ (NPSharingServicesManager*)sharedManager;
+ (void)setMailComposerClosedCallback:(MailComposerClosedNativeCallback)callback;
+ (void)setMessageComposerClosedCallback:(MessageComposerClosedNativeCallback)callback;
+ (id<NPSocialShareComposer>)createShareComposer:(NPSocialShareComposerType)composerType;
+ (bool)isSocialShareComposerAvailable:(NPSocialShareComposerType)composerType;

@end
