using HCMApp.Data.Models;
using HCMApp.Data.Repositories;
using HCMApp.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HCMApp.Data.Services
{
    public class AddressService
    {
        private readonly AppDbContext context;
        private readonly IUserRepository _userRepository;
        private readonly CityService cityService;

        public AddressService(AppDbContext context,
            IUserRepository userRepository,
            CityService cityService)
        {
            this.context = context;
            _userRepository = userRepository;
            this.cityService = cityService;
        }

        public Address GetAddress(string streetAddress, string streetAddressAdditional, string City, string Region, string Postal_Code, string Country)
        {
            Address address = null;
            if (streetAddressAdditional == null)
            {
                address = context.Addresses.Where(n => n.StreetAddress.ToLower().Equals(streetAddress.ToLower()) &&
                                                        n.City.CityName.ToLower().Equals(City.ToLower()) &&
                                                        n.City.Region.RegionName.ToLower().Equals(Region.ToLower())).SingleOrDefault();
            } else
            {
                address = context.Addresses.Where(n => n.StreetAddress.ToLower().Equals(streetAddress.ToLower()) &&
                                                        n.StreetAddressAdditional.ToLower().Equals(streetAddressAdditional.ToLower()) &&
                                                        n.City.CityName.ToLower().Equals(City.ToLower()) &&
                                                        n.City.Region.RegionName.ToLower().Equals(Region.ToLower())).SingleOrDefault();
            }
            

            if(address != null)
            {
                return address;
            } 
            else
            {
                context.Addresses.Add(new Address()
                {
                    StreetAddress = streetAddress,
                    StreetAddressAdditional = streetAddressAdditional == null ? null : streetAddressAdditional,
                    CityId = cityService.GetCityAndCreateIfNotExist(City, Region, Postal_Code, Country).Id
                });

                context.SaveChanges();

                if(streetAddressAdditional == null)
                {
                    address = context.Addresses.Where(n => n.StreetAddress.ToLower().Equals(streetAddress.ToLower()) &&
                                                        n.City.CityName.ToLower().Equals(City.ToLower()) &&
                                                        n.City.Region.RegionName.ToLower().Equals(Region.ToLower())).SingleOrDefault();
                } else
                {
                    address = context.Addresses.Where(n => n.StreetAddress.ToLower().Equals(streetAddress.ToLower()) &&
                                                        n.StreetAddressAdditional.ToLower().Equals(streetAddressAdditional.ToLower()) &&
                                                        n.City.CityName.ToLower().Equals(City.ToLower()) &&
                                                        n.City.Region.RegionName.ToLower().Equals(Region.ToLower())).SingleOrDefault();
                }

                

                return address;
            }
        }
        public AddressVM GetCurrentUserAddress(string username)
        {
            var currentUser = _userRepository.GetUserByUsername(username);

            return new AddressVM()
            {
                Address = currentUser.Address.StreetAddress,
                Additional_Address = currentUser.Address.StreetAddressAdditional,
                City = currentUser.Address.City.CityName,
                Region = currentUser.Address.City.Region.RegionName,
                Postal_Code = currentUser.Address.City.PostalCode.PostalCodeNumber,
                Country = currentUser.Address.City.Region.Country.CountryName
            };
        }
    }
}
