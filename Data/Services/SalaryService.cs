using HCMApp.Data.Enums;
using HCMApp.Data.Models;
using HCMApp.Data.Repositories;
using HCMApp.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace HCMApp.Data.Services
{
    public class SalaryService
    {
        private readonly AppDbContext context;
        private readonly IRoleRepository roleRepository;
        private readonly IUserRepository userRepository;
        private readonly AbsenceService absenceService;
        private readonly ISalaryHistoriesRepository salaryHistoriesRepository;
        private readonly WorkMonthService workMonthService;

        public SalaryService(AppDbContext context,
            IRoleRepository roleRepository,
            IUserRepository userRepository,
            AbsenceService absenceService,
            ISalaryHistoriesRepository salaryHistoriesRepository,
            WorkMonthService workMonthService)
        {
            this.context = context;
            this.roleRepository = roleRepository;
            this.userRepository = userRepository;
            this.absenceService = absenceService;
            this.salaryHistoriesRepository = salaryHistoriesRepository;
            this.workMonthService = workMonthService;
        }

        public Salary GetSalaryBySalaryGrossAmount(double grossAmount)
        {
            var salary = context.Salaries.Where(n => n.grossSalary == grossAmount).SingleOrDefault();

            if(salary != null)
            {
                return salary;
            } 
            else
            {
                context.Salaries.Add(new Salary()
                {
                    grossSalary = grossAmount,
                    netSalary = (int)CalculateNetSalary(grossAmount)
                });
                context.SaveChanges();

                salary = context.Salaries.Where(n => n.grossSalary == grossAmount).SingleOrDefault();

                return salary;
            }
        }

        public double CalculateNetSalary(double grossAmount)
        {
            double DOO = 0;
            double DZPO = 0;
            double ZO = 0;
            if (grossAmount < 3000)
            {
                DOO = Math.Round((grossAmount * (8.38 / 100)), 2);
                DZPO = Math.Round((grossAmount * (2.20 / 100)), 2);
                ZO = Math.Round((grossAmount * (3.20 / 100)), 2);
            } else
            {
                DOO = Math.Round((3000 * (8.38 / 100)), 2);
                DZPO = Math.Round((3000 * (2.20 / 100)), 2);
                ZO = Math.Round((3000 * (3.20 / 100)), 2);
            }

                

            double socialSecurityContributionsByEmployee = Math.Round((DOO + DZPO + ZO),2);
            double taxBase = Math.Round((grossAmount - socialSecurityContributionsByEmployee),2);
            double incomeTax = taxBase * 0.1;

            double netSalary = grossAmount - (socialSecurityContributionsByEmployee + incomeTax);

            return  Math.Round(netSalary, 2);
        }

        public double CalculateNetSalaryWhenSickLeaveUsed(double grossAmount, double sickLeaveCost)
        {
            double DOO = 0;
            double DZPO = 0;
            double ZO = 0;
            if (grossAmount < 3000)
            {
                DOO = Math.Round((grossAmount * (8.38 / 100)), 2);
                DZPO = Math.Round((grossAmount * (2.20 / 100)), 2);
                ZO = Math.Round((grossAmount * (3.20 / 100)), 2);
            }
            else
            {
                DOO = Math.Round((3000 * (8.38 / 100)), 2);
                DZPO = Math.Round((3000 * (2.20 / 100)), 2);
                ZO = Math.Round((3000 * (3.20 / 100)), 2);
            }

            double socialSecurityContributionsByEmployee = Math.Round((DOO + DZPO + ZO), 2);
            double taxBase = (grossAmount - sickLeaveCost) - socialSecurityContributionsByEmployee;
            double incomeTax = taxBase * 0.1;

            double netSalary = grossAmount - (socialSecurityContributionsByEmployee + incomeTax);

            return Math.Round(netSalary, 2);
        }

        public int CalculateGrossSalary(int netAmount)
        {
            double grossSalary = netAmount * 1.2887;

            return (int) grossSalary;
        }

        public SalaryRangeVM GetSalaryRangeVMforRole(int roleId)
        {
            var role = roleRepository.GetRoleById(roleId);

            return new SalaryRangeVM()
            {
                minimumSalary = role.MinimalSalary.ToString(),
                maximumSalary = role.MaximumSalary.ToString()
            };
        }

        public bool IsSalaryApprovalActive()
        {
            var salaryHistories = context.SalaryHistories.OrderByDescending(n => n.WorkMonthId).ToList();
            if(salaryHistories.Count == 0)
            {
                return true;
            } else
            {
                var lastSalaryHistoriesMonthId = context.SalaryHistories.OrderByDescending(n => n.WorkMonth.MonthNumber).ToList().FirstOrDefault().WorkMonthId;
                var month = context.WorkMonths.Where(n => n.Id == lastSalaryHistoriesMonthId).SingleOrDefault();
                var monthNum = context.WorkMonths.Where(n => n.Id == lastSalaryHistoriesMonthId).Single().MonthNumber;
                if ((monthNum == (DateTime.UtcNow.Month - 1)) || (month == null))
                {
                    return false;
                } else
                {
                    return true;
                }
            }
        }

        public List<EmployeeSalaryHistoryVM> GetEmployeeSalaryHistoryVMs()
        {
            List<EmployeeSalaryHistoryVM> employeeSalaryHistoryVMs = new List<EmployeeSalaryHistoryVM>();
            List<User> users = userRepository.GetUsers();

            var requestedMonth = DateTime.UtcNow.Month - 1;

            foreach(var user in users)
            {
                var workDaysForMonth = workMonthService.GetWorkMonth(DateTime.UtcNow.Month - 1, DateTime.UtcNow.Year).WorkDays;
                var paidLeaveDays = absenceService.GetEmployeePaidLeaveForMonth(user.Email, DateTime.UtcNow.Month - 1);

                int days = 0;
                if (user.Start_Date.Month == requestedMonth && user.Start_Date.Year == DateTime.UtcNow.Year)
                {
                    if (requestedMonth < 10)
                    {
                        days = absenceService.calculateBusinessDaysBetweenDates(DateTime.ParseExact("01-0" + requestedMonth + "-" + DateTime.UtcNow.Year, "dd-MM-yyyy", CultureInfo.InvariantCulture),
                                                                                user.Start_Date);
                    }
                    else
                    {
                        days = absenceService.calculateBusinessDaysBetweenDates(DateTime.ParseExact("01-" + requestedMonth + "-" + DateTime.UtcNow.Year, "dd-MM-yyyy", CultureInfo.InvariantCulture),
                                                                                user.Start_Date);
                    }

                }

                if(user.Start_Date.Month > requestedMonth && user.Start_Date.Year == DateTime.UtcNow.Year)
                {
                    continue;
                }

                

                double paidLeaveCost = 0;
                if(paidLeaveDays > 0)
                {
                    paidLeaveCost = CalculatePaidLeaveCost(workDaysForMonth, paidLeaveDays, user.Salary.grossSalary);
                }
                
                var unpaidLeaveDays = absenceService.GetEmployeeUnpaidLeaveForMonth(user.Email, DateTime.UtcNow.Month - 1);

                double unpaidLeaveCost = 0;
                if(unpaidLeaveDays > 0)
                {
                    unpaidLeaveCost = CalculateUnpaidLeaveCost(workDaysForMonth, unpaidLeaveDays, user.Salary.grossSalary);
                }

                var sickLeaveDays = absenceService.GetEmployeeSickLeaveForMonth(user.Email, DateTime.UtcNow.Month - 1);

                double sickLeaveCost = 0;
                if(sickLeaveDays > 0)
                {
                    sickLeaveCost = CalculateSickLeaveCost(workDaysForMonth, sickLeaveDays, user.Salary.grossSalary);
                }


                var otherLeaveDays = absenceService.GetEmployeeOtherLeaveForMonth(user.Email, DateTime.UtcNow.Month - 1);

                double otherLeaveCost = 0;
                if(otherLeaveDays > 0)
                {
                    otherLeaveCost = CalculateOtherLeaveCost(workDaysForMonth, otherLeaveDays, user.Salary.grossSalary);
                }

                double oneBusinessDayCost = user.Salary.grossSalary / workDaysForMonth;
                int workedOutDaysInMonth = 0;
                double workedOutSalary = 0;
                double netSumToReceive = 0;
                if(sickLeaveDays == 0)
                {
                    workedOutDaysInMonth = workDaysForMonth - (paidLeaveDays + unpaidLeaveDays + otherLeaveDays) - days;
                    workedOutSalary = Math.Round(((workedOutDaysInMonth * oneBusinessDayCost) + paidLeaveCost + otherLeaveCost), 2);

                    netSumToReceive = CalculateNetSalary(workedOutSalary);
                } else
                {
                    workedOutDaysInMonth = workDaysForMonth - (paidLeaveDays + unpaidLeaveDays + sickLeaveDays + otherLeaveDays) - days;
                    workedOutSalary = Math.Round((workedOutDaysInMonth * oneBusinessDayCost),2);


                    double grossAmount = workedOutSalary + paidLeaveCost + sickLeaveCost + otherLeaveCost;
                    netSumToReceive = CalculateNetSalaryWhenSickLeaveUsed(grossAmount, sickLeaveCost);
                }

                
                employeeSalaryHistoryVMs.Add(new EmployeeSalaryHistoryVM()
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Role = user.Role.RoleName,
                    Department = user.Department.DepartmentName,
                    GrossSalary = user.Salary.grossSalary.ToString(),
                    PaidLeaveUsed = paidLeaveDays,
                    PaidLeaveCost = paidLeaveCost,
                    UnpaidLeaveUsed = unpaidLeaveDays,
                    SickLeaveUsed = sickLeaveDays,
                    SickLeaveCost = sickLeaveCost,
                    OtherLeaveUsed = otherLeaveDays,
                    OtherLeaveCost = otherLeaveCost,
                    NetSumToReceive = netSumToReceive
                });
            }

            return employeeSalaryHistoryVMs;
        }

        public bool SubmitSalaryHistoryForMonth(List<EmployeeSalaryHistoryVM> employeeSalaryHistoryRecords,int month)
        {
            try
            {
                var workDaysInMonth = context.WorkMonths.Where(n => n.MonthNumber == month && n.Year == DateTime.UtcNow.Year).Single().WorkDays;
                var workMonthId = context.WorkMonths.Where(n => n.MonthNumber == month && n.Year == DateTime.UtcNow.Year).Single().Id;
                foreach (var record in employeeSalaryHistoryRecords)
                {
                    var user = userRepository.GetUserByEmail(record.Email);
                    int days = 0;
                    if (user.Start_Date.Month == month && user.Start_Date.Year == DateTime.UtcNow.Year)
                    {
                        if(month < 10)
                        {
                            days = absenceService.calculateBusinessDaysBetweenDates(DateTime.ParseExact("01-0" + month + "-" + DateTime.UtcNow.Year, "dd-MM-yyyy", CultureInfo.InvariantCulture), 
                                                                                    user.Start_Date);
                        }
                        else
                        {
                            days = absenceService.calculateBusinessDaysBetweenDates(DateTime.ParseExact("01-" + month + "-" + DateTime.UtcNow.Year, "dd-MM-yyyy", CultureInfo.InvariantCulture),
                                                                                    user.Start_Date);
                        }
                        
                    }

                    var employeeWorkedOutDays = (workDaysInMonth - days) - (record.PaidLeaveUsed + record.UnpaidLeaveUsed + record.SickLeaveUsed + record.OtherLeaveUsed);

                    
                    context.SalaryHistories.Add(new SalaryHistory()
                    {
                        WorkedOutDays = employeeWorkedOutDays,
                        PaidLeaveUsed = record.PaidLeaveUsed,
                        UnpaidLeaveUsed = record.UnpaidLeaveUsed,
                        SickLeaveUsed = record.SickLeaveUsed,
                        OtherLeaveUsed = record.OtherLeaveUsed,
                        Bonus = record.MonthBonus > 0 ? record.MonthBonus : 0,
                        TotalSalary = record.NetSumToReceive + (record.MonthBonus > 0 ? record.MonthBonus : 0),
                        UserId = userRepository.GetUserByEmail(record.Email).UserId,
                        WorkMonthId = workMonthId
                    });

                    context.SaveChanges();
                }
                return true;
            }catch
            {
                return false;
            }
        }

        private double CalculateWorkedOutSalary(int workDaysInMonth,
            int paidLeaveUsed, 
            int unpaidLeaveUsed,
            int sickLeaveUsed,
            int otherLeaveUsed,
            double grossSalary)
        {
            var businessDays = workDaysInMonth;

            var workedOutBusinessDays = businessDays - (paidLeaveUsed + unpaidLeaveUsed + sickLeaveUsed + otherLeaveUsed);
            var oneBusinessDayCost = grossSalary / businessDays;

            var workedOutSalary = oneBusinessDayCost * workedOutBusinessDays;

            return Math.Round(workedOutSalary, 2);
        }

        private double CalculatePaidLeaveCost(int workDaysInMonth,
            int paidLeaveUsed,
            double grossSalary)
        {
            double paidLeaveCost = 0;
            var oneBusinessDayCost = grossSalary / workDaysInMonth;

            if (paidLeaveUsed > 0)
            {
                paidLeaveCost = Convert.ToDouble(paidLeaveUsed) * oneBusinessDayCost;
            }
            
            return Math.Round(paidLeaveCost,2);
        }

        public double CalculateUnpaidLeaveCost(int workDaysInMonth,
            int unpaidLeaveUsed,
            double grossSalary)
        {
            double unpaidLeaveCost = 0;
            var oneBusinessDayCost = grossSalary / workDaysInMonth;

            if (unpaidLeaveUsed > 0)
            {
                unpaidLeaveCost = Convert.ToDouble(unpaidLeaveUsed) * oneBusinessDayCost;
            }

            return Math.Round(unpaidLeaveCost, 2);
        }

        private double CalculateSickLeaveCost(int workDaysInMonth,
            int sickLeaveUsed,
            double grossSalary)
        {
            double sickLeaveCostEmployer = 0;
            var oneBusinessDayCost = grossSalary / workDaysInMonth;

            if (sickLeaveUsed > 0)
            {
                sickLeaveCostEmployer = Convert.ToDouble(3) * (oneBusinessDayCost * 0.7);
            }
            return Math.Round(sickLeaveCostEmployer,2);
        }
        
        private double CalculateOtherLeaveCost(int workDaysInMonth,
            int otherLeaveUsed,
            double grossSalary)
        {
            double otherLeaveCost = 0;
            var oneBusinessDayCost = grossSalary / workDaysInMonth;

            if (otherLeaveUsed > 0)
            {
                otherLeaveCost = Convert.ToDouble(otherLeaveUsed) * oneBusinessDayCost;
            }

            return Math.Round(otherLeaveCost,2);
        } 

        public List<SalaryHistoryRecordVM> GetEmployeeSalaryHistoriesVM(string username)
        {
            return salaryHistoriesRepository.GetSalaryHistories()
                .Where(n => n.User.Username.ToLower().Equals(username.ToLower()))
                .Select(salary_history => new SalaryHistoryRecordVM()
                {
                    GrossSalary = salary_history.User.Salary.grossSalary.ToString().Replace(",", "."),
                    PaidLeaveUsed = salary_history.PaidLeaveUsed.Value,
                    PaidLeaveCost = CalculatePaidLeaveCost(salary_history.WorkMonth.WorkDays, salary_history.PaidLeaveUsed.Value, salary_history.User.Salary.grossSalary),
                    UnpaidLeaveUsed = salary_history.UnpaidLeaveUsed.Value,
                    SickLeaveUsed = salary_history.SickLeaveUsed.Value,
                    SickLeaveCost = CalculateSickLeaveCost(salary_history.WorkMonth.WorkDays, salary_history.SickLeaveUsed.Value, salary_history.User.Salary.grossSalary),
                    OtherLeaveUsed = salary_history.OtherLeaveUsed.Value,
                    OtherLeaveCost = CalculateOtherLeaveCost(salary_history.WorkMonth.WorkDays, salary_history.OtherLeaveUsed.Value, salary_history.User.Salary.grossSalary),
                    MonthBonus = salary_history.Bonus.HasValue ? salary_history.Bonus.Value : 0,
                    WorkedOutSalary = CalculateWorkedOutSalary(salary_history.WorkMonth.WorkDays,
                                                                salary_history.PaidLeaveUsed.Value,
                                                                salary_history.UnpaidLeaveUsed.Value,
                                                                salary_history.SickLeaveUsed.Value,
                                                                salary_history.OtherLeaveUsed.Value,
                                                                salary_history.User.Salary.grossSalary),
                    WorkedOutDays = salary_history.WorkedOutDays,
                    MonthNum = salary_history.WorkMonth.MonthNumber,
                    Month = ((Month) salary_history.WorkMonth.MonthNumber).ToString(),
                    Year = salary_history.WorkMonth.Year,
                    WorkDaysInMonth = salary_history.WorkMonth.WorkDays,
                    TotalSalaryReceived = salary_history.TotalSalary
                }).OrderByDescending(n => n.Year).ThenByDescending(n => n.MonthNum).ToList();
        }

        //Used in the initializer to calculate the total salary
        public double CalculateNetSumToReceive(int monthNum,
            int paidLeaveUsed,
            int unpaidLeaveUsed,
            int sickLeaveUsed,
            int otherLeaveUsed,
            double grossSalary,
            int bonus)
        {
            var workDaysForMonth = workMonthService.GetWorkMonth(monthNum, DateTime.UtcNow.Year).WorkDays;

            double paidLeaveCost = 0;
            if (paidLeaveUsed > 0)
            {
                paidLeaveCost = CalculatePaidLeaveCost(workDaysForMonth, paidLeaveUsed, grossSalary);
            }

            double unpaidLeaveCost = 0;
            if (unpaidLeaveUsed > 0)
            {
                unpaidLeaveCost = CalculateUnpaidLeaveCost(workDaysForMonth, unpaidLeaveUsed, grossSalary);
            }

            double sickLeaveCost = 0;
            if (sickLeaveUsed > 0)
            {
                sickLeaveCost = CalculateSickLeaveCost(workDaysForMonth, sickLeaveUsed, grossSalary);
            }

            double otherLeaveCost = 0;
            if (otherLeaveUsed > 0)
            {
                otherLeaveCost = CalculateOtherLeaveCost(workDaysForMonth, otherLeaveUsed, grossSalary);
            }

            double oneBusinessDayCost = grossSalary / workDaysForMonth;
            int workedOutDaysInMonth = 0;
            double workedOutSalary = 0;
            double netSumToReceive = 0;
            if (sickLeaveUsed == 0)
            {
                workedOutDaysInMonth = workDaysForMonth - (paidLeaveUsed + unpaidLeaveUsed + sickLeaveUsed + otherLeaveUsed);
                workedOutSalary = Math.Round(((workedOutDaysInMonth * oneBusinessDayCost) + paidLeaveCost + otherLeaveCost),2);

                netSumToReceive = CalculateNetSalary(workedOutSalary);
            }
            else
            {
                workedOutDaysInMonth = workDaysForMonth - (paidLeaveUsed + unpaidLeaveUsed + sickLeaveUsed + otherLeaveUsed);
                workedOutSalary = Math.Round((workedOutDaysInMonth * oneBusinessDayCost), 2);


                double grossAmount = workedOutSalary + paidLeaveCost + sickLeaveCost + otherLeaveCost;

                netSumToReceive = CalculateNetSalaryWhenSickLeaveUsed(grossAmount, sickLeaveCost);
            }

            if(bonus > 0)
            {
                netSumToReceive += bonus;
            }

            return netSumToReceive;
        }
    }
}
