//
//  ListenerWrapperBase.m
//  Unity-iPhone
//
//  Created by Ayyappa J on 06/10/22.
//

#import "NPListenerWrapperBase.h"

@implementation NPListenerWrapperBase
@synthesize tag;

-(id) initWithTag:(void*) tag
{
    self = [super init];
    self.tag = tag;
    
    return self;
}
@end
