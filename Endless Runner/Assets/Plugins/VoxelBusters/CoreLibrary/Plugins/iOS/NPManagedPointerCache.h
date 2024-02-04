//
//  NPManagedPointerCache.h
//  Native Plugins
//
//  Created by Ashwin kumar on 22/01/19.
//  Copyright (c) 2019 Voxel Busters Interactive LLP. All rights reserved.
//

#import <Foundation/Foundation.h>

@interface NPManagedPointerCache : NSObject

+ (NPManagedPointerCache*)sharedInstance;

- (void)addPointer:(void*)pointer forKey:(id)object;
- (void*)pointerForKey:(id)object;
- (void)removePointerForKey:(id)object;

@end
