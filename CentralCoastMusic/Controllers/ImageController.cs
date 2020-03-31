using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CentralCoastMusic.Models;
using CentralCoastMusic.Services;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CentralCoastMusic.Controllers
{
    public class ImageController : Controller
    {
        private readonly ImageService _imageService;
        private readonly ArtistService _artistService;
        private readonly StreamService _streamService;
        public ImageController(ImageService imageService, ArtistService artistService,StreamService streamService)
        {
            _imageService = imageService;
            _artistService = artistService;
            _streamService = streamService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> UploadImage(string type)
        {
            HttpContext.Request.Cookies.TryGetValue("uid", out string user);
            HttpContext.Request.Cookies.TryGetValue("token", out string token);
            var auth = new Dictionary<string, string>()
            {
                {"uid", user },
                {"token",token }
            };

            var file = Request.Form.Files.FirstOrDefault();

            //Give file a GUID as a name
            var id = Guid.NewGuid().ToString();
            //upload that file                                   
            _imageService.UploadFile(file,id);
            //get that profileimage linked to user
            await _imageService.AddProfileImage(auth, id);

            return Ok();
        }

        [HttpGet]
        public async Task<string> GetImage(string id, string type)
        {
            var imageId = "";
            switch(type)
            {
                case "ProfileImage":
                    imageId = await _imageService.GetProfileImage(id);                    
                    break;

            }
            //need to figure out stream images

            //Get file guid from profile id
            //Return that image
            return _imageService.GetImage(type + "/" + imageId);
        }

        [HttpDelete]
        public async Task<ActionResult> RemoveImage(string type)
        {
            //Get file guid from profile id
            //Remove that image
            //Remove that id         


            HttpContext.Request.Cookies.TryGetValue("uid", out string user);
            HttpContext.Request.Cookies.TryGetValue("token", out string token);
            var auth = new Dictionary<string, string>()
            {
                {"uid", user },
                {"token",token }
            };

            var imageId = await _imageService.GetProfileImage(user);

            _imageService.RemoveImage(type+"/"+imageId);
            await _imageService.RemoveProfileImage(auth, user);


            return Ok();

        }
    }
}