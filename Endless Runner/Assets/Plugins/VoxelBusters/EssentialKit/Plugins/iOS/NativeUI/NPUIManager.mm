//
//  NPUIManager.mm
//  Native Plugins
//
//  Created by Ashwin kumar on 22/01/19.
//  Copyright (c) 2019 Voxel Busters Interactive LLP. All rights reserved.
//

#import "NPUIManager.h"
#import "NPKit.h"
#import "UIViewController+Presentation.h"
#import "UIAlertController+DatePicker.h"

static NPUIManager*                         _sharedManager                  = nil;
static NativeAlertActionSelectCallback      _alertActionSelectCallback      = nil;
static NativeDatePickerControllerCallback   _datePickerControllerCallback   = nil;


@interface NPUIManager ()
@end

@implementation NPUIManager
void* cachedTagPtr;

+ (NPUIManager*)sharedManager
{
    if (nil == _sharedManager)
    {
        _sharedManager  = [[NPUIManager alloc] init];
    }
    return _sharedManager;
}

+ (void)setAlertActionSelectCallback:(NativeAlertActionSelectCallback)callback
{
    // save references
    _alertActionSelectCallback    = callback;
}

+ (void)setDatePickerControllerCallback:(NativeDatePickerControllerCallback)callback
{
    _datePickerControllerCallback   = callback;
}

- (void)showAlertController:(UIAlertController*)alertController
{
    // present view  contoller
    CGRect  viewFrame   = [UnityGetGLView() frame];
    CGPoint spawnPoint  = CGPointMake(CGRectGetMidX(viewFrame), CGRectGetMidY(viewFrame));
    [UnityGetGLViewController() presentViewControllerInPopoverStyleIfRequired:alertController
                                                                 withDelegate:self
                                                                 fromPosition:spawnPoint
                                                     permittedArrowDirections:UIPopoverArrowDirectionAny
                                                                     animated:YES
                                                                   completion:nil];
}

- (void)dismissAlertController:(UIAlertController *)alertController
{
    UIViewController*   parentVC    = UnityGetGLViewController();
    if (parentVC.presentedViewController == alertController)
    {
        [UnityGetGLViewController() dismissViewControllerAnimated:YES completion:^{
            NSLog(@"[NativePlugins] Dismissing alert controller.");
        }];
    }
}

- (void) showDatePicker:(UIDatePickerMode) mode withInitialDate:(long) initialDateTime withMinimumDate:(long) min withMaximumDate:(long) max  tagPtr:(void*)tagPtr {

    
    UIAlertController *controller = [UIAlertController create:   mode
                                                                     withInitialDate:(initialDateTime == -1) ? nil : [NSDate dateWithTimeIntervalSince1970:initialDateTime]
                                                                     withMinimumDate:(min == -1) ? nil : [NSDate dateWithTimeIntervalSince1970:min]
                                                                     withMaximumDate:(max == -1) ? nil : [NSDate dateWithTimeIntervalSince1970:max]
                                                                     withTag:tagPtr
                                                                    withCallback: ^(NSDate * selectedDate, void* tag) {
                                                                                if(_datePickerControllerCallback != nil) {
                                                                                    _datePickerControllerCallback(selectedDate == nil ? -1 : (long)([selectedDate timeIntervalSince1970]), tag);
                                                                                }
                                                                            }];
        
    CGRect  viewFrame   = [UnityGetGLView() frame];
    CGPoint spawnPoint  = CGPointMake(CGRectGetMidX(viewFrame), CGRectGetMidY(viewFrame));
    cachedTagPtr = tagPtr;
    [UnityGetGLViewController() presentViewControllerInPopoverStyleIfRequired:controller
                                                                    withDelegate:self
                                                                    fromPosition:spawnPoint
                                                                        animated:YES
                                                                   completion:nil];
}

#pragma mark - Callback methods

- (void)alertController:(UIAlertController*)alertController didSelectAction:(UIAlertAction*)action
{
    // get selected button index
    NSUInteger  selectedActionIndex = [[alertController actions] indexOfObject:action];
    NSLog(@"[NativePlugins] Selected alert action is at index: %lu", (unsigned long)selectedActionIndex);

    // dimiss view controller
    [UnityGetGLViewController() dismissViewControllerAnimated:YES completion:nil];

    
    UIAlertController* retainAlertController = alertController;
    
    // invoke associated event
    if(_alertActionSelectCallback != nil)
    {
        _alertActionSelectCallback((__bridge void*)retainAlertController, (int)selectedActionIndex);
    }
}

#pragma mark - UIPopoverPresentationControllerDelegate implementation

- (void)presentationControllerDidDismiss:(UIPresentationController*)presentationController
{
    UIViewController *controller = presentationController.presentedViewController;
    
    if (controller != nil && [controller class] == [UIAlertController class])
    {
        UIAlertController* alertController  = (UIAlertController*)controller;

        // find cancel button
        NSArray*        actions         = [alertController actions];
        UIAlertAction*  targetAction    = nil;
        for (UIAlertAction* action in actions)
        {
            if (action.style == UIAlertActionStyleCancel || action.style == UIAlertActionStyleDestructive)
            {
                targetAction = action;
                break;
            }
        }
        // fallback case, select first available button
        if (targetAction == nil && [actions count] > 0)
        {
            targetAction    = actions[0];
        }
        // invoke callback
        if (targetAction)
        {
            [self alertController:alertController didSelectAction:targetAction];
        }
    }
    else if (controller != nil && [controller class] == [UIAlertController class])
    {
        UIAlertController* datePickerController  = (UIAlertController*)controller;
        NSLog(@"Date picker controller dismissed : %@", datePickerController);

        if(_datePickerControllerCallback != nil) {
            _datePickerControllerCallback(-1, cachedTagPtr);
        }
    }
}

@end
