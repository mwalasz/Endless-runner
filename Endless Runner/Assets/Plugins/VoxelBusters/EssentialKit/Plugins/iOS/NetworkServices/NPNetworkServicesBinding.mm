//
//  NPNetworkServicesBinding.mm
//  Native Plugins
//
//  Created by Ashwin kumar on 22/01/19.
//  Copyright (c) 2019 Voxel Busters Interactive LLP. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <arpa/inet.h>
#import "Reachability.h"
#import "NPKit.h"
#import "NPNetworkServicesDataTypes.h"

static ReachabilityChangeNativeCallback     _internetReachabilityChangeCallback     = nil;
static ReachabilityChangeNativeCallback     _hostReachabilityChangeCallback         = nil;

#pragma mark - Custom definitions

@interface NetworkReachabilityObserver : NSObject

@property(nonatomic) NetworkStatus      internetReachabilityStatus;
@property(nonatomic) NetworkStatus      hostReachabilityStatus;
@property(nonatomic, copy) NSString*    hostAddress;

-(void)startReachabilityNotifier;
-(void)stopReachabilityNotifier;

@end

@interface NetworkReachabilityObserver ()

@property(nonatomic) Reachability*      internetReachability;
@property(nonatomic) Reachability*      hostReachability;

@end

@implementation NetworkReachabilityObserver

@synthesize internetReachabilityStatus  = _internetReachabilityStatus;
@synthesize internetReachability        = _internetReachability;

@synthesize hostAddress                 = _hostAddress;
@synthesize hostReachabilityStatus      = _hostReachabilityStatus;
@synthesize hostReachability            = _hostReachability;

-(id)init
{
    self    = [super init];
    if (self)
    {
        // set default values
        _internetReachabilityStatus     = NotReachable;
        _hostReachabilityStatus         = NotReachable;
        
        // register for notification
        [[NSNotificationCenter defaultCenter] addObserver:self
                                                 selector:@selector(reachabilityChanged:)
                                                     name:kNPReachabilityChangedNotification
                                                   object:nil];
    }
    return self;
}

-(void)dealloc
{
    // stop process
    [self stopReachabilityNotifier];

    // unregister from events
    [[NSNotificationCenter defaultCenter] removeObserver:self];
}

-(void)startReachabilityNotifier
{
    // stop existing process
    [self stopReachabilityNotifier];
    
    // create new process
    [self startInternetReachabilityNotifier];
    [self startHostReachabilityNotifier];
}

-(void)startInternetReachabilityNotifier
{
    // create new instance
    self.internetReachability   = [Reachability reachabilityForInternetConnection];
    [self.internetReachability startNotifier];
    [self updateInternetReachabilityStatus];
}

-(void)startHostReachabilityNotifier
{
    // check whether host address info is available
    if (!self.hostAddress)
        return;
    
    struct sockaddr_in6 addr;
    memset(&addr, 0, sizeof(struct sockaddr_in6));
    addr.sin6_len           = sizeof(struct sockaddr_in6);
    addr.sin6_family        = AF_INET6;
    inet_pton(AF_INET6, [_hostAddress UTF8String], &addr.sin6_addr);
    
    // create new instance
    self.hostReachability   = [Reachability reachabilityWithAddress:(const struct sockaddr*)&addr];
    [self.hostReachability startNotifier];
    [self updateHostReachabilityStatus];
}

-(void)stopReachabilityNotifier
{
    // stop active process
    [self stopReachabilityNotifier:self.internetReachability];
    [self stopReachabilityNotifier:self.hostReachability];
    
    // reset fields
    self.internetReachability   = nil;
    self.hostReachability       = nil;
}

-(void)stopReachabilityNotifier:(Reachability*)reachability
{
    if (reachability)
    {
        [reachability stopNotifier];
    }
}

#pragma mark - Setter methods

-(void)setInternetReachabilityStatus:(NetworkStatus)value
{
    if (value != _internetReachabilityStatus)
    {
        _internetReachabilityStatus = value;
        _internetReachabilityChangeCallback((value != NotReachable), value);
    }
}

-(void)setHostReachabilityStatus:(NetworkStatus)value
{
    if (value != _hostReachabilityStatus)
    {
        _hostReachabilityStatus     = value;
        _hostReachabilityChangeCallback((value != NotReachable), value);
    }
}

#pragma mark - Notification methods

- (void)reachabilityChanged:(NSNotification*)notification
{
    id object = [notification object];
    if ([object isKindOfClass:[Reachability class]])
    {
        if (object == self.internetReachability)
        {
            [self updateInternetReachabilityStatus];
        }
        else if (object == self.hostReachability)
        {
            [self updateHostReachabilityStatus];
        }
    }
}

- (void)updateInternetReachabilityStatus
{
    self.internetReachabilityStatus     = [self.internetReachability currentReachabilityStatus];
}

- (void)updateHostReachabilityStatus
{
    self.hostReachabilityStatus         = [self.hostReachability currentReachabilityStatus];
}

@end

#pragma mark - Native binding methods

static NetworkReachabilityObserver* _sharedObserver   = nil;

NPBINDING DONTSTRIP void NPNetworkServicesRegisterCallbacks(ReachabilityChangeNativeCallback internetReachabilityChangeCallback, ReachabilityChangeNativeCallback hostReachabilityChangeCallback)
{
    _internetReachabilityChangeCallback = internetReachabilityChangeCallback;
    _hostReachabilityChangeCallback     = hostReachabilityChangeCallback;
}

NPBINDING DONTSTRIP void NPNetworkServicesInit(const char* hostAddress)
{
    if (!_sharedObserver)
    {
        _sharedObserver = [[NetworkReachabilityObserver alloc] init];
        [_sharedObserver setHostAddress:NPCreateNSStringFromCString(hostAddress)];
    }
}

NPBINDING DONTSTRIP void NPNetworkServicesStartReachabilityNotifier()
{
    [_sharedObserver startReachabilityNotifier];
}

NPBINDING DONTSTRIP void NPNetworkServicesStopReachabilityNotifier()
{
    [_sharedObserver stopReachabilityNotifier];
}

NPBINDING DONTSTRIP NetworkStatus NPNetworkServicesGetInternetReachabilityStatus()
{
    return _sharedObserver.internetReachabilityStatus;
}

NPBINDING DONTSTRIP NetworkStatus NPNetworkServicesGetHostReachabilityStatus()
{
    return _sharedObserver.hostReachabilityStatus;
}
