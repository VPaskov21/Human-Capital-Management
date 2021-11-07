using HCMApp.Data.Models;
using HCMApp.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HCMApp.Data.Services
{
    public class LoginService
    {
        private readonly AppDbContext context;
        private readonly IUserRepository userRepository;

        public LoginService(AppDbContext context, IUserRepository userRepository)
        {
            this.context = context;
            this.userRepository = userRepository;
        }

        public User GetUser(string username, string password) => context.Users.FirstOrDefault(u => u.Username.Equals(username) && u.Password.Equals(password));

        public User GetUserByUsername(string username) => userRepository.GetUserByUsername(username);
    }
}
