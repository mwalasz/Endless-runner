//
//  NPUIManager.h
//  Native Plugins
//
//  Created by Ashwin kumar on 22/01/19.
//  Copyright (c) 2019 Voxel Busters Interactive LLP. All rights reserved.
//

#import <MessageUI/MessageUI.h>

// signature
typedef void (*NativeAlertActionSelectCallback)(void* nativePtr, int selectedActionIndex);
typedef void (*NativeDatePickerControllerCallback)(long selectedValue, void* tagPtr);

@interface NPUIManager : NSObject<UIPopoverPresentationControllerDelegate>

+ (NPUIManager*)sharedManager;
+ (void)setAlertActionSelectCallback:(NativeAlertActionSelectCallback)callback;
+ (void)setDatePickerControllerCallback:(NativeDatePickerControllerCallback)callback;

// presentation methods
- (void)showAlertController:(UIAlertController*)alertController;
- (void)dismissAlertController:(UIAlertController*)alertController;
- (void) showDatePicker:(UIDatePickerMode) mode withInitialDate:(long) initialDateTime withMinimumDate:(long) min withMaximumDate:(long) max tagPtr:(void*)tagPtr;

// callback methods
- (void)alertController:(UIAlertController*)alertController didSelectAction:(UIAlertAction*)action;

@end
