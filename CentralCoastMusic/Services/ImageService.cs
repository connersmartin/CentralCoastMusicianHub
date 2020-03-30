using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CentralCoastMusic.Services
{
    public class ImageService
    {
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
    }
}
