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
        private readonly Helper _helper;

        public TagService(DataService dataService,Helper helper)
        {
            _dataService = dataService;
            _helper = helper;
        }
        /// <summary>
        /// Get tags for a given artist by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Dictionary<string,Tag>> GetTags(string id)
        {
            var response = await _dataService.ApiGoogle("GET", null, "Tags/" + id, null);
            var tagList = _helper.Mapper<Dictionary<string,Tag>>(response);
            return tagList;
        }

        /// <summary>
        /// Adds a tag
        /// </summary>
        /// <param name="tagRequest"></param>
        /// <returns></returns>
        public async Task<string> AddTag(TagRequest tagRequest)
        {
            tagRequest.Tag.Id = Guid.NewGuid().ToString();
            var path = "Tags/" + tagRequest.Auth["uid"] + "/" + tagRequest.Tag.Id;
            var json = JsonSerializer.Serialize(tagRequest.Tag);
            var response = await _dataService.ApiGoogle("PUT", json, path, tagRequest.Auth);

            var tag = _helper.Mapper<Link>(response);

            return tag.Id;
        }
        /// <summary>
        /// Removes a tag
        /// </summary>
        /// <param name="tagRequest"></param>
        /// <returns></returns>
        public async Task RemoveTag(TagRequest tagRequest)
        {
            var path = "Tags/" + tagRequest.Auth["uid"] + "/" + tagRequest.Tag.Id;
            var response = await _dataService.ApiGoogle("DELETE", null, path, tagRequest.Auth);

        }
    }
}
