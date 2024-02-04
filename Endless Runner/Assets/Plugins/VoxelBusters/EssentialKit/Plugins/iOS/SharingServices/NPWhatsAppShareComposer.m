//
//  NPWhatsAppShareComposer.m
//  Native Plugins
//
//  Created by Ashwin kumar on 22/01/19.
//  Copyright (c) 2019 Voxel Busters Interactive LLP. All rights reserved.
//

#import "NPWhatsAppShareComposer.h"
#import "NPDefines.h"

typedef enum : NSInteger
{
    NativePluginsWhatsAppShareTypeUndefined,
    NativePluginsWhatsAppShareTypeText,
    NativePluginsWhatsAppShareTypeImage,
} NativePluginsWhatsAppShareType;

@interface NPWhatsAppShareComposer ()

// properties
@property(nonatomic) NativePluginsWhatsAppShareType shareType;
@property(nonatomic) BOOL                           didCompleteSharing;
@property(nonatomic) CGPoint                        openPoint;

@property(nonatomic, copy) NSString*                text;
@property(nonatomic, strong) UIImage*               image;
@property(nonatomic, copy) SLComposeViewControllerCompletionHandler completionHandler;
@property(nonatomic, strong) UIDocumentInteractionController* interactionController;

@end

@implementation NPWhatsAppShareComposer

@synthesize shareType           = _shareType;
@synthesize didCompleteSharing  = _didCompleteSharing;
@synthesize openPoint           = _openPoint;

@synthesize text                = _text;
@synthesize image               = _image;
@synthesize completionHandler   = _completionHandler;

- (id)init
{
	self	= [super init];
    if (self)
	{
        self.shareType          = NativePluginsWhatsAppShareTypeUndefined;
		self.didCompleteSharing = NO;
        self.openPoint          = CGPointZero;
	}
	
	return self;
}

#pragma mark - Static methods

+ (bool)IsServiceAvailable
{
	return [[UIApplication sharedApplication] canOpenURL:[NSURL URLWithString:@"whatsapp://app"]];
}

#pragma mark - NativePluginsShareComposeController methods

- (BOOL)addText:(NSString*)text
{
    if (NativePluginsWhatsAppShareTypeUndefined == self.shareType)
    {
        bool    canShare    = [NPWhatsAppShareComposer IsServiceAvailable];
        if (canShare)
        {
            self.text       = text;
            self.shareType  = NativePluginsWhatsAppShareTypeText;
        }
        
        return canShare;
    }
    
    return false;
}

- (BOOL)addImage:(UIImage*)image
{
    if (NativePluginsWhatsAppShareTypeUndefined == self.shareType)
    {
        bool  canShare    = [NPWhatsAppShareComposer IsServiceAvailable];
        if (canShare)
        {
            self.image    = image;
            self.shareType= NativePluginsWhatsAppShareTypeImage;
        }

        return canShare;
    }
    
    return false;
}

- (BOOL)addURL:(NSURL*)url
{
    return [self addText:[url absoluteString]];
}

- (void)setCompletionHandler:(SLComposeViewControllerCompletionHandler)completionHandler
{
    _completionHandler  = [completionHandler copy];
}

- (void)showAtPosition:(CGPoint)position
{
    // save position
    self.openPoint  = position;

    // based on item type execute appropriate action
    switch (self.shareType)
    {
        case NativePluginsWhatsAppShareTypeText:
            [self shareText];
            break;
            
        case NativePluginsWhatsAppShareTypeImage:
            [self shareImage];
            break;
            
        default:
            [self invokeCompletionCallbackWithStatus:NO];
            break;
    }
}

#pragma mark - Private methods

- (void)shareText
{
    NSString*   messageURLStr   = [NSString stringWithFormat:@"whatsapp://send?text=%@", self.text];
    NSURL*      messageURL      = [NSURL URLWithString:[messageURLStr stringByAddingPercentEncodingWithAllowedCharacters:[NSCharacterSet URLQueryAllowedCharacterSet]]];
    
    // pass data to destination app
    [[UIApplication sharedApplication] openURL:messageURL
                                       options:@{}
                             completionHandler:^(BOOL success) {
                                 // send callback
                                 [self invokeCompletionCallbackWithStatus:success];
                             }];
}

- (void)shareImage
{
    if (self.image)
    {
        NSString*   tempFilePath    = [NSHomeDirectory() stringByAppendingPathComponent:@"Documents/WhatsAppTemp.wai"];
        [UIImageJPEGRepresentation(self.image, 1.0) writeToFile:tempFilePath atomically:YES];

        // create interaction controller
        _interactionController  = [UIDocumentInteractionController interactionControllerWithURL:[NSURL fileURLWithPath:tempFilePath]];
        _interactionController.UTI                               = @"net.whatsapp.image";
        _interactionController.delegate                          = self;
        [_interactionController presentOpenInMenuFromRect:CGRectMake(self.openPoint.x, self.openPoint.y, 1, 1)
                                                  inView:UnityGetGLView()
                                                animated:YES];
    }
    else
    {
        [self invokeCompletionCallbackWithStatus:NO];
    }
}
    
- (void)invokeCompletionCallbackWithStatus:(BOOL)status
{
    if (self.completionHandler)
    {
        self.completionHandler(status ? SLComposeViewControllerResultDone : SLComposeViewControllerResultCancelled);
    }
}
    
#pragma mark - UIDocumentInteractionControllerDelegate implementation

- (void)documentInteractionControllerWillPresentOpenInMenu:(UIDocumentInteractionController*)controller
{
    NSLog(@"[NativePlugins] WhatsApp share controller will present.");
}

- (void)documentInteractionControllerDidDismissOpenInMenu:(UIDocumentInteractionController*)controller
{
    NSLog(@"[NativePlugins] WhatsApp share controller did dismiss.");
}

- (void)documentInteractionController:(UIDocumentInteractionController*)controller willBeginSendingToApplication:(nullable NSString*)application
{
    NSLog(@"[NativePlugins] WhatsApp share controller will begin sending to application.");
    self.didCompleteSharing = YES;
}

- (void)documentInteractionController:(UIDocumentInteractionController*)controller didEndSendingToApplication:(nullable NSString*)application
{
    NSLog(@"[NativePlugins] WhatsApp share controller did end sending to application.");
    // execute callback block
    [self invokeCompletionCallbackWithStatus:self.didCompleteSharing];
}

@end
