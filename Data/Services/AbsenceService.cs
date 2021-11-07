using HCMApp.Data.Models;
using HCMApp.Data.Repositories;
using HCMApp.Data.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace HCMApp.Data.Services
{
    public class AbsenceService
    {
        private readonly IAbsenceReasonsRepository absenceReasonsRepository;
        private readonly IUserRepository userRepository;
        private readonly AppDbContext context;
        private readonly IAbsenceRequestsRepository absenceRequestsRepository;
        private readonly HolidaysService holidaysService;

        public AbsenceService(IAbsenceReasonsRepository absenceReasonsRepository,
            IUserRepository userRepository,
            AppDbContext context,
            IAbsenceRequestsRepository absenceRequestsRepository,
            HolidaysService holidaysService)
        {
            this.absenceReasonsRepository = absenceReasonsRepository;
            this.userRepository = userRepository;
            this.context = context;
            this.absenceRequestsRepository = absenceRequestsRepository;
            this.holidaysService = holidaysService;
        }

        public List<AbsenceReasonVM> GetAbsenceReasonVMs() =>
            absenceReasonsRepository.GetAbsence_Reasons()
            .Select(absence_reason => new AbsenceReasonVM()
            {
                ReasonName = absence_reason.ReasonName
            }).OrderBy(n => n.ReasonName).ToList();

        public string AddAbsence(string username, AbsenceRequestVM absenceRequestVM)
        {
            var currentUser = userRepository.GetUserByUsername(username);

            var currentYearUserLeaveDaysAvailable = currentUser.Leave_Days.Where(n => n.Year.Equals(DateTime.UtcNow.Year)).Select(n => n.AvailableLeaveDays).SingleOrDefault();

            DateTime startDate = absenceRequestVM.FromDate;
            DateTime endDate = absenceRequestVM.ToDate;

            int LeaveDaysRequested = 0;
            LeaveDaysRequested = calculateBusinessDaysBetweenDates(startDate, endDate);

            if(absenceRequestVM.Reason.Equals("Платена отпуска"))
            {
                if (currentYearUserLeaveDaysAvailable < LeaveDaysRequested)
                {
                    return "Not enough days available";
                }
            }

            context.Absence_Requests.Add(new Absence_Request()
            {
                StartDate = startDate,
                EndTime = endDate,
                Status = "pending",
                UserId = currentUser.UserId,
                Absence_ReasonId = absenceReasonsRepository.GetAbsence_Reasons()
                                    .Where(n => n.ReasonName.ToLower().Equals(absenceRequestVM.Reason.ToLower()))
                                    .SingleOrDefault().Id
            });

            context.SaveChanges();

            return "Success";
        }

        public List<AbsenceRequestVM> GetPendingAbsenceRequestVMs()
        {
            return context.Absence_Requests
                    .Where(n => n.Status.Equals("pending"))
                    .Select(absence_request => new AbsenceRequestVM() 
                    {
                        EmployeeName = absence_request.User.FirstName + " " + absence_request.User.LastName,
                        Email = absence_request.User.Email,
                        FromDate = absence_request.StartDate,
                        ToDate = absence_request.EndTime,
                        Reason = absence_request.Absence_Reason.ReasonName
                    }).ToList();
        }

        public List<AbsenceRequestVM> GetFinishedAbsenceRequestsVMs()
        {
            return context.Absence_Requests
                    .Where(n => n.Status.Equals("approved") || n.Status.Equals("rejected") || n.Status.Equals("cancelled"))
                    .Select(absence_request => new AbsenceRequestVM()
                    {
                        EmployeeName = absence_request.User.FirstName + " " + absence_request.User.LastName,
                        Email = absence_request.User.Email,
                        FromDate = absence_request.StartDate,
                        ToDate = absence_request.EndTime,
                        Reason = absence_request.Absence_Reason.ReasonName,
                        Status = absence_request.Status.Equals("approved") ? "одобрена" :
                                absence_request.Status.Equals("rejected") ? "отхвърлена" :
                                absence_request.Status.Equals("cancelled") ? "отказана" : ""
                    }).OrderByDescending(n => n.FromDate).ThenByDescending(n => n.ToDate).ToList();
        }

        public bool ApproveAbsenceRequest(AbsenceRequestVM absenceRequestVM)
        {
            var request = absenceRequestsRepository.GetAbsenceRequests().Where(n => n.User.Email.Equals(absenceRequestVM.Email) &&
                                                                n.StartDate.Equals(absenceRequestVM.FromDate) &&
                                                                n.EndTime.Equals(absenceRequestVM.ToDate) &&
                                                                n.Absence_Reason.ReasonName.Equals(absenceRequestVM.Reason)).SingleOrDefault();

            if(request != null)
            {
                request.Status = "approved";
                if(request.Absence_Reason.ReasonName.Equals("Платена отпуска"))
                {
                    var LeaveDaysRequested = calculateBusinessDaysBetweenDates(request.StartDate, request.EndTime);
                    request.User.Leave_Days.Find(n => n.Year.Equals(DateTime.UtcNow.Year)).AvailableLeaveDays -= LeaveDaysRequested;
                }
                try
                {
                    context.SaveChanges();
                }
                catch
                {
                    return false;
                }
                return true;
            }

            return false;
        }

        public bool RejectAbsenceRequest(AbsenceRequestVM absenceRequestVM)
        {
            var request = absenceRequestsRepository.GetAbsenceRequests().Where(n => n.User.Email.Equals(absenceRequestVM.Email) &&
                                                                n.StartDate.Equals(absenceRequestVM.FromDate) &&
                                                                n.EndTime.Equals(absenceRequestVM.ToDate) &&
                                                                n.Absence_Reason.ReasonName.Equals(absenceRequestVM.Reason)).SingleOrDefault();

            if (request != null)
            {
                request.Status = "rejected";
                try
                {
                    context.SaveChanges();
                }
                catch
                {
                    return false;
                }
                return true;
            }

            return false;
        }

        public bool CancelAbsenceRequest(AbsenceRequestVM absenceRequestVM)
        {
            var request = absenceRequestsRepository.GetAbsenceRequests().Where(n => n.User.Email.Equals(absenceRequestVM.Email) &&
                                                               n.StartDate.Equals(absenceRequestVM.FromDate) &&
                                                               n.EndTime.Equals(absenceRequestVM.ToDate) &&
                                                               n.Absence_Reason.ReasonName.Equals(absenceRequestVM.Reason)).SingleOrDefault();

            if (request != null)
            {
                request.Status = "cancelled";
                if (request.Absence_Reason.ReasonName.Equals("Платена отпуска"))
                {
                    var LeaveDaysRequested = calculateBusinessDaysBetweenDates(request.StartDate, request.EndTime);
                    request.User.Leave_Days.Find(n => n.Year.Equals(DateTime.UtcNow.Year)).AvailableLeaveDays += LeaveDaysRequested;
                }

                try
                {
                    context.SaveChanges();
                }
                catch
                {
                    return false;
                }

                return true;
            }

            return false;
        }

        public List<AbsenceRequestVM> GetAbsenceRequestVMsForEmployee(string username) =>
            context.Absence_Requests.Where(n => n.User.Username.Equals(username)).Select(absence_request => new AbsenceRequestVM()
            {
                Email = absence_request.User.Email,
                EmployeeName = absence_request.User.FirstName + " " + absence_request.User.LastName,
                FromDate = absence_request.StartDate,
                ToDate = absence_request.EndTime,
                Reason = absence_request.Absence_Reason.ReasonName,
                Status = absence_request.Status.Equals("approved") ? "одобрена" :
                            absence_request.Status.Equals("rejected") ? "отхвърлена" :
                            absence_request.Status.Equals("pending") ? "в процес на обработка" :
                            absence_request.Status.Equals("cancelled") ? "отказана" : ""
            }).OrderByDescending(n => n.FromDate).ThenByDescending(n => n.ToDate).ToList();
        

        public List<CalendarEventVM> GetCalendarPaidAbsenceVMsForCurrentYear()
        {
            int currentYear = DateTime.UtcNow.Year;
            return context.Absence_Requests
                    .Where(n => (n.StartDate.Year.Equals(currentYear) ||
                                n.EndTime.Year.Equals(currentYear)) &&
                                n.Status.Equals("approved") &&
                                n.Absence_Reason.ReasonName.ToLower().Equals("Платена отпуска".ToLower()))
                    .Select(absence_request => new CalendarEventVM()
                    {
                        title = absence_request.User.FirstName + " " + absence_request.User.LastName,
                        start = absence_request.StartDate.ToString("yyyy-MM-dd"),
                        end = absence_request.EndTime.AddDays(1).ToString("yyyy-MM-dd"),
                        description = absence_request.Absence_Reason.ReasonName,
                        period = absence_request.StartDate.ToShortDateString() + " - " + absence_request.EndTime.ToShortDateString()
                    }).ToList();
        }

        public List<CalendarEventVM> GetCalendarUnpaidAbsenceVMsForCurrentYear()
        {
            int currentYear = DateTime.UtcNow.Year;
            return context.Absence_Requests
                    .Where(n => (n.StartDate.Year.Equals(currentYear) ||
                                n.EndTime.Year.Equals(currentYear)) &&
                                n.Status.Equals("approved") &&
                                n.Absence_Reason.ReasonName.ToLower().Equals("Неплатена отпуска".ToLower()))
                    .Select(absence_request => new CalendarEventVM()
                    {
                        title = absence_request.User.FirstName + " " + absence_request.User.LastName,
                        start = absence_request.StartDate.ToString("yyyy-MM-dd"),
                        end = absence_request.EndTime.AddDays(1).ToString("yyyy-MM-dd"),
                        description = absence_request.Absence_Reason.ReasonName,
                        period = absence_request.StartDate.ToShortDateString() + " - " + absence_request.EndTime.ToShortDateString()
                    }).ToList();
        }

        public List<CalendarEventVM> GetCalendarSickAbsenceVMsForCurrentYear()
        {
            int currentYear = DateTime.UtcNow.Year;
            return context.Absence_Requests
                    .Where(n => (n.StartDate.Year.Equals(currentYear) ||
                                n.EndTime.Year.Equals(currentYear)) &&
                                n.Status.Equals("approved") &&
                                n.Absence_Reason.ReasonName.ToLower().Equals("Болничен".ToLower()))
                    .Select(absence_request => new CalendarEventVM()
                    {
                        title = absence_request.User.FirstName + " " + absence_request.User.LastName,
                        start = absence_request.StartDate.ToString("yyyy-MM-dd"),
                        end = absence_request.EndTime.AddDays(1).ToString("yyyy-MM-dd"),
                        description = absence_request.Absence_Reason.ReasonName,
                        period = absence_request.StartDate.ToShortDateString() + " - " + absence_request.EndTime.ToShortDateString()
                    }).ToList();
        }

        public List<CalendarEventVM> GetCalendarOtherAbsenceVMsForCurrentYear()
        {
            int currentYear = DateTime.UtcNow.Year;
            return context.Absence_Requests
                    .Where(n => (n.StartDate.Year.Equals(currentYear) ||
                                n.EndTime.Year.Equals(currentYear)) &&
                                n.Status.Equals("approved") &&
                                (n.Absence_Reason.ReasonName.ToLower().Equals("Брак".ToLower()) ||
                                n.Absence_Reason.ReasonName.ToLower().Equals("Кръводаряване".ToLower()) ||
                                 n.Absence_Reason.ReasonName.ToLower().Equals("Погребение".ToLower())))
                    .Select(absence_request => new CalendarEventVM()
                    {
                        title = absence_request.User.FirstName + " " + absence_request.User.LastName,
                        start = absence_request.StartDate.ToString("yyyy-MM-dd"),
                        end = absence_request.EndTime.AddDays(1).ToString("yyyy-MM-dd"),
                        description = absence_request.Absence_Reason.ReasonName,
                        period = absence_request.StartDate.ToShortDateString() + " - " + absence_request.EndTime.ToShortDateString()
                    }).ToList();
        }

        public int calculateBusinessDaysBetweenDates(DateTime start, DateTime end)
        {
            DateTime startDate = start;
            DateTime endDate = end;

            if(start == end)
            {
                return 0;
            }

            int LeaveDaysRequested = 0;
            var i = startDate;
            do
            {
                if (!(i.DayOfWeek.Equals(DayOfWeek.Saturday)) && !(i.DayOfWeek.Equals(DayOfWeek.Sunday)))
                {
                    LeaveDaysRequested++;
                }
                i = i.AddDays(1);
            } while (i <= endDate);

            return LeaveDaysRequested;
        }

        public int GetEmployeePaidLeaveForMonth(string email, int month)
        {
            var paidLeaveDays = 0;
            
            List<Absence_Request> paidLeaves = absenceRequestsRepository.GetAbsenceRequestsForUser(email).Where(n => n.Absence_Reason.ReasonName.ToLower().Equals("Платена отпуска".ToLower())
                                                                                                                && n.StartDate.Month <= month &&
                                                                                                                    n.EndTime.Month >= month &&
                                                                                                                    n.Status.ToLower().Equals("approved")).ToList();
            var holidays = holidaysService.GetHolidayDatesForTheYear(DateTime.UtcNow.Year);
            foreach (var paidLeave in paidLeaves)
            {
                var i = paidLeave.StartDate;
                do
                {
                    if (i.Month == month)
                    {
                        if (!(i.DayOfWeek.Equals(DayOfWeek.Saturday)) && !(i.DayOfWeek.Equals(DayOfWeek.Sunday)))
                        {
                            if (!holidays.Contains(i))
                            {
                                paidLeaveDays++;
                            }
                        }
                    }

                    i = i.AddDays(1);
                } while (i <= paidLeave.EndTime);
            }
            var test = paidLeaveDays;
            return paidLeaveDays;
        }

        public int GetEmployeeUnpaidLeaveForMonth(string email, int month)
        {
            var unpaidLeaveDays = 0;

            List<Absence_Request> unpaidLeaves = absenceRequestsRepository.GetAbsenceRequestsForUser(email).Where(n => n.Absence_Reason.ReasonName.ToLower().Equals("Неплатена отпуска".ToLower())
                                                                                                                && n.StartDate.Month <= month &&
                                                                                                                    n.EndTime.Month >= month &&
                                                                                                                    n.Status.ToLower().Equals("approved")).ToList();
            var holidays = holidaysService.GetHolidayDatesForTheYear(DateTime.UtcNow.Year);
            foreach (var unpaidLeave in unpaidLeaves)
            {
                var i = unpaidLeave.StartDate;
                do
                {
                    if (i.Month == month)
                    {
                        if (!(i.DayOfWeek.Equals(DayOfWeek.Saturday)) && !(i.DayOfWeek.Equals(DayOfWeek.Sunday)))
                        {
                            if (!holidays.Contains(i))
                            {
                                unpaidLeaveDays++;
                            }
                        }
                    }

                    i = i.AddDays(1);
                } while (i <= unpaidLeave.EndTime);
            }
            var test = unpaidLeaveDays;
            return unpaidLeaveDays;
        }

        public int GetEmployeeSickLeaveForMonth(string email, int month)
        {
            var sickLeaveDays = 0;

            List<Absence_Request> sickLeaves = absenceRequestsRepository.GetAbsenceRequestsForUser(email).Where(n => n.Absence_Reason.ReasonName.ToLower().Equals("Болничен".ToLower())
                                                                                                                && n.StartDate.Month <= month &&
                                                                                                                    n.EndTime.Month >= month &&
                                                                                                                    n.Status.ToLower().Equals("approved")).ToList();
            var holidays = holidaysService.GetHolidayDatesForTheYear(DateTime.UtcNow.Year);


            foreach (var sickLeave in sickLeaves)
            {
                var i = sickLeave.StartDate;
                do
                {
                    if (i.Month == month)
                    {
                        if (!(i.DayOfWeek.Equals(DayOfWeek.Saturday)) && !(i.DayOfWeek.Equals(DayOfWeek.Sunday)))
                        {
                            if (!holidays.Contains(i))
                            {
                                sickLeaveDays++;
                            }
                        }
                    }

                    i = i.AddDays(1);
                } while (i <= sickLeave.EndTime);
            }
            var test = sickLeaveDays;
            return sickLeaveDays;
        }

        public int GetEmployeeOtherLeaveForMonth(string email, int month)
        {
            var otherLeaveDays = 0;

            List<Absence_Request> otherLeaves = absenceRequestsRepository.GetAbsenceRequestsForUser(email).Where(n => (n.Absence_Reason.ReasonName.ToLower().Equals("Брак".ToLower()) ||
                                                                                                                   n.Absence_Reason.ReasonName.ToLower().Equals("Кръводаряване".ToLower()) ||
                                                                                                                   n.Absence_Reason.ReasonName.ToLower().Equals("Погребение".ToLower()))
                                                                                                                && n.StartDate.Month <= month &&
                                                                                                                    n.EndTime.Month >= month &&
                                                                                                                    n.Status.ToLower().Equals("approved")).ToList();
            var holidays = holidaysService.GetHolidayDatesForTheYear(DateTime.UtcNow.Year);
            foreach (var otherLeave in otherLeaves)
            {
                var i = otherLeave.StartDate;
                do
                {
                    if (i.Month == month)
                    {
                        if (!(i.DayOfWeek.Equals(DayOfWeek.Saturday)) && !(i.DayOfWeek.Equals(DayOfWeek.Sunday)))
                        {
                            if (!holidays.Contains(i))
                            {
                                otherLeaveDays++;
                            }
                        }
                    }

                    i = i.AddDays(1);
                } while (i <= otherLeave.EndTime);
            }
            var test = otherLeaveDays;
            return otherLeaveDays;
        }
    }
}
