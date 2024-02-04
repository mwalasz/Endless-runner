//
//  NPUnityDataTypes.mm
//  Native Plugins
//
//  Created by Ashwin kumar on 22/01/19.
//  Copyright (c) 2019 Voxel Busters Interactive LLP. All rights reserved.
//

#import "NPUnityDataTypes.h"

NPUnityTextEncodingFormat const NPUnityTextEncodingFormatUTF8   = @"utf8";

/// <summary> Encodes characters using the UTF-16 encoding. (Readonly)</summary>
NPUnityTextEncodingFormat const NPUnityTextEncodingFormatUTF16  = @"utf16";

/// <summary> Encodes characters using the UTF-32 encoding. (Readonly)</summary>
NPUnityTextEncodingFormat const NPUnityTextEncodingFormatUTF32  = @"utf32";


NPArray::NPArray(int length)
{
    this->length    = length;
    this->ptr       = length > 0 ? (void**)calloc(length, sizeof(void*)) : nil;
}

NPArray::~NPArray()
{
    free(ptr);
}

void NPArray::setObjectAtIndex(int index, void* obj)
{
    this->ptr[index]    = obj;
}
