//
//  UIViewController+Presentation.mm
//  Native Plugins
//
//  Created by Ashwin kumar on 22/01/19.
//  Copyright (c) 2019 Voxel Busters Interactive LLP. All rights reserved.
//

#import "UIViewController+Presentation.h"

@implementation UIViewController (Presentation)

- (void)presentViewControllerInPopoverStyleIfRequired:(UIViewController*)viewControllerToPresent
                                         withDelegate:(id<UIPopoverPresentationControllerDelegate>)delegate
                                         fromPosition:(CGPoint)position
                                             animated:(BOOL)flag
                                           completion:(void (^)())completion
{
    [self presentViewControllerInPopoverStyleIfRequired:viewControllerToPresent
                                           withDelegate:delegate
                                           fromPosition:position
                               permittedArrowDirections:0
                                               animated:flag
                                             completion:completion];
}

- (void)presentViewControllerInPopoverStyleIfRequired:(UIViewController*)viewControllerToPresent
                                         withDelegate:(id<UIPopoverPresentationControllerDelegate>)delegate
                                         fromPosition:(CGPoint)position
                             permittedArrowDirections:(UIPopoverArrowDirection)direction
                                             animated:(BOOL)flag
                                          completion:(void (^)())completion
{
    // change presentation style to popover for iPad device
    if (UI_USER_INTERFACE_IDIOM() == UIUserInterfaceIdiomPad)
    {
        viewControllerToPresent.modalPresentationStyle                  = UIModalPresentationPopover;
    
        UIPopoverPresentationController* popoverPresentationController  = viewControllerToPresent.popoverPresentationController;
        popoverPresentationController.delegate                          = delegate;
        popoverPresentationController.sourceView                        = self.view;
        popoverPresentationController.sourceRect                        = CGRectMake(position.x, position.y, 1, 1);
        popoverPresentationController.permittedArrowDirections          = direction;
    }
    else
    {
        [viewControllerToPresent setPresentationSettings];
    }
    
    if(![viewControllerToPresent isKindOfClass:[UIAlertController class]])
    {
        viewControllerToPresent.presentationController.delegate = delegate;
    }
    
    // present specified object
    [self presentViewController:viewControllerToPresent animated:flag completion:completion];
}

- (void)setPresentationSettings
{
    if (@available(iOS 13.0, *)) {
        self.modalPresentationStyle                    = UIModalPresentationAutomatic;
    } else {
        // Fallback on earlier versions
        self.modalPresentationStyle                    = UIModalPresentationFullScreen;
    }
}

- (void)presentViewControllerInFullScreen: (UIViewController*)viewControllerToPresent
                                 animated: (BOOL)flag
                                completion: (void (^)())completion
{
    self.modalPresentationStyle   = UIModalPresentationFullScreen;
    
    // present specified object
    [self presentViewController:viewControllerToPresent animated:flag completion:completion];
}
@end
