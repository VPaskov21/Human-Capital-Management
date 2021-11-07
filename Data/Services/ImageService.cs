using HCMApp.Data.Repositories;
using HCMApp.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;

namespace HCMApp.Data.Services
{
    public class ImageService
    {
        private readonly AppDbContext context;
        private readonly IUserRepository _userRepository;
        private readonly IWebHostEnvironment webHostEnvironment;

        public ImageService(AppDbContext context,
            IUserRepository userRepository,
            IWebHostEnvironment webHostEnvironment)
        {
            this.context = context;
            _userRepository = userRepository;
            this.webHostEnvironment = webHostEnvironment;
        }

        public ImageVM GetImageSource(string username)
        {
            var currentUser = _userRepository.GetUserByUsername(username);

            return new ImageVM()
            {
                ImageSrc = currentUser.ImageSrc
            };
        }

        public void SaveImage(HttpContext httpContext, string username)
        {
            var newFileName = string.Empty;

            if (httpContext.Request.Form.Files != null)
            {
                var fileName = string.Empty;
                string PathDB = string.Empty;

                var file = httpContext.Request.Form.Files.FirstOrDefault();


                if (file.Length > 0)
                {
                    //Getting FileName
                    fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');

                    //Assigning Unique Filename (Guid)
                    //var myUniqueFileName = Convert.ToString(Guid.NewGuid());

                    //Getting file Extension
                    var FileExtension = Path.GetExtension(fileName);

                    // concating  FileName + FileExtension
                    newFileName = username + FileExtension;

                    // Combines two strings into a path.
                    fileName = Path.Combine(webHostEnvironment.WebRootPath, "profile_images") + $@"\{newFileName}";

                    
                    using (FileStream fs = System.IO.File.Create(fileName))
                    {
                        file.CopyTo(fs);
                        fs.Flush();
                    }

                    var user = _userRepository.GetUsers().Where(n => n.Username.Equals(username)).SingleOrDefault();
                    user.ImageSrc = newFileName;

                    context.SaveChanges();
                }
            }
        }
    }
}
