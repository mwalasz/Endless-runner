//
//  UIView+LayoutConstraints.mm
//  Native Plugins
//
//  Created by Ashwin kumar on 22/01/19.
//  Copyright (c) 2019 Voxel Busters Interactive LLP. All rights reserved.
//

#import "UIView+LayoutConstraints.h"

@implementation UIView (LayoutConstraints)

- (void)removeExistingConstraints
{
    __weak UIView* parentView   = self.superview;
    __weak UIView* view         = self;
    for (NSLayoutConstraint* constraint in parentView.constraints)
    {
        if (constraint.firstItem == view)
        {
            constraint.active   = false;
            [parentView removeConstraint:constraint];
        }
    }
}

@end
