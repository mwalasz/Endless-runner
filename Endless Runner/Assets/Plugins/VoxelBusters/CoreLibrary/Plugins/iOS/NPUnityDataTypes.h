//
//  NPUnityDataTypes.h
//  Native Plugins
//
//  Created by Ashwin kumar on 22/01/19.
//  Copyright (c) 2019 Voxel Busters Interactive LLP. All rights reserved.
//

#include "NPDefines.h"
#include "NPUnityDateComponents.h"

typedef enum : NSInteger
{
    UIImageEncodeTypePNG,
    UIImageEncodeTypeJPEG,
} UIImageEncodeType;

struct NPUnityCircularRegion
{
    double  latitude;
    double  longitude;
    float   radius;
    NPIntPtr  regionIdPtr;
};
typedef struct NPUnityCircularRegion NPUnityCircularRegion;

struct NPUnityAttachment
{
    int     dataArrayLength;
    NPIntPtr  dataArrayPtr;
    NPIntPtr  mimeTypePtr;
    NPIntPtr  fileNamePtr;
};
typedef struct NPUnityAttachment NPUnityAttachment;

struct NPUnityRect
{
    float   x;
    float   y;
    float   width;
    float   height;
};
typedef struct NPUnityRect NPUnityRect;

struct NPUnityColor
{
    float   r;
    float   g;
    float   b;
    float   a;
};
typedef struct NPUnityColor NPUnityColor;

struct NPArray
{
    NPIntPtr* ptr;
    int     length;
    
    // constructors
    NPArray(int length);
    ~NPArray();
    
    // methods
    void setObjectAtIndex(int index, void* obj);
};
typedef struct NPArray NPArray;

struct NPArrayProxy
{
    NPIntPtr* ptr;
    int length;
};
typedef struct NPArrayProxy NPArrayProxy;

struct NPError
{
    int code;
    NPIntPtr description;
};
typedef struct NPError NPError;

struct NPSize
{
    float width;
    float height;
};
typedef struct NPSize NPSize;

typedef NSString* NPUnityTextEncodingFormat NS_EXTENSIBLE_STRING_ENUM;

/// <summary> Encodes characters using the UTF-8 encoding. (Readonly)</summary>
FOUNDATION_EXPORT NPUnityTextEncodingFormat const NPUnityTextEncodingFormatUTF8;

/// <summary> Encodes characters using the UTF-16 encoding. (Readonly)</summary>
FOUNDATION_EXPORT NPUnityTextEncodingFormat const NPUnityTextEncodingFormatUTF16;

/// <summary> Encodes characters using the UTF-32 encoding. (Readonly)</summary>
FOUNDATION_EXPORT NPUnityTextEncodingFormat const NPUnityTextEncodingFormatUTF32;
