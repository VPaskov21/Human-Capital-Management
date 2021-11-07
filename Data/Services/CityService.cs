using HCMApp.Data.Models;
using HCMApp.Data.Repositories;
using HCMApp.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HCMApp.Data.Services
{
    public class CityService
    {
        private readonly ICityRepository cityRepository;
        private readonly AppDbContext context;
        private readonly PostalCodeService postalCodeService;
        private readonly RegionService regionService;

        public CityService(ICityRepository cityRepository, 
            AppDbContext context,
            PostalCodeService postalCodeService,
            RegionService regionService)
        {
            this.cityRepository = cityRepository;
            this.context = context;
            this.postalCodeService = postalCodeService;
            this.regionService = regionService;
        }

        public List<CityVM> GetCityVMsForRegion(string regionName) =>
            cityRepository.GetCities().Where(n => n.Region.RegionName.ToLower().Equals(regionName.ToLower()))
            .Select(city => new CityVM()
            {
                Id = city.Id,
                CityName = city.CityName
            }).ToList();

        public City GetCityAndCreateIfNotExist(string cityName, string region, string PostalCode, string country)
        {
            City city = cityRepository.GetCityByName(cityName);

            if(city != null)
            {
                return city;
            } else
            {
                context.Cities.Add(new City()
                {
                    CityName = cityName,
                    PostalCodeId = postalCodeService.GetPostalCodeAndCreateIfNotExist(cityName, PostalCode).Id,
                    RegionId = regionService.GetRegionAndCreateIfNotExist(region,country).Id
                });

                context.SaveChanges();

                city = cityRepository.GetCities().Where(n => n.CityName.ToLower().Equals(cityName.ToLower()) &&
                                                             n.Region.RegionName.ToLower().Equals(region.ToLower()) &&
                                                             n.PostalCode.PostalCodeNumber.Equals(PostalCode) &&
                                                             n.Region.Country.CountryName.ToLower().Equals(country.ToLower())).SingleOrDefault();

                return city;
            }
        }
    }
}
