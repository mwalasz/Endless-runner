//
//  NPShareSheetWrapper.h
//  Native Plugins
//
//  Created by Ashwin kumar on 22/01/19.
//  Copyright (c) 2019 Voxel Busters Interactive LLP. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "NPSharingServicesDataTypes.h"

@interface NPShareSheetWrapper : NSObject<UIPopoverPresentationControllerDelegate>

// callbacks
+ (void)setShareSheetClosedCallback:(ShareSheetClosedNativeCallback)callback;

- (void)addItem:(NSObject*)item;
- (void)showAtPosition:(CGPoint)position withAnimation:(BOOL)animated;

@end
