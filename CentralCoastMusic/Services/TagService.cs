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
        public async Task<Dictionary<string,Tag>> GetTags(string id)
        {
            var response = await _dataService.ApiGoogle("GET", null, "Tags/" + id, null);
            var tagList = JsonSerializer.Deserialize<Dictionary<string,Tag>>(response);
            return tagList;
        }

        public async Task<string> AddTag(TagRequest tagRequest)
        {
            tagRequest.Tag.Id = Guid.NewGuid().ToString();
            var path = "Tags/" + tagRequest.Auth["uid"] + "/" + tagRequest.Tag.Id;
            var json = JsonSerializer.Serialize(tagRequest.Tag);
            var response = await _dataService.ApiGoogle("PUT", json, path, tagRequest.Auth);

            var tag = JsonSerializer.Deserialize<Link>(response);

            return tag.Id;
        }

        public async Task RemoveTag(TagRequest tagRequest)
        {
            var path = "Tags/" + tagRequest.Auth["uid"] + "/" + tagRequest.Tag.Id;
            var response = await _dataService.ApiGoogle("DELETE", null, path, tagRequest.Auth);

        }
    }
}
