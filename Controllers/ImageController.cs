using HCMApp.Data.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HCMApp.Controllers
{
    public class ImageController : Controller
    {
        private readonly ImageService imageService;

        public ImageController(ImageService imageService)
        {
            this.imageService = imageService;
        }/*
        public PartialViewResult Action()
        {
            var currentUser = HttpContext.User.Identity.Name;
            var imageSrc = imageService.GetImageSource(currentUser);
            return PartialView(imageSrc);
        }

        public IActionResult OnGetPartial()
        {
            var currentUser = HttpContext.User.Identity.Name;
            ViewData["ImageSource"] = imageService.GetImageSource(HttpContext.User.Identity.Name);
            return new PartialViewResult
            {
                ViewName = "ImagePartialView",
                ViewData = ViewData,
         };
        }

        public PartialViewResult Index()
        {
            var currentUser = HttpContext.User.Identity.Name;
            ViewData["ImageSource"] = imageService.GetImageSource(HttpContext.User.Identity.Name);
            var imageSrc = imageService.GetImageSource(currentUser);
            return PartialView("ImagePartialView", imageSrc);
        }
        */
    }
}
