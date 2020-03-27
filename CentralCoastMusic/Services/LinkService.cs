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
        public async Task<List<string>> GetLinks(string id)
        {
            var linkResponse = await _dataService.ApiGoogle("GET", null, "Links/" + id, null);
            var linkList = JsonSerializer.Deserialize<List<string>>(linkResponse);
            return linkList;
        }

        public async Task AddLinks(LinkRequest linkRequest)
        {
            var json = JsonSerializer.Serialize(linkRequest.Link);
            var linkResponse = await _dataService.ApiGoogle("PUT", json, "Links/" + linkRequest.Auth["uid"], linkRequest.Auth);
         
        }

        public async Task RemoveLinks(LinkRequest linkRequest)
        {
            var json = JsonSerializer.Serialize(linkRequest.Link);
            var linkResponse = await _dataService.ApiGoogle("PATCH", json, "Links/" + linkRequest.Auth["uid"], linkRequest.Auth);

        }
    }
}
