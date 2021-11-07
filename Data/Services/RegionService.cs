using HCMApp.Data.Models;
using HCMApp.Data.Repositories;
using HCMApp.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HCMApp.Data.Services
{
    public class RegionService
    {
        private readonly IRegionRepository regionRepository;
        private readonly AppDbContext context;
        private readonly ICountryRepository countryRepository;

        public RegionService(IRegionRepository regionRepository,
            AppDbContext context,
            ICountryRepository countryRepository)
        {
            this.regionRepository = regionRepository;
            this.context = context;
            this.countryRepository = countryRepository;
        }

        public List<RegionVM> GetRegionVMs() =>
            regionRepository.GetRegions().Select(region => new RegionVM()
            {
                RegionName = region.RegionName
            }).ToList();


        public List<RegionVM> GetRegionVMsForCountry(string countryName) =>
            regionRepository.GetRegions().Where(n => n.Country.CountryName.ToLower().Equals(countryName.ToLower()))
            .Select(region => new RegionVM()
            {
                Id = region.Id,
                RegionName = region.RegionName
            }).ToList();

        public Region GetRegionAndCreateIfNotExist(string regionName, string Country)
        {
            Region region = regionRepository.GetRegionByName(regionName);

            if (region != null)
            {
                return region;
            }
            else
            {
                context.Regions.Add(new Region()
                {
                    RegionName = regionName,
                    Country = countryRepository.GetCountryByName(Country)
                });

                context.SaveChanges();

                region = regionRepository.GetRegions().Where(n => n.RegionName.ToLower().Equals(regionName.ToLower()) &&
                                                    n.Country.CountryName.ToLower().Equals(Country.ToLower())).SingleOrDefault();

                return region;
            }
        }
    }
}
