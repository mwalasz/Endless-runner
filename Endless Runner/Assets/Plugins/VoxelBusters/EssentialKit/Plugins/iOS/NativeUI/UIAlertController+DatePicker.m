//
//  NPDatePickerViewController.m
//  Unity-iPhone
//
//  Created by Ayyappa J on 02/12/20.
//

#import "UIAlertController+DatePicker.h"
#import "UIViewController+Presentation.h"
#import "UIView+LayoutConstraints.h"

static const float       kOffset = 26.0f;

@implementation UIAlertController (DatePicker)


+ (id)create:(UIDatePickerMode) mode withInitialDate:(NSDate*) initialDate withMinimumDate:(NSDate*) minimumDate withMaximumDate:(NSDate*) maximumDate withTag:(nonnull void *)tagPtr withCallback:(DatePickerFinishCallback) callback;
{
    UIAlertController *controller = [UIAlertController alertControllerWithTitle:@"Select" message:nil preferredStyle:UIAlertControllerStyleActionSheet];
    [controller setup:mode withInitialDate:initialDate withMinimumDate:minimumDate withMaximumDate:maximumDate withTag:tagPtr withCallback:callback];
    return controller;
}

- (void)setup:(UIDatePickerMode) mode withInitialDate:(NSDate*) initialDate withMinimumDate:(NSDate*) minimumDate withMaximumDate:(NSDate*) maximumDate withTag:(nonnull void *)tagPtr withCallback:(DatePickerFinishCallback) callback;
{
    UIDatePicker *datePicker = [[UIDatePicker alloc] init];

    #ifdef __IPHONE_13_4
    if (@available(iOS 13.4, *)) {
        datePicker.preferredDatePickerStyle = UIDatePickerStyleWheels;
    }
    #endif
    
    //[self.view setBackgroundColor:[UIColor clearColor]];
    datePicker.datePickerMode = mode;

    [datePicker setDate:initialDate];
    [datePicker setMinimumDate:minimumDate];
    [datePicker setMaximumDate:maximumDate];
    [self.view addSubview: datePicker];

    
    UIAlertAction* selectAction = [UIAlertAction actionWithTitle:@"Ok" style:UIAlertActionStyleDefault
       handler:^(UIAlertAction * action) {
        NSLog(@"Date picker ok clicked");
        //[self dismissViewControllerAnimated:true completion:nil];
        
        if(callback != nil)
        {
            callback([datePicker date], tagPtr);
        }
        
    }];
    
    UIAlertAction* cancelAction = [UIAlertAction actionWithTitle:@"Cancel" style:UIAlertActionStyleCancel
       handler:^(UIAlertAction * action) {
        NSLog(@"Date picker cancel clicked");
        if(callback != nil)
        {
            callback(nil, tagPtr);
        }
    }];
     
    [self addAction:selectAction];
    [self addAction:cancelAction];

    
    [datePicker setTranslatesAutoresizingMaskIntoConstraints:false];
    [datePicker.widthAnchor constraintEqualToAnchor:self.view.widthAnchor].active = true;
    [datePicker.topAnchor constraintEqualToAnchor:self.view.topAnchor constant:kOffset].active = true;
    [datePicker.bottomAnchor constraintEqualToAnchor:self.view.bottomAnchor constant:   (UI_USER_INTERFACE_IDIOM() == UIUserInterfaceIdiomPad) ? -(kOffset * 2) : -(kOffset * 4)].active = true;
}
@end
