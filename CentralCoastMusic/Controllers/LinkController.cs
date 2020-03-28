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
        private readonly LinkService _linkService;

        public LinkController(TagService tagService,LinkService linkService)
        {
            _tagService = tagService;
            _linkService = linkService;
        }
        public async Task<IActionResult> GetLinks()
        {
            HttpContext.Request.Cookies.TryGetValue("uid", out string user);
            HttpContext.Request.Cookies.TryGetValue("token", out string token);
            var auth = new Dictionary<string, string>()
            {
                {"uid", user },
                {"token",token }
            };
            var linkResponse = await _linkService.GetLinks(auth["uid"]);
            var linkList = linkResponse.Select(l => l.Value).ToList();
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

        public async Task<string> AddTag([FromBody] Link link)
        {

            HttpContext.Request.Cookies.TryGetValue("uid", out string user);
            HttpContext.Request.Cookies.TryGetValue("token", out string token);
            var auth = new Dictionary<string, string>()
            {
                {"uid", user },
                {"token",token }
            };
            var tagResponse = await _tagService.AddTag(new LinkRequest() { Auth = auth, Link = link });
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
            await _tagService.RemoveTag(new LinkRequest() { Auth = auth, Link = new Link() { Id = id } });
        }

        public async Task<string> AddLink([FromBody] Link link)
        {
            HttpContext.Request.Cookies.TryGetValue("uid", out string user);
            HttpContext.Request.Cookies.TryGetValue("token", out string token);
            var auth = new Dictionary<string, string>()
            {
                {"uid", user },
                {"token",token }
            };
            var linkResponse = await _linkService.AddLink(new LinkRequest() { Auth = auth, Link = link });

            return linkResponse;
        }

        public async Task RemoveLink(string id)
        {
            HttpContext.Request.Cookies.TryGetValue("uid", out string user);
            HttpContext.Request.Cookies.TryGetValue("token", out string token);
            var auth = new Dictionary<string, string>()
            {
                {"uid", user },
                {"token",token }
            };
            await _linkService.RemoveLink(new LinkRequest() { Auth = auth, Link = new Link() { Id = id } });

        }

    }
}