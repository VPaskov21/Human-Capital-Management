using HCMApp.Data.Repositories;
using HCMApp.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HCMApp.Data.Services
{
    public class CountryService
    {
        private readonly ICountryRepository countryRepository;

        public CountryService(ICountryRepository countryRepository)
        {
            this.countryRepository = countryRepository;
        }

        public List<CountryVM> GetCountriesVM() =>
            countryRepository.GetCountries().Select(country => new CountryVM()
            {
                Id = country.Id,
                CountryName = country.CountryName
            }).ToList();
    }
}
