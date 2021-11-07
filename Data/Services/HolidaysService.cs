using HCMApp.Data.Models;
using HCMApp.Data.ViewModels;
using Nager.Date;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HCMApp.Data.Services
{
    public class HolidaysService
    {
        private readonly AppDbContext context;

        public HolidaysService(AppDbContext context)
        {
            this.context = context;
        }

        public List<Holiday> GetHolidaysForTheYear(int year)
        {
            var publicHolidays = DateSystem.GetPublicHolidays(year, CountryCode.BG);
            List<Holiday> localHolidays = new List<Holiday>();

            foreach (var holiday in publicHolidays)
            {
                if (!holiday.Date.Equals(DateTime.Parse(year + "-11-01")))
                {
                    if (holiday.Date.DayOfWeek.Equals(DayOfWeek.Saturday) || holiday.Date.DayOfWeek.Equals(DayOfWeek.Sunday))
                    {
                        DateTime actualAbsenceDay = DateTime.Now;
                        if (holiday.Date.DayOfWeek.Equals(DayOfWeek.Saturday))
                        {
                            actualAbsenceDay = holiday.Date.AddDays(2);
                        }
                        else
                        {
                            actualAbsenceDay = holiday.Date.AddDays(1);
                        }

                        var localHoliday = new Holiday()
                        {
                            HolidayName = holiday.LocalName,
                            HolidayDate = holiday.Date,
                            ActualAbsenceDay = actualAbsenceDay
                        };
                        if (localHoliday.HolidayName.Equals("Великден"))
                        {
                            localHoliday.ActualAbsenceDay = null;
                        }
                        localHolidays.Add(localHoliday);
                    }
                    else
                    {
                        var localHoliday = new Holiday()
                        {
                            HolidayName = holiday.LocalName,
                            HolidayDate = holiday.Date
                        };
                        localHolidays.Add(localHoliday);
                    }
                }
            }

            /*foreach (var holiday in localHolidays)
            {
                for (int i = localHolidays.Count - 1; i > 0; i--)
                {
                    if (localHolidays[i].HolidayDate.CompareTo(holiday.ActualAbsenceDay) == 0)
                    {
                        holiday.ActualAbsenceDay = localHolidays[i].HolidayDate.AddDays(1);
                    }
                }
            }*/

            for (int i = 0; i < localHolidays.Count; i++)
            {
                foreach (var holiday in localHolidays)
                {

                    while (localHolidays[i].HolidayDate.CompareTo(holiday.ActualAbsenceDay) == 0)
                    {
                        holiday.ActualAbsenceDay = localHolidays[i].HolidayDate.AddDays(1);
                    }
                }
            }

            for (int i = localHolidays.Count - 1; i > 0; i--)
            {

                if (localHolidays[i].ActualAbsenceDay != null && localHolidays[i-1].ActualAbsenceDay != null)
                {
                        if ((localHolidays[i].ActualAbsenceDay.Value.CompareTo(localHolidays[i-1].ActualAbsenceDay) == 0))
                        {
                            localHolidays[i].ActualAbsenceDay = localHolidays[i-1].ActualAbsenceDay.Value.AddDays(1);
                        }
                    
                }
            }

            return localHolidays;
        }

        public List<CalendarEventVM> GetCalendarEventVMsForCurrentYear()
        {
            List<Holiday> localHolidays = GetHolidaysForTheYear(DateTime.UtcNow.Year);

            List<CalendarEventVM> calendarHolidays = new List<CalendarEventVM>();

            foreach(var holiday in localHolidays)
            {
                calendarHolidays.Add(new CalendarEventVM()
                {
                    title = holiday.HolidayName,
                    start = holiday.HolidayDate.ToString("yyyy-MM-dd"),
                    description = holiday.HolidayName
                });

                if(holiday.ActualAbsenceDay != null)
                {
                    calendarHolidays.Add(new CalendarEventVM()
                    {
                        title = holiday.HolidayName,
                        start = holiday.ActualAbsenceDay.Value.ToString("yyyy-MM-dd"),
                        description = holiday.HolidayName
                    });
                }
            }

            return calendarHolidays;
        }

        public List<DateTime> GetHolidayDatesForTheYear(int year)
        {
            List<Holiday> holidays = GetHolidaysForTheYear(year);
            List<DateTime> holidaysDates = new List<DateTime>();

            foreach(var holiday in holidays)
            {
                if(holiday.ActualAbsenceDay == null)
                {
                    holidaysDates.Add(holiday.HolidayDate);
                } else
                {
                    holidaysDates.Add(holiday.ActualAbsenceDay.Value);
                }
            }

            return holidaysDates;
        }

        public List<HolidayVM> GetHolidayVMsForTheYear(int year)
        {
            return GetHolidaysForTheYear(year).Select(holiday => new HolidayVM()
            {
                HolidayName = holiday.HolidayName,
                HolidayDate = holiday.HolidayDate,
                ActualAbsenceDay = holiday.ActualAbsenceDay
            }).ToList();
        }
    }
}
