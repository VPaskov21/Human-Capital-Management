using HCMApp.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HCMApp.Data.Services
{
    public class ValidationService
    {

        public bool ValidateEmployeeData(AddEmployeeVM employeeVM) =>   ValidateName(employeeVM.FirstName) &&
                                                                        ValidateName(employeeVM.MiddleName) &&
                                                                        ValidateName(employeeVM.LastName) &&
                                                                        ValidateEmail(employeeVM.Email) &&
                                                                        ValidatePhoneNumber(employeeVM.PhoneNumber) &&
                                                                        ValidateCountryOrRegionOrCity(employeeVM.Country) &&
                                                                        ValidateCountryOrRegionOrCity(employeeVM.Region) &&
                                                                        ValidateCountryOrRegionOrCity(employeeVM.City) &&
                                                                        ValidateAddress(employeeVM.Address) &&
                                                                        ValidateAdditionalAddress(employeeVM.Additional_Address) &&
                                                                        ValidatePostalCode(employeeVM.Postal_Code) &&
                                                                        ValidateSalary(employeeVM.Salary.ToString());

        public bool ValidateEmployeeData(EditEmployeeVM employeeVM) => ValidateName(employeeVM.FirstName) &&
                                                                        ValidateName(employeeVM.MiddleName) &&
                                                                        ValidateName(employeeVM.LastName) &&
                                                                        ValidateEmail(employeeVM.Email) &&
                                                                        ValidatePhoneNumber(employeeVM.PhoneNumber) &&
                                                                        ValidateCountryOrRegionOrCity(employeeVM.Country) &&
                                                                        ValidateCountryOrRegionOrCity(employeeVM.Region) &&
                                                                        ValidateCountryOrRegionOrCity(employeeVM.City) &&
                                                                        ValidateAddress(employeeVM.Address) &&
                                                                        ValidateAdditionalAddress(employeeVM.Address_Additional) &&
                                                                        ValidatePostalCode(employeeVM.Postal_Code) &&
                                                                        ValidateSalary(employeeVM.Salary.ToString());

        public bool ValidateEmployeePasswords(PasswordVM passwordVM) => ValidatePassword(passwordVM.NewPassword) &&
                                                                        ValidatePassword(passwordVM.ConfirmPassword);

        public bool ValidateEmployeeAddress(AddressVM addressVM) => ValidateCountryOrRegionOrCity(addressVM.Country) &&
                                                                    ValidateCountryOrRegionOrCity(addressVM.Region) &&
                                                                    ValidateCountryOrRegionOrCity(addressVM.City) &&
                                                                    ValidateAddress(addressVM.Address) &&
                                                                    ValidateAdditionalAddress(addressVM.Additional_Address) &&
                                                                    ValidatePostalCode(addressVM.Postal_Code);

        public bool ValidateRole(string name, string minSalary, string maxSalary) => ValidateRoleName(name) &&
                                                                                        ValidateSalary(minSalary) &&
                                                                                        ValidateSalary(maxSalary);

        private bool ValidateName(string name) => Regex.IsMatch(name, "^([a-zA-Zа-яА-Я])+$");

        private bool ValidateEmail(string email) => Regex.IsMatch(email, @"^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$");

        private bool ValidatePhoneNumber(string phone) => Regex.IsMatch(phone, "^(0)(87|88|89|98)[0-9]{7}$");

        private bool ValidateCountryOrRegionOrCity(string input) => Regex.IsMatch(input, "^[a-zA-Zа-яА-Я]*$");

        private bool ValidatePostalCode(string postalCode) => Regex.IsMatch(postalCode, "^[1-9]{1}[0-9]{3}$");

        private bool ValidateSalary(string salary) => Regex.IsMatch(salary, "^[0-9]+((.){1}[0-9]{2})?$");

        private bool ValidatePassword(string password) => password.Length > 8;

        private bool ValidateAddress(string address) => Regex.IsMatch(address, "^[a-zA-Z0-9а-яА-Я- ,.]*$");

        private bool ValidateAdditionalAddress(string address)
        {
            if(address != null)
            {
                return ValidateAddress(address);
            }

            return true;
        }

        public bool ValidateDepartmentName(string name) => Regex.IsMatch(name, "^([a-zA-Zа-яА-Я ])+$");

        private bool ValidateRoleName(string name) => Regex.IsMatch(name, "^[a-zA-Z0а-яА-Я ,]*$");
    }
}
