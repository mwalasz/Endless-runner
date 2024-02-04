//
//  NPUnityDateComponents.mm
//  Native Plugins
//
//  Created by Ashwin kumar on 22/01/19.
//  Copyright (c) 2019 Voxel Busters Interactive LLP. All rights reserved.
//

#import "NPUnityDateComponents.h"

NSCalendarIdentifier NPGetCalendarIdentifier(Calendar calendar)
{
    switch (calendar) {
        case CalendarBuddhist:
            return NSCalendarIdentifierBuddhist;
            
        case CalendarChinese:
            return NSCalendarIdentifierChinese;

        case CalendarCoptic:
            return NSCalendarIdentifierCoptic;

        case CalendarEthiopicAmeteAlem:
            return NSCalendarIdentifierEthiopicAmeteAlem;

        case CalendarEthiopicAmeteMihret:
            return NSCalendarIdentifierEthiopicAmeteMihret;

        case CalendarGregorian:
            return NSCalendarIdentifierGregorian;

        case CalendarHebrew:
            return NSCalendarIdentifierHebrew;

        case CalendarIndian:
            return NSCalendarIdentifierIndian;

        case CalendarIslamic:
            return NSCalendarIdentifierIslamic;

        case CalendarIslamicCivil:
            return NSCalendarIdentifierIslamicCivil;

        case CalendarIslamicTabular:
            return NSCalendarIdentifierIslamicTabular;

        case CalendarIslamicUmmAlQura:
            return NSCalendarIdentifierIslamicUmmAlQura;

        case CalendarIso8601:
            return NSCalendarIdentifierISO8601;

        case CalendarJapanese:
            return NSCalendarIdentifierJapanese;

        case CalendarPersian:
            return NSCalendarIdentifierPersian;

        case CalendarRepublicOfChina:
            return NSCalendarIdentifierRepublicOfChina;

        default:
            break;
    }
}

Calendar NPGetCalendarEnum(NSCalendarIdentifier identifier)
{
    if ([identifier isEqualToString:NSCalendarIdentifierBuddhist])
    {
        return CalendarBuddhist;
    }
    if ([identifier isEqualToString:NSCalendarIdentifierChinese])
    {
        return CalendarChinese;
    }
    if ([identifier isEqualToString:NSCalendarIdentifierCoptic])
    {
        return CalendarCoptic;
    }
    if ([identifier isEqualToString:NSCalendarIdentifierEthiopicAmeteAlem])
    {
        return CalendarEthiopicAmeteAlem;
    }
    if ([identifier isEqualToString:NSCalendarIdentifierEthiopicAmeteMihret])
    {
        return CalendarEthiopicAmeteMihret;
    }
    if ([identifier isEqualToString:NSCalendarIdentifierGregorian])
    {
        return CalendarGregorian;
    }
    if ([identifier isEqualToString:NSCalendarIdentifierHebrew])
    {
        return CalendarHebrew;
    }
    if ([identifier isEqualToString:NSCalendarIdentifierIndian])
    {
        return CalendarIndian;
    }
    if ([identifier isEqualToString:NSCalendarIdentifierIslamic])
    {
        return CalendarIslamic;
    }
    if ([identifier isEqualToString:NSCalendarIdentifierIslamicCivil])
    {
        return CalendarIslamicCivil;
    }
    if ([identifier isEqualToString:NSCalendarIdentifierIslamicTabular])
    {
        return CalendarIslamicTabular;
    }
    if ([identifier isEqualToString:NSCalendarIdentifierIslamicUmmAlQura])
    {
        return CalendarIslamicUmmAlQura;
    }
    if ([identifier isEqualToString:NSCalendarIdentifierISO8601])
    {
        return CalendarIso8601;
    }
    if ([identifier isEqualToString:NSCalendarIdentifierJapanese])
    {
        return CalendarJapanese;
    }
    if ([identifier isEqualToString:NSCalendarIdentifierPersian])
    {
        return CalendarPersian;
    }
    if ([identifier isEqualToString:NSCalendarIdentifierRepublicOfChina])
    {
        return CalendarRepublicOfChina;
    }
    
    return (Calendar)0;
}

void NPUnityDateComponents::CopyProperties(NSDateComponents* dateComponents)
{
    this->calendar      = NPGetCalendarEnum([[dateComponents calendar] calendarIdentifier]);
    this->year          = [dateComponents year];
    this->month         = [dateComponents month];
    this->day           = [dateComponents day];
    this->hour          = [dateComponents hour];
    this->minute        = [dateComponents minute];
    this->second        = [dateComponents second];
    this->nanosecond    = [dateComponents nanosecond];
    this->weekday       = [dateComponents weekday];
    this->weekOfMonth   = [dateComponents weekOfMonth];
    this->weekOfYear    = [dateComponents weekOfYear];
}

NSDateComponents* NPUnityDateComponents::ToNSDateComponents()
{
    NSDateComponents*   dateComponents  = [[NSDateComponents alloc] init];
    if ((int)calendar != 0)
    {
        NSCalendar*     nsCalendar      = [NSCalendar calendarWithIdentifier:NPGetCalendarIdentifier(this->calendar)];
        [dateComponents setCalendar:nsCalendar];
    }
    if (this->year > 0)
    {
        [dateComponents setYear:this->year];
    }
    if (this->month > 0)
    {
        [dateComponents setMonth:this->month];
    }
    if (this->day > 0)
    {
        [dateComponents setDay:this->day];
    }
    [dateComponents setHour:this->hour];
    [dateComponents setMinute:this->minute];
    [dateComponents setSecond:this->second];
    [dateComponents setNanosecond:this->nanosecond];
    if (this->weekday > 0)
    {
        [dateComponents setWeekday:this->weekday];
    }
    if (this->weekOfMonth > 0)
    {
        [dateComponents setWeekOfMonth:this->weekOfMonth];
    }
    if (this->weekOfYear > 0)
    {
        [dateComponents setWeekOfYear:this->weekOfYear];
    }
    
    return dateComponents;
}
