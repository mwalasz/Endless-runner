//
//  NPBindingHelper.h
//  Native Plugins
//
//  Created by Ashwin kumar on 22/01/19.
//  Copyright (c) 2019 Voxel Busters Interactive LLP. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>
#import <MobileCoreServices/MobileCoreServices.h>
#import "NPDefines.h"
#import "NPUnityDataTypes.h"

// common functions
char* NPCreateCStringFromNSString(NSString* nsString);
char* NPCreateCStringCopyFromNSString(NSString* nsString);
char* NPCreateCStringFromNSError(NSError* error);
char* NPCreateCStringCopyFromNSError(NSError* error);
NSArray<NSString*>* NPCreateArrayOfNSString(const char** array, int length);
NSString* NPCreateNSStringFromCString(const char* cString);
NSURL* NPCreateNSURLFromCString(const char* cString);
NPArray* NPCreateArrayOfCString(NSArray<NSString*>* array);
NSString* NPExtractTokenFromNSData(id token);
NPError NPCreateError(int code, NSString* description);
NPError NPNullError();

// array opearations
NPArray* NPCreateNativeArrayFromNSArray(NSArray* array);

// image operations
NSData* NPEncodeImageAsData(UIImage* image, UIImageEncodeType encodeType);
UIImage* NPCaptureScreenshotAsImage();
NSData* NPCaptureScreenshotAsData(UIImageEncodeType encodeType);
UIImage* NPCreateImage(void* dataArrayPtr, int dataLength);
UIImage* LoadImageFromResources(NSString* name, NSString* extension);

// convertor operations
NSString* NPConvertMimeTypeToUTType(NSString* mimeType);

// JSON methods
NSString* NPToJson(id object, NSError** error);
id NPFromJson(NSString* jsonString, NSError** error);

// formatter
NSNumberFormatter* NPGetBillingPriceFormatter(NSLocale* locale);
NSString* NPCreateNSStringFromNSDate(NSDate* date);
NSDate* NPCreateNSDateFromNSString(NSString* dateStr);

// rect
CGPoint GetLastTouchPosition();
void SetLastTouchPosition(CGPoint position);
CGRect NPConvertScreenSpaceRectToNormalisedRect(CGRect frame);
CGRect NPConvertNormalisedRectToScreenSpaceRect(CGRect normalisedRect);
NPUnityRect NPRectMake(CGRect rect);
CGPoint NPConverToNativePosition(float posX, float posY);

// color methods
NPUnityColor NPColorMake(float r, float g, float b, float a);
NPUnityColor NPColorCreateFromUIColor(UIColor* color);

// converter
NSString* NPGetTextEncodingName(NPUnityTextEncodingFormat format);
NPSize NPCreateSizeFromCGSize(CGSize size);

// briding methods
void* NPRetainWithOwnershipTransfer(id object);
void* NPRetain(void* objectPtr);
void NPRelease(void* objectPtr);
