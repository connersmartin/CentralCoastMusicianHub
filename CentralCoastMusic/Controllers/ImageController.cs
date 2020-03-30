using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public ImageController(ImageService imageService)
        {
            _imageService = imageService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadImage(string type)
        {
            var file = Request.Form.Files.FirstOrDefault();
            
            //Give file a GUID as a name
            //upload that guid to the user's profile

            _imageService.UploadFile(file);

            return Ok();
        }

        [HttpGet]
        public ActionResult GetImage(string id)
        {            
            //Get file guid from profile id
            //Return that image

            //_imageService.GetImage();

            return Ok();
        }

        [HttpDelete]
        public ActionResult RemoveImage(string id)
        {
            //Get file guid from profile id
            //Remove that image
            //Remove that id

            //_imageService.DeleteImage();

            return Ok();
        }
    }
}