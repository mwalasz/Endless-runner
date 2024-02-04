//
//  NPBindingHelper.mm
//  Native Plugins
//
//  Created by Ashwin kumar on 22/01/19.
//  Copyright (c) 2019 Voxel Busters Interactive LLP. All rights reserved.
//

#import "NPBindingHelper.h"
#import "NPDefines.h"
#import "UnityView.h"

#if !defined(MAKE_UNITY_VERSION)
#define MAKE_UNITY_VERSION(ver, maj, min) ((ver)*10000 + (maj)*100 + (min))
#endif

static NSMutableDictionary* _referenceHistory   = nil;
static CGPoint _lastTouchPosition = CGPointMake(0, 0);

UIImage* NPFixOrientation(UIImage* image, BOOL isOpaque);

#pragma mark - String operations

char* NPCreateCStringFromNSString(NSString* nsString)
{
    if (nsString)
    {
        return (char*)[nsString UTF8String];
    }
    
    return nil;
}

char* NPCreateCStringCopyFromNSString(NSString* nsString)
{
    if (nsString)
    {
        // create copy
        const char* cString = [nsString UTF8String];
        char*       copy    = (char*)malloc(strlen(cString) + 1);
        strcpy(copy, cString);
        return copy;
    }
    
    return nil;
}

char* NPCreateCStringFromNSError(NSError* error)
{
    if (error)
    {
        return (char*)[[error description] UTF8String];
    }
    
    return nil;
}

NPError NPCreateError(int code, NSString* description)
{
    NPError error;
    error.code = code;
    error.description = (void*)[description UTF8String];
    return error;
}

NPError NPNullError()
{
    NPError error = {-1, NULL};
    return error;
}


char* NPCreateCStringCopyFromNSError(NSError* error)
{
    if (error)
    {
        return NPCreateCStringCopyFromNSString([error localizedDescription]);
    }
    
    return nil;
}

NSArray<NSString*>* NPCreateArrayOfNSString(const char** array, int length)
{
    if (array)
    {
        NSMutableArray<NSString*>*  nativeArray     = [NSMutableArray<NSString*> arrayWithCapacity:length];
        for (int iter = 0; iter < length; iter++)
        {
            const char* cValue  = array[iter];
            [nativeArray addObject:[NSString stringWithUTF8String:cValue]];
        }
        
        return nativeArray;
    }
    
    return nil;
}

NSString* NPCreateNSStringFromCString(const char* cString)
{
    if (cString)
    {
        return [NSString stringWithUTF8String:cString];
    }
    
    return nil;
}

NSURL* NPCreateNSURLFromCString(const char* cString)
{
    if (cString)
    {
        NSString*   urlString   = [NSString stringWithUTF8String:cString];
        return [NSURL URLWithString:urlString];
    }
    
    return nil;
}

NPArray* NPCreateArrayOfCString(NSArray<NSString*>* array)
{
    if (array)
    {
        int         length      = (int)[array count];
        NPArray*    nativeArray = new NPArray(length);
        for (int iter = 0; iter < length; iter++)
        {
            const char* item    = [[array objectAtIndex:iter] UTF8String];
            nativeArray->setObjectAtIndex(iter, (void*)item);
        }
        
        return nativeArray;
    }
    else
    {
        return new NPArray(-1);
    }
}

NSString* NPConvertDataToHexString(NSData* data)
{
    NSUInteger len = data.length;
    if (len == 0)
    {
        return nil;
    }
    
    const unsigned char*    buffer      = (const unsigned char*)[data bytes];
    NSMutableString*        hexString   = [NSMutableString stringWithCapacity:(len * 2)];
    for (int i = 0; i < len; ++i)
    {
        [hexString appendFormat:@"%02x", buffer[i]];
    }
    return [hexString copy];
}

NSString* NPExtractTokenFromNSData(id token)
{
    if (SYSTEM_VERSION_GREATER_THAN_OR_EQUAL_TO(@"13"))
    {
        return NPConvertDataToHexString(token);
    }
    else
    {
        NSString*   tokenStr    = [token description];
        tokenStr                = [tokenStr stringByTrimmingCharactersInSet:[NSCharacterSet characterSetWithCharactersInString:@"<>"]];
        tokenStr                = [tokenStr stringByReplacingOccurrencesOfString:@" " withString:@""];
        return tokenStr;
    }
}

#pragma mark - Array operations

NPArray* NPCreateNativeArrayFromNSArray(NSArray* array)
{
    if (array)// && [array count] > 0
    {
        int         length      = (int)[array count];
        NPArray*    nativeArray = new NPArray(length);
        
        for (int iter = 0; iter < length; iter++)
        {
            if([array[iter] isKindOfClass:[NSValue class]])
            {
                NSValue *value = (NSValue*)array[iter];
                nativeArray->setObjectAtIndex(iter, value.pointerValue);
            }
            else
            {
                nativeArray->setObjectAtIndex(iter, (__bridge void*)array[iter]);
            }
        }
 
        return nativeArray;
    }
    else
    {
        return new NPArray(-1);
    }
}

#pragma mark - Image operations

NSData* NPEncodeImageAsData(UIImage* image, UIImageEncodeType encodeType)
{
    switch (encodeType)
    {
        case UIImageEncodeTypePNG:
            return UIImagePNGRepresentation(NPFixOrientation(image, false));
            
        case UIImageEncodeTypeJPEG:
            return UIImageJPEGRepresentation(NPFixOrientation(image, true), 1);
            
        default:
            return nil;
    }
}

UIImage* NPFixOrientation(UIImage* image, bool isOpaque) //This is used as unity fails to consider exif flags for jpegs and also UIImageRepresentation fails to save orientation info when saved in png format. One shot - Two birds :D
{
    if (image.imageOrientation == UIImageOrientationUp) return image;
    CGSize size = image.size;
    float scale = image.scale;
    
    UIGraphicsBeginImageContextWithOptions(size, isOpaque, scale);
    [image drawInRect:(CGRect){0, 0, size}];
    UIImage *orientedImage = UIGraphicsGetImageFromCurrentImageContext();
    UIGraphicsEndImageContext();
    return orientedImage;
}

UIImage* NPCaptureScreenshotAsImage()
{
    UIView*     glView  = UnityGetGLView();
    CGRect      bounds  = glView.bounds;
    
    // write contents of view to context and create image using it
    UIGraphicsBeginImageContextWithOptions(bounds.size, YES, 0.0);
    [glView drawViewHierarchyInRect:bounds afterScreenUpdates:YES];
    UIImage *image = UIGraphicsGetImageFromCurrentImageContext();
    UIGraphicsEndImageContext();
    
    return image;
}

NSData* NPCaptureScreenshotAsData(UIImageEncodeType encodeType)
{
    switch (encodeType)
    {
        case UIImageEncodeTypePNG:
            return UIImagePNGRepresentation(NPCaptureScreenshotAsImage());
            
        case UIImageEncodeTypeJPEG:
            return UIImageJPEGRepresentation(NPCaptureScreenshotAsImage(), 1);
            
        default:
            return nil;
    }
}

UIImage* NPCreateImage(void* dataArrayPtr, int dataLength)
{
    NSData* data = [NSData dataWithBytes:dataArrayPtr length:dataLength];
    return [UIImage imageWithData:data];
}

UIImage* LoadImageFromResources(NSString* name, NSString* extension)
{
#if UNITY_VERSION < MAKE_UNITY_VERSION(2019,3,0)
    return [UIImage imageNamed:[NSString stringWithFormat:@"%@.%@", name, extension]];
#else
    NSBundle*   bundle          = [NSBundle bundleForClass:[UnityView class]];
    NSString*   resourcePath    = [bundle pathForResource:name ofType:extension];
    return [UIImage imageWithContentsOfFile:resourcePath];
#endif
}

#pragma mark - Converter operations

NSString* NPConvertMimeTypeToUTType(NSString* mimeType)
{
    if ([mimeType isEqualToString:kMimeTypePNG])
    {
        return (NSString*)kUTTypePNG;
    }
    if ([mimeType isEqualToString:kMimeTypeJPG])
    {
        return (NSString*)kUTTypeJPEG;
    }
    
    return NULL;
}

#pragma mark - JSON utility methods

NSString* NPToJson(id object, NSError** error)
{
    if (object)
    {
        NSData* jsonData    = [NSJSONSerialization dataWithJSONObject:object
                                                              options:0
                                                                error:error];
        
        if (*error == nil)
        {
            return [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
        }
    }
    
    return nil;
}

id NPFromJson(NSString* jsonString, NSError** error)
{
    if (jsonString)
    {
        NSData* jsonData    = [jsonString dataUsingEncoding:NSUTF8StringEncoding];
        if (jsonData)
        {
            id  object      = [NSJSONSerialization JSONObjectWithData:jsonData
                                                                  options:0
                                                                    error:error];
            if (*error == nil)
            {
                return object;
            }
        }
    }
    
    return nil;
}

#pragma mark - Formatters

static NSNumberFormatter* _billingPriceFormatter = nil;
NSNumberFormatter* NPGetBillingPriceFormatter(NSLocale* locale)
{
    static dispatch_once_t sharedBlock;
    dispatch_once(&sharedBlock, ^{
        NSNumberFormatter*  priceFormatter  = [[NSNumberFormatter alloc] init];
        [priceFormatter setFormatterBehavior:NSNumberFormatterBehavior10_4];
        [priceFormatter setNumberStyle:NSNumberFormatterCurrencyStyle];
        [priceFormatter setLocale:locale];

        // store reference
        _billingPriceFormatter  = priceFormatter;
    });
    
    return _billingPriceFormatter;
}

static NSDateFormatter* _dateFormatter = nil;
NSDateFormatter* GetDateFormatter()
{
    static dispatch_once_t onceToken;
    dispatch_once(&onceToken, ^{
        NSDateFormatter*        dateFormatter   = [[NSDateFormatter alloc] init];
        [dateFormatter setLocale:[NSLocale localeWithLocaleIdentifier:@"en_US_POSIX"]];
        [dateFormatter setTimeZone:[NSTimeZone timeZoneWithAbbreviation:@"UTC"]];
        [dateFormatter setDateFormat:@"yyyy-MM-dd HH:mm:ss Z"];
        
        // save reference
        _dateFormatter  = dateFormatter;
    });
    
    return _dateFormatter;
}

NSString* NPCreateNSStringFromNSDate(NSDate* date)
{
    if (date)
    {
        __weak  NSDateFormatter*    dateFormatter   = GetDateFormatter();
        return [dateFormatter stringFromDate:date];
    }
    
    return nil;
}

NSDate* NPCreateNSDateFromNSString(NSString* dateStr)
{
    if (dateStr)
    {
        __weak  NSDateFormatter*    dateFormatter   = GetDateFormatter();
        return [dateFormatter dateFromString:dateStr];
    }
    
    return nil;
}

#pragma mark - Rect functions

CGFloat GetStatusBarHeight()
{
    UIApplication*  sharedApplication   = [UIApplication sharedApplication];
    if (![sharedApplication isStatusBarHidden])
    {
        CGRect      statusBarFrame      = [sharedApplication statusBarFrame];
        return statusBarFrame.size.height;
    }
    
    return 0;
}

CGPoint GetLastTouchPosition()
{
    return _lastTouchPosition;
}

void SetLastTouchPosition(CGPoint position)
{
    _lastTouchPosition  = position;
}

CGRect NPConvertScreenSpaceRectToNormalisedRect(CGRect frame)
{
    CGRect  screenBounds        = [[UIScreen mainScreen] bounds];
    // convert frame to normalised rect
    CGRect  normalisedRect;
    normalisedRect.origin.x     = (CGRectGetMinX(frame) - CGRectGetMinX(screenBounds)) / CGRectGetWidth(screenBounds);
    normalisedRect.origin.y     = (CGRectGetMinY(frame) - CGRectGetMinY(screenBounds)) / CGRectGetHeight(screenBounds);
    normalisedRect.size.width   = CGRectGetWidth(frame) / CGRectGetWidth(screenBounds);
    normalisedRect.size.height  = CGRectGetHeight(frame) / CGRectGetHeight(screenBounds);
    
    return normalisedRect;
}

CGRect NPConvertNormalisedRectToScreenSpaceRect(CGRect normalisedRect)
{
    CGRect  screenBounds        = [[UIScreen mainScreen] bounds];
    // calculate frame
    CGRect  newFrame;
    newFrame.origin.x           = (CGRectGetMinX(normalisedRect) * CGRectGetWidth(screenBounds)) + CGRectGetMinX(screenBounds);
    newFrame.origin.y           = (CGRectGetMinY(normalisedRect) * CGRectGetHeight(screenBounds)) + CGRectGetMinY(screenBounds);
    newFrame.size.width         = CGRectGetWidth(normalisedRect) * CGRectGetWidth(screenBounds);
    newFrame.size.height        = CGRectGetHeight(normalisedRect) * CGRectGetHeight(screenBounds);
    
    return newFrame;
}

NPUnityRect NPRectMake(CGRect rect)
{
    NPUnityRect npRect;
    npRect.x        = rect.origin.x;
    npRect.y        = rect.origin.y;
    npRect.width    = rect.size.width;
    npRect.height   = rect.size.height;
    
    return npRect;
}

CGPoint NPConverToNativePosition(float posX, float posY)
{
    float   contentScale    = [[UIScreen mainScreen] scale];
    return CGPointMake(posX / contentScale, posY / contentScale);
}

#pragma mark - Color methods

NPUnityColor NPColorMake(float r, float g, float b, float a)
{
    NPUnityColor npColor;
    npColor.r   = r;
    npColor.g   = g;
    npColor.b   = b;
    npColor.a   = a;
    return npColor;
}

NPUnityColor NPColorCreateFromUIColor(UIColor* color)
{
    CGFloat r, g, b, a;
    [color getRed:&r green:&g blue:&b alpha:&a];
    return NPColorMake((float)r, (float)g , (float)b, (float)a);
}

#pragma mark - Converter methods

NSString* NPGetTextEncodingName(NPUnityTextEncodingFormat format)
{
    if ([format isEqualToString:NPUnityTextEncodingFormatUTF8])
    {
        return @"utf-8";
    }
    if ([format isEqualToString:NPUnityTextEncodingFormatUTF16])
    {
        return @"utf-16";
    }
    if ([format isEqualToString:NPUnityTextEncodingFormatUTF32])
    {
        return @"utf-32";
    }
    
    return nil;
}

NPSize NPCreateSizeFromCGSize(CGSize size)
{
    NPSize npSize;
    npSize.width = size.width;
    npSize.height = size.height;
    
    return npSize;
}

#pragma mark - Reference management methods

void AddObjectReference(void* object, bool* isNew)
{
    // ensure that collection object is valid
    if (_referenceHistory == nil)
    {
        _referenceHistory = [NSMutableDictionary dictionary];
    }
    
    // add object to collection
    id          key     = [NSValue valueWithPointer:(const void*)object];
    NSNumber*   value   = [_referenceHistory objectForKey:key];
    if (value == nil)
    {
        [_referenceHistory setObject:[NSNumber numberWithInt:1] forKey:key];
        *isNew          = true;
    }
    else
    {
        [_referenceHistory setObject:[NSNumber numberWithInt:[value intValue] + 1] forKey:key];
        *isNew          = false;
    }
}

void* NPRetainWithOwnershipTransfer(id object)
{
    void*   objectPtr;
    bool    isNew;
    AddObjectReference((__bridge void*)object, &isNew);
    if (isNew)
    {
        objectPtr   = (void*)CFBridgingRetain(object);
    }
    else
    {
        objectPtr   = (void*)CFRetain((__bridge CFTypeRef)object);
    }

#if NATIVE_PLUGINS_DEBUG_ENABLED
    NSLog(@"[NativePlugins] Retaining ownership for pointer %@.", objectPtr);
#endif
    
    return objectPtr;
}

void* NPRetain(void* objectPtr)
{
    return NPRetainWithOwnershipTransfer((__bridge id)objectPtr);
}

void NPRelease(void* objectPtr)
{
    if (_referenceHistory == nil)
    {
#if NATIVE_PLUGINS_DEBUG_ENABLED
        NSLog(@"[NativePlugins] Fatal error! Collection object is not initialised. It seems like Release is called, before retain.");
#endif
        return;
    }

    id          key     = [NSValue valueWithPointer:(const void*)objectPtr];
    NSNumber*   value   = [_referenceHistory objectForKey:key];
    if (value == nil)
    {
#if NATIVE_PLUGINS_DEBUG_ENABLED
        NSLog(@"[NativePlugins] Fatal error! Object not found. It seems like Release is called, before retain.");
#endif
        return;
    }

    // check value
    if ([value intValue] == 1)
    {
#if NATIVE_PLUGINS_DEBUG_ENABLED
        NSLog(@"[NativePlugins] Release object ownership for pointer %@.", objectPtr);
#endif
        CFBridgingRelease(objectPtr);
        [_referenceHistory removeObjectForKey:key];
        return;
    }

    // update reference table
    CFRelease(objectPtr);
    [_referenceHistory setObject:[NSNumber numberWithInt:[value intValue] - 1] forKey:key];
}
