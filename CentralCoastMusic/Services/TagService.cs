using System;
using System.Collections.Generic;
using System.Linq;
using CentralCoastMusic.Models;
using System.Text.Json;
using System.Threading.Tasks;

namespace CentralCoastMusic.Services
{
    public class TagService
    {
        private readonly DataService _dataService;

        public TagService(DataService dataService)
        {
            _dataService = dataService;
        }
        public async Task<Dictionary<string,Link>> GetTags(string id)
        {
            var linkResponse = await _dataService.ApiGoogle("GET", null, "Tags/" + id, null);
            var linkList = JsonSerializer.Deserialize<Dictionary<string,Link>>(linkResponse);
            return linkList;
        }

        public async Task<string> AddTag(LinkRequest linkRequest)
        {
            linkRequest.Link.Id = Guid.NewGuid().ToString();
            var path = "Tags/" + linkRequest.Auth["uid"] + "/" + linkRequest.Link.Id;
            var json = JsonSerializer.Serialize(linkRequest.Link);
            var linkResponse = await _dataService.ApiGoogle("PUT", json, path, linkRequest.Auth);

            var link = JsonSerializer.Deserialize<Link>(linkResponse);

            return link.Id;
        }

        public async Task RemoveTag(LinkRequest linkRequest)
        {
            var path = "Tags/" + linkRequest.Auth["uid"] + "/" + linkRequest.Link.Id;
            var linkResponse = await _dataService.ApiGoogle("DELETE", null, path, linkRequest.Auth);

        }
    }
}
