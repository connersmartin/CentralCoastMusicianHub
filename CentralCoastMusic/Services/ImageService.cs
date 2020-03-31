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

        public ImageService(DataService dataService)
        {
            _dataService = dataService;
        }


        private string jsonCred = AppSettings.AppSetting["authJsonCred"];
        private string bucketName = AppSettings.AppSetting["googleStorageBucket"];
        private string imageUrl = AppSettings.AppSetting["imageBaseUrl"];

        //https://cloud.google.com/storage/docs/uploading-objects#storage-upload-object-csharp
        internal void UploadFile(IFormFile file,string objectName)
        {

            var storage = StorageClient.Create(GoogleCredential.FromJson(jsonCred), null);
            using (var f = file.OpenReadStream())
            {
                storage.UploadObject(bucketName, "ProfileImage/"+objectName, file.ContentType, f);                
            }

        }

        //https://cloud.google.com/storage/docs/downloading-objects
        internal string GetImage(string objectName)
        {
            return imageUrl + objectName;
        }

        internal void RemoveImage(string objectName)
        {
            var storage = StorageClient.Create(GoogleCredential.FromJson(jsonCred), null);          
            storage.DeleteObject(bucketName, objectName);
            Console.WriteLine($"Deleted {objectName}.");
        }

        internal async Task AddProfileImage(Dictionary<string,string> auth, string id)
        {
            var json = JsonSerializer.Serialize(new Dictionary<string, string>() { { "Id", id } });
            var sub = "ProfileImage/" + auth["uid"];   
            var response = await _dataService.ApiGoogle("PUT", json, sub, auth);
        }

        internal async Task<string> GetProfileImage(string id)
        {            
            var sub = "ProfileImage/" + id;
            var response = await _dataService.ApiGoogle("GET", null, sub, null);

            var imageId = JsonSerializer.Deserialize<Dictionary<string, string>>(response);

            return imageId==null? "": imageId.FirstOrDefault().Value;
        }

        internal async Task RemoveProfileImage(Dictionary<string, string> auth, string id)
        {
            var sub = "ProfileImage/" + auth["uid"];
            var response = await _dataService.ApiGoogle("DELETE", null, sub, auth);
        }
    }
}
