//
//  NPShareSheetWrapper.mm
//  Native Plugins
//
//  Created by Ashwin kumar on 22/01/19.
//  Copyright (c) 2019 Voxel Busters Interactive LLP. All rights reserved.
//

#import "NPShareSheetWrapper.h"
#import "NPKit.h"
#import "UIViewController+Presentation.h"

// static properties
static ShareSheetClosedNativeCallback   _shareSheetClosedCallback;

@interface NPShareSheetWrapper ()

@property(nonatomic) NSMutableArray*           itemsArray;
@property(nonatomic) UIActivityViewController* activityController;

@end

@implementation NPShareSheetWrapper

@synthesize itemsArray          = _itemsArray;
@synthesize activityController  = _activityController;

+ (void)setShareSheetClosedCallback:(ShareSheetClosedNativeCallback)callback
{
    _shareSheetClosedCallback   = callback;
}

- (id)init
{
    self = [super init];
    if (self)
    {
        [self setItemsArray:[NSMutableArray array]];
    }
    return self;
}

- (void)dealloc
{
    if ([self activityController])
    {
        [[self activityController] setCompletionWithItemsHandler:nil];
    }
}

#pragma mark - Public methods

- (void)addItem:(NSObject *)item
{
    [[self itemsArray] addObject:item];
}

- (void)showAtPosition:(CGPoint)position withAnimation:(BOOL)animated
{
    position = NPConverToNativePosition(position.x, position.y);
    
    __weak NPShareSheetWrapper* weakSelf = self;
    
    // create new instance
    UIActivityViewController* activityVC    = [[UIActivityViewController alloc] initWithActivityItems:_itemsArray applicationActivities:nil];
    [activityVC setCompletionWithItemsHandler:^(UIActivityType  _Nullable activityType, BOOL completed, NSArray * _Nullable returnedItems, NSError * _Nullable activityError) {
        [weakSelf onSheetActionFinished:activityType isCompleted:completed withReturnItems:returnedItems andError:activityError];
    }];
    [self setActivityController:activityVC];
    
    
    // show the view controller
    [UnityGetGLViewController() presentViewControllerInPopoverStyleIfRequired:activityVC
                                                                 withDelegate:self
                                                                 fromPosition:position
                                                                permittedArrowDirections : UIPopoverArrowDirectionAny
                                                                     animated:YES
                                                                   completion:^{
        NSLog(@"[NativePlugins] Showing share sheet.");
    }];
}

- (void)onSheetActionFinished:(UIActivityType)activityType
                  isCompleted:(BOOL)completed
              withReturnItems:(NSArray* __nullable)returnedItems
                     andError:(NSError* __nullable)activityError
{
    NSLog(@"[NativePlugins] Dismissing share sheet. Status: %d", completed);

    // send data using callback
    void* nativePtr     = (void*)(__bridge CFTypeRef)self;
    _shareSheetClosedCallback(nativePtr, completed, NPCreateCStringFromNSError(activityError));
    
    // remove composer from window
    [UnityGetGLViewController() dismissViewControllerAnimated:YES completion:nil];
}

#pragma mark - UIPopoverPresentationControllerDelegate implementation

- (void)presentationControllerDidDismiss:(UIPresentationController*)presentationController
{
    NSLog(@"[NativePlugins] Dismissing share sheet.");
}

@end
