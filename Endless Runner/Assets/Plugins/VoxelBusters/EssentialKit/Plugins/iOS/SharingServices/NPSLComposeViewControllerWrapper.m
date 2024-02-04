//
//  NPSLComposeViewControllerAdapter.m
//  Native Plugins
//
//  Created by Ashwin kumar on 22/01/19.
//  Copyright (c) 2019 Voxel Busters Interactive LLP. All rights reserved.
//

#import "NPSLComposeViewControllerWrapper.h"
#import "NPDefines.h"
#import "UIViewController+Presentation.h"

// constants
static NSString* const kURLSchemeFacebook   = @"fb://";
static NSString* const kURLSchemeTwitter    = @"twitter://";

@interface NPSLComposeViewControllerWrapper ()

// internal properties
@property(nonatomic) SLComposeViewController*                   internalComposer;
@property(nonatomic) SLComposeViewControllerCompletionHandler   completionHandler;

@end

@implementation NPSLComposeViewControllerWrapper

@synthesize internalComposer    = _internalComposer;
@synthesize completionHandler   = _completionHandler;

#pragma mark - Static methods

+ (bool)isServiceTypeAvailable:(NSString*)serviceType
{
    if (SYSTEM_VERSION_GREATER_THAN_OR_EQUAL_TO(@"11.0"))
    {
        if ([serviceType isEqualToString:SLServiceTypeFacebook])
        {
            return [[UIApplication sharedApplication] canOpenURL:[NSURL URLWithString:kURLSchemeFacebook]];
        }
        if ([serviceType isEqualToString:SLServiceTypeTwitter])
        {
            return [[UIApplication sharedApplication] canOpenURL:[NSURL URLWithString:kURLSchemeTwitter]];
        }
        
        return false;
    }
    else
    {
        return [SLComposeViewController isAvailableForServiceType:serviceType];
    }
}

#pragma mark - Init methods

- (id)initWithServiceType:(NSString*)serviceType
{
    self = [super init];
    if (self)
    {
        // create composer instance
        __weak NPSLComposeViewControllerWrapper*    weakSelf    = self;
        __weak SLComposeViewController*             composer    = [SLComposeViewController composeViewControllerForServiceType:serviceType];
        [composer setCompletionHandler:^(SLComposeViewControllerResult result) {
            [weakSelf onComposerDidDismiss:result];
        }];
        
        // set properties
        [self setInternalComposer:composer];
    }
    return self;
}

- (void)dealloc
{
    if ([self internalComposer])
    {
        [[self internalComposer] setCompletionHandler:nil];
    }
}

#pragma mark - Private methods

- (void)onComposerDidDismiss:(SLComposeViewControllerResult)result
{
    // invoke user defined completion block
    if ([self completionHandler])
    {
        [self completionHandler](result);
    }
    
    // dismiss controller
    [UnityGetGLViewController() dismissViewControllerAnimated:YES completion:NULL];
}

#pragma mark - NPSocialShareComposer methods

- (BOOL)addText:(NSString*)text
{
    return [[self internalComposer] setInitialText:text];
}

- (BOOL)addImage:(UIImage*)image
{
   return [[self internalComposer] addImage:image];
}

- (BOOL)addURL:(NSURL*)url
{
    return [[self internalComposer] addURL:url];
}

- (void)setCompletionHandler:(SLComposeViewControllerCompletionHandler)completionHandler
{
    _completionHandler = [completionHandler copy];
}

- (void)showAtPosition:(CGPoint)position
{
    //Embedding internal composer within a navigation controller as its buggy when directly presented. Now callbacks on dismissed are proper.
    UINavigationController *viewControllerToPresent=[[UINavigationController alloc]initWithRootViewController:[self internalComposer]];
    [viewControllerToPresent setNavigationBarHidden:TRUE];
    
    [UnityGetGLViewController() presentViewControllerInPopoverStyleIfRequired:viewControllerToPresent
                                                                 withDelegate:self
                                                                 fromPosition:position
                                                                     animated:YES
                                                                   completion:nil];
}

#pragma mark - UIPopoverPresentationControllerDelegate implementation

- (void)presentationControllerDidDismiss:(UIPresentationController*)presentationController
{
    [self onComposerDidDismiss:SLComposeViewControllerResultCancelled];
}

@end
