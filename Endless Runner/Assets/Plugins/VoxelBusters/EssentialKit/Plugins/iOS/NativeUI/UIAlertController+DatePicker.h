//
//  UIAlertController+DatePicker.h
//  Unity-iPhone
//
//  Created by Ayyappa J on 02/12/20.
//

#import <UIKit/UIKit.h>

NS_ASSUME_NONNULL_BEGIN

typedef void(^DatePickerFinishCallback)(NSDate* _Nullable, void* tagPtr);

@interface UIAlertController (DatePicker)
+ (instancetype)create:(UIDatePickerMode) mode withInitialDate:(NSDate*) initialDateTime withMinimumDate:(NSDate*) minimumDate withMaximumDate:(NSDate*) maximumDate withTag:(void*) tagPtr withCallback:(DatePickerFinishCallback) callback;
@end

NS_ASSUME_NONNULL_END
