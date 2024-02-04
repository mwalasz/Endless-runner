//
//  NPDatePickerControllerBinding.mm
//  Native Plugins
//
//  Created by Ashwin kumar on 22/01/19.
//  Copyright (c) 2019 Voxel Busters Interactive LLP. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>
#import "NPKit.h"
#import "NPUIManager.h"

#pragma mark - Native binding methods

NPBINDING DONTSTRIP void NPDatePickerControllerRegisterCallback(NativeDatePickerControllerCallback callback)
{
    [NPUIManager setDatePickerControllerCallback:callback];
}


NPBINDING DONTSTRIP void* NPDatePickerControllerShow(UIDatePickerMode mode, long initialEpoch, long minimumEpoch, long maximumEpoch, void* tagPtr)
{
    [[NPUIManager sharedManager] showDatePicker:mode withInitialDate:initialEpoch withMinimumDate:minimumEpoch withMaximumDate:maximumEpoch tagPtr:tagPtr];
}
