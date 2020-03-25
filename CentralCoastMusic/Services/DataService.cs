using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CentralCoastMusic.Services
{
    public class DataService
    {
        private readonly AuthService _authService;

        public DataService(AuthService authService)
        {
            _authService = authService;
        }

        //How do I want to data?
        private HttpClient _client = new HttpClient();
        private string baseUrl = AppSettings.AppSetting["firebaseBaseUrl"];


        //TODO clean up and test
        public async Task<byte[]> ApiGoogle(string method, string json, string sub,
            Dictionary<string, string> auth)
        {
            var token = "";
            var authCheck = "";
            if (auth != null)
            {
                token = auth["token"];
                //Make sure user is authorized
                authCheck = await _authService.Google(token);
            }
            var url = baseUrl + sub + ".json";
            var res = new HttpResponseMessage();

            if (method == "GET")
            {
                //our GET shouldn't require auth
                res = await _client.GetAsync(url);
            }
            else if (authCheck == auth["uid"])
            {
                url+= "?auth=" + token;
                switch (method)
                {
                    case "PUT":
                        HttpContent newContent = new StringContent(json, Encoding.UTF8, "application/json");
                        res = await _client.PutAsync(url, newContent);
                        break;
                    case "PATCH":
                        HttpContent updateContent = new StringContent(json, Encoding.UTF8, "application/json");
                        res = await _client.PatchAsync(url, updateContent);
                        break;
                    case "DELETE":
                        res = await _client.DeleteAsync(url);
                        break;
                }
            }
            var debugText = await res.Content.ReadAsStringAsync();
            
            var interim = await res.Content.ReadAsByteArrayAsync();

            return interim;
        }
    }
}
