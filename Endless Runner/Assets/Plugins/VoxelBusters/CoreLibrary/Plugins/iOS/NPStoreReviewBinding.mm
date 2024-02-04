//
//  NPStoreReviewBinding.mm
//  Native Plugins
//
//  Created by Ashwin kumar on 22/01/19.
//  Copyright (c) 2019 Voxel Busters Interactive LLP. All rights reserved.
//

#import <StoreKit/StoreKit.h>
#import "NPDefines.h"

NPBINDING DONTSTRIP bool NPStoreReviewCanUseDeepLinking()
{
    return SYSTEM_VERSION_LESS_THAN(@"10.3");
}

NPBINDING DONTSTRIP void NPStoreReviewRequestReview()
{
    [SKStoreReviewController requestReview];
}
