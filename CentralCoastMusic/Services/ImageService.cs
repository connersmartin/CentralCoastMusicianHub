using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace CentralCoastMusic.Services
{
    public class ImageService
    {
        private readonly DataService _dataService;
        private readonly Helper _helper;

        public ImageService(DataService dataService,Helper helper)
        {
            _dataService = dataService;
            _helper = helper;
        }

        private string jsonCred = AppSettings.AppSetting["authJsonCred"];
        private string bucketName = AppSettings.AppSetting["googleStorageBucket"];
        private string imageUrl = AppSettings.AppSetting["imageBaseUrl"];

        /// <summary>
        /// Uploads the file to google cloud
        /// </summary>
        /// <param name="file"></param>
        /// <param name="objectName"></param>
        internal void UploadFile(IFormFile file,string objectName)
        {

            var storage = StorageClient.Create(GoogleCredential.FromJson(jsonCred), null);
            using (var f = file.OpenReadStream())
            {
                storage.UploadObject(bucketName, "ProfileImage/"+objectName, file.ContentType, f);                
            }

        }

        /// <summary>
        /// Gets the url of the image from the storage bucket
        /// </summary>
        /// <param name="objectName"></param>
        /// <returns></returns>
        internal string GetImage(string objectName)
        {
            return imageUrl + objectName;
        }
        /// <summary>
        /// deletes the image from the bucket and removes the reference in the artist
        /// </summary>
        /// <param name="objectName"></param>
        internal void RemoveImage(string objectName)
        {
            var storage = StorageClient.Create(GoogleCredential.FromJson(jsonCred), null);          
            storage.DeleteObject(bucketName, objectName);
            Console.WriteLine($"Deleted {objectName}.");
        }
        /// <summary>
        /// Adds profile image reference
        /// </summary>
        /// <param name="auth"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        internal async Task AddProfileImage(Dictionary<string,string> auth, string id)
        {
            var json = JsonSerializer.Serialize(new Dictionary<string, string>() { { "Id", id } });
            var sub = "ProfileImage/" + auth["uid"];   
            var response = await _dataService.ApiGoogle("PUT", json, sub, auth);
        }
        /// <summary>
        /// Gets profile image reference
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        internal async Task<string> GetProfileImage(string id)
        {            
            var sub = "ProfileImage/" + id;
            var response = await _dataService.ApiGoogle("GET", null, sub, null);

            var imageId = _helper.Mapper<Dictionary<string, string>>(response);

            return imageId==null? "": imageId.FirstOrDefault().Value;
        }
        /// <summary>
        /// Removes profile image reference
        /// </summary>
        /// <param name="auth"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        internal async Task RemoveProfileImage(Dictionary<string, string> auth, string id)
        {
            var sub = "ProfileImage/" + auth["uid"];
            var response = await _dataService.ApiGoogle("DELETE", null, sub, auth);
        }
    }
}
