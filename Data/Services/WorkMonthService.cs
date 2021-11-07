using HCMApp.Data.Models;
using HCMApp.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace HCMApp.Data.Services
{
    public class WorkMonthService
    {
        private readonly IWorkMonthsRepository workMonthsRepository;
        private readonly HolidaysService holidaysService;

        public WorkMonthService(IWorkMonthsRepository workMonthsRepository,
            HolidaysService holidaysService)
        {
            this.workMonthsRepository = workMonthsRepository;
            this.holidaysService = holidaysService;
        }

        public List<WorkMonth> GetWorkMonthsForTheYear(int year)
        {
            List<WorkMonth> workMonths = new List<WorkMonth>();

            var holidays = holidaysService.GetHolidayDatesForTheYear(year);

            for (int i = 1; i <= 12; i++)
            {
                var monthNum = i;
                var daysInMonth = DateTime.DaysInMonth(year, monthNum);
                var businessDaysInMonth = daysInMonth;

                for (int k = 1; k <= daysInMonth; k++)
                {
                   DateTime date;
                   if(monthNum<10)
                    {
                        if(k<10)
                        {
                            date = DateTime.ParseExact("0" + k + "/0" + monthNum + "/" + year, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        }
                        else
                        {
                            date = DateTime.ParseExact(k + "/0" + monthNum + "/" + year, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        }
                        
                    }
                    else
                    {
                        if (k < 10)
                        {
                            date = DateTime.ParseExact("0" + k + "/" + monthNum + "/" + year, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        }
                        else
                        {
                            date = DateTime.ParseExact(k + "/" + monthNum + "/" + year, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        }
                    }
                    

                    if (date.DayOfWeek.Equals(DayOfWeek.Saturday) || date.DayOfWeek.Equals(DayOfWeek.Sunday))
                    {
                        businessDaysInMonth--;
                    }
                    else
                    {
                        if(holidays.Contains(date))
                        {
                            businessDaysInMonth--;
                        }
                    }
                }
                workMonths.Add(new WorkMonth()
                {
                    MonthNumber = i,
                    Year = year,
                    WorkDays = businessDaysInMonth
                });
            }

            return workMonths;
        }

        public WorkMonth GetWorkMonth(int month, int year)
        {
            var workMonths = workMonthsRepository.GetWorkMonths();
            return workMonths.Where(n => n.MonthNumber == month && n.Year == year).SingleOrDefault();
        }
    }
}
