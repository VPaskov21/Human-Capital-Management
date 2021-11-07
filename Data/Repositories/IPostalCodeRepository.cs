using HCMApp.Data.Models;
using System.Collections.Generic;

namespace HCMApp.Data.Repositories
{
    public interface IPostalCodeRepository
    {
        PostalCode GetPostalCodeByCode(string postalCode);
        PostalCode GetPostalCodeForCity(string cityName);
        List<PostalCode> GetPostalCodes();
    }
}