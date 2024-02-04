//
//  NPUtilityBinding.mm
//  Native Plugins
//
//  Created by Ashwin kumar on 22/01/19.
//  Copyright (c) 2019 Voxel Busters Interactive LLP. All rights reserved.
//

#import "NPUtilityBinding.h"
#import "NPKit.h"

// static fields
static LoadImageNativeCallback    _loadImageCallback    = nil;

#pragma mark - Utility methods

void LoadTextureData(NSData* imageData, void* tagPtr)
{
    if (imageData)
    {
        _loadImageCallback((void *)[imageData bytes], (int)[imageData length], tagPtr);
    }
    else
    {
        _loadImageCallback(nil, 0, tagPtr);
    }
}

#pragma mark - Native binding methods

NPBINDING DONTSTRIP void NPUtilityRegisterCallbacks(LoadImageNativeCallback loadImageCallback)
{
    // cache
    _loadImageCallback      = loadImageCallback;
}

NPBINDING DONTSTRIP void NPUtilityLoadImage(void* nativeDataPtr, void* tagPtr)
{
    NSData*     imageData   = (__bridge NSData*)nativeDataPtr;
    LoadTextureData(imageData, tagPtr);
}

NPBINDING DONTSTRIP void NPUtilityTakeScreenshot(void* tagPtr)
{
    NSData*     imageData   = NPCaptureScreenshotAsData(UIImageEncodeTypePNG);
    LoadTextureData(imageData, tagPtr);
}

NPBINDING DONTSTRIP void NPUtilityRetainObject(void* nativePtr)
{
    NPRetain(nativePtr);
}

NPBINDING DONTSTRIP void NPUtilityReleaseObject(void* nativePtr)
{
    NPRelease(nativePtr);
}

NPBINDING DONTSTRIP void NPUtilityOpenSettings()
{
    [[UIApplication sharedApplication] openURL:[NSURL URLWithString:UIApplicationOpenSettingsURLString]
                                       options:@{}
                             completionHandler:^(BOOL success) {
                                 // done
                             }];
}

NPBINDING DONTSTRIP void NPUtilityFreeCPointerObject(NPIntPtr nativePtr)
{
    free(nativePtr);
}

NPBINDING DONTSTRIP void NPUtilitySetLastTouchPosition(float posX, float posY)
{
    SetLastTouchPosition(NPConverToNativePosition(posX, posY));
}


