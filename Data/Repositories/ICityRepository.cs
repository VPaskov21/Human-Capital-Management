using HCMApp.Data.Models;
using System.Collections.Generic;

namespace HCMApp.Data.Repositories
{
    public interface ICityRepository
    {
        List<City> GetCities();
        City GetCityByName(string cityName);
    }
}