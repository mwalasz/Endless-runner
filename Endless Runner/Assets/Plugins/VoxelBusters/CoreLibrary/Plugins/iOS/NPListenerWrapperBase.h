//
//  ListenerWrapperBase.h
//  Unity-iPhone
//
//  Created by Ayyappa J on 06/10/22.
//

#import <Foundation/Foundation.h>

NS_ASSUME_NONNULL_BEGIN

@interface NPListenerWrapperBase : NSObject
@property (nonatomic, assign) void* tag;
-(id) initWithTag:(void*) tag;
@end

NS_ASSUME_NONNULL_END
