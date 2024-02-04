//
//  NPUnityDateComponents.h
//  Native Plugins
//
//  Created by Ashwin kumar on 22/01/19.
//  Copyright (c) 2019 Voxel Busters Interactive LLP. All rights reserved.
//

// custom types
typedef enum : int
{
    CalendarGregorian,
    CalendarBuddhist = 1,
    CalendarChinese,
    CalendarCoptic,
    CalendarEthiopicAmeteMihret,
    CalendarEthiopicAmeteAlem,
    CalendarIso8601,
    CalendarHebrew,
    CalendarIndian,
    CalendarIslamic,
    CalendarIslamicCivil,
    CalendarJapanese,
    CalendarPersian,
    CalendarRepublicOfChina,
    CalendarIslamicTabular,
    CalendarIslamicUmmAlQura,
} Calendar;

// methods
NSCalendarIdentifier NPGetCalendarIdentifier(Calendar calendar);
Calendar NPGetCalendarEnum(NSCalendarIdentifier identifier);

struct NPUnityDateComponents
{
    // fields
    Calendar calendar;
    long year;
    long month;
    long day;
    long hour;
    long minute;
    long second;
    long nanosecond;
    long weekday;
    long weekOfMonth;
    long weekOfYear;
    
    // methods
    void CopyProperties(NSDateComponents* dateComponents);
    NSDateComponents* ToNSDateComponents();
};
typedef struct NPUnityDateComponents NPUnityDateComponents;
