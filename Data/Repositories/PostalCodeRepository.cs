using HCMApp.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HCMApp.Data.Repositories
{
    public class PostalCodeRepository : IPostalCodeRepository
    {
        private readonly AppDbContext context;

        public PostalCodeRepository(AppDbContext context)
        {
            this.context = context;
        }

        public List<PostalCode> GetPostalCodes() => context.PostalCodes.Include(n => n.City).ToList();

        public PostalCode GetPostalCodeForCity(string cityName) => GetPostalCodes().Where(n => n.City.CityName.ToLower().Equals(cityName.ToLower())).SingleOrDefault();

        public PostalCode GetPostalCodeByCode(string postalCode) => GetPostalCodes().Where(n => n.PostalCodeNumber.Equals(postalCode)).SingleOrDefault();
    }
}
