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
        //https://cloud.google.com/storage/docs/uploading-objects#storage-upload-object-csharp
        internal void UploadFile(IFormFile file)
        {
            string objectName = file.FileName;

            var storage = StorageClient.Create(GoogleCredential.FromJson(jsonCred), null);
            using (var f = file.OpenReadStream())
            {
                storage.UploadObject(bucketName, "ProfileImage/"+objectName, file.ContentType, f);                
            }

        }

        //https://cloud.google.com/storage/docs/downloading-objects
        internal void GetImage(string bucketName, string objectName, string localPath = null)
        {
            var storage = StorageClient.Create(GoogleCredential.FromJson(jsonCred), null);
            //need to research this
            using (var outputFile = File.OpenWrite(localPath))
            {
                storage.DownloadObject(bucketName, objectName, outputFile);
            }
            Console.WriteLine($"downloaded {objectName} to {localPath}.");
        }

        internal void RemoveImage(string bucketName, string objectName, string localPath = null)
        {
            var storage = StorageClient.Create(GoogleCredential.FromJson(jsonCred), null);
            //need to research this
            using (var outputFile = File.OpenWrite(localPath))
            {
                storage.DownloadObject(bucketName, objectName, outputFile);
            }
            Console.WriteLine($"downloaded {objectName} to {localPath}.");
        }
    }
}
