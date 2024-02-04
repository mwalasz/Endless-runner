//
//  NPManagedPointerCache.mm
//  Native Plugins
//
//  Created by Ashwin kumar on 22/01/19.
//  Copyright (c) 2019 Voxel Busters Interactive LLP. All rights reserved.
//

#import "NPManagedPointerCache.h"

// static fields
static NPManagedPointerCache*                       _sharedInstance     = nil;

@interface NPManagedPointerCache ()

@property(nonatomic, strong) NSMutableDictionary*   tagCollection;

@end

@implementation NPManagedPointerCache

@synthesize tagCollection = _tagCollection;

+ (NPManagedPointerCache*)sharedInstance
{
    if (nil == _sharedInstance)
    {
        _sharedInstance = [[NPManagedPointerCache alloc] init];
    }
    
    return _sharedInstance;
}

- (id)init
{
    self    = [super init];
    if (self)
    {
        // initialise
        self.tagCollection  = [[NSMutableDictionary alloc] init];
    }
    
    return self;
}

- (void)addPointer:(void*)pointer forKey:(id)object
{
    [self.tagCollection setObject:[NSValue valueWithPointer:pointer] forKey:[NSValue valueWithNonretainedObject:object]];
}

- (void*)pointerForKey:(id)object
{
    NSValue*    value   = [self.tagCollection objectForKey:[NSValue valueWithNonretainedObject:object]];
    return [value pointerValue];
}

- (void)removePointerForKey:(id)object
{
    [self.tagCollection removeObjectForKey:[NSValue valueWithNonretainedObject:object]];
}

@end
