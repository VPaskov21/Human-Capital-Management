using HCMApp.Data.Models;
using HCMApp.Data.Services;
using HCMApp.Data.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HCMApp.Controllers
{
    [Route("api/")]
    [ApiController]
    public class APIController : ControllerBase
    {
        private readonly RoleService roleService;
        private readonly CountryService countryService;
        private readonly RegionService regionService;
        private readonly CityService cityService;
        private readonly PostalCodeService postalCodeService;
        private readonly AbsenceService absenceService;
        private readonly SalaryService salaryService;
        private readonly HolidaysService holidaysService;

        public APIController(RoleService roleService,
            CountryService countryService,
            RegionService regionService,
            CityService cityService,
            PostalCodeService postalCodeService,
            AbsenceService absenceService,
            SalaryService salaryService,
            HolidaysService holidaysService)
        {
            this.roleService = roleService;
            this.countryService = countryService;
            this.regionService = regionService;
            this.cityService = cityService;
            this.postalCodeService = postalCodeService;
            this.absenceService = absenceService;
            this.salaryService = salaryService;
            this.holidaysService = holidaysService;
        }

        [HttpGet("department/get-roles-for-department/{departmentId}")]
        public IActionResult GetRolesForDepartment(int departmentId)
        {
            List<RoleVM> rolesInDepartment = roleService.GetRolesForDepartment(departmentId).OrderBy(n => n.RoleName).ToList();
            return Ok(rolesInDepartment);
        }

        [HttpGet("countries/get-countries")]
        public IActionResult GetCountries()
        {
            List<CountryVM> countries = countryService.GetCountriesVM();
            return Ok(countries);
        }

        [HttpGet("regions/get-regions-for-country/{countryName}")]
        public IActionResult GetRegionsForCountry(string countryName)
        {
            List<RegionVM> regions = regionService.GetRegionVMsForCountry(countryName);
            return Ok(regions);
        }

        [HttpGet("cities/get-cities-for-region/{regionName}")]
        public IActionResult GetCitiesForRegion(string regionName)
        {
            List<CityVM> cities = cityService.GetCityVMsForRegion(regionName);

            return Ok(cities);
        }

        [HttpGet("postal-codes/get-postal-code-for-city/{cityName}")]
        public IActionResult GetPostalCodeForCity(string cityName)
        {
            PostalCodeVM postalCode = postalCodeService.GetPostalCodeVMForCity(cityName);
            if(postalCode != null)
            {
                return Ok(postalCode);
            } else
            {
                return Ok(null);
            }
            
        }

        [HttpGet("role/get-current-employee-role/{emailAddress}")]
        public IActionResult GetCurrentEmployeeRole(string emailAddress)
        {
            RoleVM currentEmployeeRole = roleService.GetRoleForEmployee(emailAddress);
            return Ok(currentEmployeeRole);
        }

        [HttpGet("absence/get-absence-reasons/")]
        public IActionResult GetAbsenceReasons()
        {
            List<AbsenceReasonVM> absenceReasonsVMList = absenceService.GetAbsenceReasonVMs();
            return Ok(absenceReasonsVMList);
        }

        [HttpGet("absence/get-approved-paid-absences-for-current-year/")]
        public IActionResult GetApprovedPaidAbsenceRequestsForCurrentYear()
        {
            List<CalendarEventVM> calendarPaidAbsences = absenceService.GetCalendarPaidAbsenceVMsForCurrentYear();
            return Ok(calendarPaidAbsences);
        }

        [HttpGet("absence/get-approved-unpaid-absences-for-current-year/")]
        public IActionResult GetApprovedUnpaidAbsenceRequestsForCurrentYear()
        {
            List<CalendarEventVM> calendarUnpaidAbsences = absenceService.GetCalendarUnpaidAbsenceVMsForCurrentYear();
            return Ok(calendarUnpaidAbsences);
        }

        [HttpGet("absence/get-approved-sick-absences-for-current-year/")]
        public IActionResult GetApprovedSickAbsenceRequestsForCurrentYear()
        {
            List<CalendarEventVM> calendarSickAbsences = absenceService.GetCalendarSickAbsenceVMsForCurrentYear();
            return Ok(calendarSickAbsences);
        }

        [HttpGet("absence/get-approved-other-absences-for-current-year/")]
        public IActionResult GetApprovedOtherAbsenceRequestsForCurrentYear()
        {
            List<CalendarEventVM> calendarOtherAbsences = absenceService.GetCalendarOtherAbsenceVMsForCurrentYear();
            return Ok(calendarOtherAbsences);
        }

        [HttpGet("holiday/get-holidays-for-current-year/")]
        public IActionResult GetHolidaysForCurrentYear()
        {
            List<CalendarEventVM> calendarHolidays = holidaysService.GetCalendarEventVMsForCurrentYear();
            return Ok(calendarHolidays);
        }

        [HttpGet("role/get-salary-range-for-role/{roleId}")]
        public IActionResult GetSalaryRangeForRole(int roleId)
        {
            SalaryRangeVM salaryRange = salaryService.GetSalaryRangeVMforRole(roleId);

            return Ok(salaryRange);
        }
    }
}
