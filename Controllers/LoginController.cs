using HCMApp.Data;
using HCMApp.Data.Repositories;
using HCMApp.Data.Services;
using HCMApp.Data.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HCMApp.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly AppDbContext context;

        public LoginController(IUserRepository userRepository, AppDbContext context)
        {
            _userRepository = userRepository;
            this.context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            ViewData["Login"] = HttpContext.Request.Query["Login"].ToString();

            bool isAuthenticated = User.Identity.IsAuthenticated;
            
            if(isAuthenticated)
            {
                var claims = User.Identities.First().Claims.ToList();
                var userType = claims.ElementAt(1).Value;
                return RedirectToAction("Index", userType);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login([Bind] UserVM userVM)
        {
            if ((!string.IsNullOrEmpty(userVM.Username)) && (!string.IsNullOrEmpty(userVM.Password)))
            {
                var user = _userRepository.GetUser(userVM.Username, userVM.Password);
                if (user != null)
                {
                    var claims = new List<Claim>
                        {
                        new Claim(ClaimTypes.Name, userVM.Username),
                        new Claim(ClaimTypes.Role, user.UserType.UserTypeName)
                        };

                    var claimsIdentity = new ClaimsIdentity(claims,
                        CookieAuthenticationDefaults.AuthenticationScheme);

                    var authProperties = new AuthenticationProperties() { IsPersistent = userVM.RememberMe };

                    if(userVM.RememberMe)
                    {
                        authProperties.ExpiresUtc = DateTimeOffset.UtcNow.AddMonths(1);
                    }

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);

                    return RedirectToAction("Index", user.UserType.UserTypeName);
                } else
                {
                    var adminUser = context.AdminUsers.Where(n => n.Username.Equals(userVM.Username) && n.Password.Equals(userVM.Password)).SingleOrDefault();

                    if(adminUser != null)
                    {
                        var claims = new List<Claim>
                        {
                        new Claim(ClaimTypes.Name, userVM.Username),
                        new Claim(ClaimTypes.Role, "Admin")
                        };

                        var claimsIdentity = new ClaimsIdentity(claims,
                        CookieAuthenticationDefaults.AuthenticationScheme);

                        var authProperties = new AuthenticationProperties() { IsPersistent = userVM.RememberMe };

                        if (userVM.RememberMe)
                        {
                            authProperties.ExpiresUtc = DateTimeOffset.UtcNow.AddMonths(1);
                        }

                        await HttpContext.SignInAsync(
                            CookieAuthenticationDefaults.AuthenticationScheme,
                            new ClaimsPrincipal(claimsIdentity),
                            authProperties);

                        return RedirectToAction("Index", "Admin");
                    }
                }
            }

            RouteValueDictionary rvd = new RouteValueDictionary();
            rvd.Add("login", "badLogin");

            return RedirectToAction("Index", "Login", rvd);
        }
    }
}
