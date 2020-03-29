using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CentralCoastMusic.Services;
using CentralCoastMusic.Models;

namespace CentralCoastMusic.Controllers
{
    public class LinkController : Controller
    {
        private readonly TagService _tagService;
        private readonly StreamService _streamService;

        public LinkController(TagService tagService,StreamService streamService)
        {
            _tagService = tagService;
            _streamService = streamService;
        }
        public async Task<IActionResult> GetStreams()
        {
            HttpContext.Request.Cookies.TryGetValue("uid", out string user);
            HttpContext.Request.Cookies.TryGetValue("token", out string token);
            var auth = new Dictionary<string, string>()
            {
                {"uid", user },
                {"token",token }
            };
            var streamResponse = await _streamService.GetStreams(auth["uid"]);
            var linkList = streamResponse.Select(l => l.Value).ToList();
            return PartialView(linkList);
        }

        public async Task<IActionResult> GetTags()
        {
            HttpContext.Request.Cookies.TryGetValue("uid", out string user);
            HttpContext.Request.Cookies.TryGetValue("token", out string token);
            var auth = new Dictionary<string, string>()
            {
                {"uid", user },
                {"token",token }
            };
            var tagResponse = await _tagService.GetTags(auth["uid"]);
            var tagList = tagResponse.Select(l => l.Value).ToList();
            return PartialView(tagList);
        }

        public async Task<string> AddTag([FromBody] Tag tag)
        {

            HttpContext.Request.Cookies.TryGetValue("uid", out string user);
            HttpContext.Request.Cookies.TryGetValue("token", out string token);
            var auth = new Dictionary<string, string>()
            {
                {"uid", user },
                {"token",token }
            };
            var tagResponse = await _tagService.AddTag(new TagRequest() { Auth = auth, Tag = tag });
            return tagResponse;
        }

        public async Task RemoveTag(string id)
        {
            HttpContext.Request.Cookies.TryGetValue("uid", out string user);
            HttpContext.Request.Cookies.TryGetValue("token", out string token);
            var auth = new Dictionary<string, string>()
            {
                {"uid", user },
                {"token",token }
            };            
            await _tagService.RemoveTag(new TagRequest() { Auth = auth, Tag = new Tag() { Id = id } });
        }

        public async Task<string> AddStream([FromBody] Stream stream)
        {
            HttpContext.Request.Cookies.TryGetValue("uid", out string user);
            HttpContext.Request.Cookies.TryGetValue("token", out string token);
            var auth = new Dictionary<string, string>()
            {
                {"uid", user },
                {"token",token }
            };
            var response = await _streamService.AddStream(new StreamRequest() { Auth = auth, Stream = stream });

            return response;
        }

        public async Task RemoveStream(string id)
        {
            HttpContext.Request.Cookies.TryGetValue("uid", out string user);
            HttpContext.Request.Cookies.TryGetValue("token", out string token);
            var auth = new Dictionary<string, string>()
            {
                {"uid", user },
                {"token",token }
            };
            await _streamService.RemoveStream(new StreamRequest() { Auth = auth, Stream = new Stream() { Id = id } });

        }

    }
}