using HCMApp.Data.Models;
using System.Collections.Generic;

namespace HCMApp.Data.Repositories
{
    public interface ICountryRepository
    {
        List<Country> GetCountries();
        Country GetCountryByName(string countryName);
    }
}