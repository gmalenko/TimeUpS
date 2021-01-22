using Itenso.TimePeriod;
using Nager.Date;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TimeUpS
{
    class Program
    {

        static void Main(string[] args)
        {
            var dateTimeInput = new DateTime();
            var minutesInput = 0;

            Console.Write("Start = ");
            string input = Console.ReadLine().Trim();
            dateTimeInput = Convert.ToDateTime(input);



            Console.Write("Minutes = ");
            input = Console.ReadLine().Trim();
            minutesInput = Convert.ToInt32(input);

            var result = GetEndDate(dateTimeInput, minutesInput);

            Console.WriteLine("Answer = " + result.ToString("dddd, MMMM dd, yyyy hh:mm:ss tt"));


        }
        public static Dictionary<DateTime, string> GenerateHolidays(int year)
        {
            var availableHolidays = new Dictionary<string, string>();
            availableHolidays.Add("Christmas Day", "");
            availableHolidays.Add("Thanksgiving Day", "");
            availableHolidays.Add("New Year's Day", "");
            availableHolidays.Add("Memorial Day", "");
            availableHolidays.Add("Independence Day", "");
            availableHolidays.Add("Labor Day", "");

            var dateDictionary = new Dictionary<DateTime, string>();

            var publicHolidays = DateSystem.GetPublicHoliday(year, "US");
            foreach (var holiday in publicHolidays)
            {
                if (availableHolidays.Keys.Contains(holiday.LocalName))
                {
                    dateDictionary.Add(holiday.Date, holiday.LocalName);
                };
            }

            return dateDictionary;
        }


        public static DateTime GetEndDate(DateTime start, int minutes)
        {
            CalendarDateAdd calendarDateAdd = new CalendarDateAdd();
            // weekdays
            calendarDateAdd.AddWorkingWeekDays();
            // holidays
            var holidays = GenerateHolidays(start.Year);
            foreach (var holiday in holidays)
            {
                calendarDateAdd.ExcludePeriods.Add(new Day(holiday.Key.Year, holiday.Key.Month, holiday.Key.Day));
            }

            calendarDateAdd.WorkingHours.Add(new HourRange(new Time(8, 0, 0), new Time(12, 0, 0, 0)));
            calendarDateAdd.WorkingHours.Add(new HourRange(new Time(13, 0, 0), new Time(17, 0, 0)));
            TimeSpan offset = new TimeSpan(0, minutes, 0); // 22 hours

            DateTime? end = calendarDateAdd.Add(start, offset);
            return end.Value;

        }


    }
}
