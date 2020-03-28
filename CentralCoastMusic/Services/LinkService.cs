using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using CentralCoastMusic.Models;

namespace CentralCoastMusic.Services
{
    public class LinkService
    {
        private readonly DataService _dataService;

        public LinkService(DataService dataService)
        {
            _dataService = dataService;
        }
        public async Task<Dictionary<string, Link>> GetLinks(string id)
        {
            var linkResponse = await _dataService.ApiGoogle("GET", null, "Links/" + id, null);
            var linkList = JsonSerializer.Deserialize<Dictionary<string, Link>>(linkResponse);
            return linkList;
        }

        public async Task<string> AddLink(LinkRequest linkRequest)
        {
            linkRequest.Link.Id = Guid.NewGuid().ToString();
            var path = "Links/" + linkRequest.Auth["uid"] + "/" + linkRequest.Link.Id;
            var json = JsonSerializer.Serialize(linkRequest.Link);
            var linkResponse = await _dataService.ApiGoogle("PUT", json, path, linkRequest.Auth);

            var link = JsonSerializer.Deserialize<Link>(linkResponse);

            return link.Id;
        }

        public async Task RemoveLink(LinkRequest linkRequest)
        {
            var path = "Links/" + linkRequest.Auth["uid"] + "/" + linkRequest.Link.Id;
            var linkResponse = await _dataService.ApiGoogle("PATCH", null, path, linkRequest.Auth);

        }
    }
}
