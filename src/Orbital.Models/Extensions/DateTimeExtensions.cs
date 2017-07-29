using System;
using NodaTime;

namespace Orbital.Models.Extensions
{
    public static class DateTimeExtensions
    {
        public static LocalDate ToLocalDate(this DateTime date)
        {            
            return new LocalDate(date.Year, date.Month, date.Day);
        }
        public static LocalDate? ToLocalDate(this DateTime? date)
        {
            return date?.ToLocalDate();
        }
    }
    
    public static class LocalDateExtensions
    {
        public static DateTime ToDateTime(this LocalDate date)
        {
            return new DateTime(date.Year, date.Month, date.Day);
        }
        public static DateTime? ToDateTime(this LocalDate? date)
        {
            return date?.ToDateTime();
        }
    }
}
