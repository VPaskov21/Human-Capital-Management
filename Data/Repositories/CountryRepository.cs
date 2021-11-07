using HCMApp.Data.Models;
using HCMApp.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HCMApp.Data.Repositories
{
    public class CountryRepository : ICountryRepository
    {
        private readonly AppDbContext context;

        public CountryRepository(AppDbContext context)
        {
            this.context = context;
        }

        public List<Country> GetCountries() => context.Countries.ToList();

        public Country GetCountryByName(string countryName) => GetCountries().Where(n => n.CountryName.ToLower().Equals(countryName.ToLower())).SingleOrDefault();

    }
}
