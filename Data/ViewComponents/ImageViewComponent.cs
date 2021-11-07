using HCMApp.Data.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HCMApp.Controllers
{
    public class ImageViewComponent : ViewComponent
    {
        private readonly ImageService imageService;

        public ImageViewComponent(ImageService imageService)
        {
            this.imageService = imageService;
        }

        public IViewComponentResult Invoke()
        {
            var currentUser = HttpContext.User.Identity.Name;
            ViewData["ImageSource"] = imageService.GetImageSource(HttpContext.User.Identity.Name);
            var imageSrc = imageService.GetImageSource(currentUser);
            return View(imageSrc);
        }
    }
}
