using HCMApp.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HCMApp.Data.Repositories
{
    public class CityRepository : ICityRepository
    {
        private readonly AppDbContext context;

        public CityRepository(AppDbContext context)
        {
            this.context = context;
        }

        public List<City> GetCities() => context.Cities.Include(n => n.Region)
                                                            .ThenInclude(n => n.Country)
                                                        .Include(n => n.PostalCode)
                                                        .ToList();

        public City GetCityByName(string cityName) => GetCities().Where(n => n.CityName.ToLower().Equals(cityName.ToLower())).SingleOrDefault();
    }
}
