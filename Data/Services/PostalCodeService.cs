using HCMApp.Data.Models;
using HCMApp.Data.Repositories;
using HCMApp.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HCMApp.Data.Services
{
    public class PostalCodeService
    {
        private readonly IPostalCodeRepository postalCodeRepository;
        private readonly AppDbContext context;

        public PostalCodeService(IPostalCodeRepository postalCodeRepository,
            AppDbContext context)
        {
            this.postalCodeRepository = postalCodeRepository;
            this.context = context;
        }

        public PostalCodeVM GetPostalCodeVMForCity(string cityName) =>
            postalCodeRepository.GetPostalCodes().Where(n => n.City.CityName.ToLower().Equals(cityName.ToLower()))
            .Select(postalCode => new PostalCodeVM()
            {
                PostalCode = postalCode.PostalCodeNumber
            }).SingleOrDefault();

        public PostalCode GetPostalCodeAndCreateIfNotExist(string cityName, string postalCode)
        {
            PostalCode localPostalCode = postalCodeRepository.GetPostalCodeForCity(cityName);

            if(localPostalCode != null)
            {
                return localPostalCode;
            } else
            {
                context.PostalCodes.Add(new PostalCode()
                {
                    PostalCodeNumber = postalCode,
                });

                context.SaveChanges();

                localPostalCode = postalCodeRepository.GetPostalCodeByCode(postalCode);

                return localPostalCode;
            }
        }
    }
}
