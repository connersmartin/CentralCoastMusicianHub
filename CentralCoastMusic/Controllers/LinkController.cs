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
        private readonly ArtistService _artistService;

        public LinkController(TagService tagService,StreamService streamService,ArtistService artistService)
        {
            _tagService = tagService;
            _streamService = streamService;
            _artistService = artistService;
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
            var artist = await _artistService.GetArtist(user);
            stream.Calendar = PopulateCalendar(artist, stream);
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

        public string PopulateCalendar(Artist artist, Stream stream)
        {
            var calendar = @"BEGIN:VCALENDAR
PRODID:-//Google Inc//Google Calendar 70.9054//EN

VERSION:2.0
CALSCALE:GREGORIAN
METHOD:REQUEST
BEGIN:VEVENT
DTSTART:{3}
DTEND:{4}
ORGANIZER;CN=""{0}"":mailto:test@test.com
DESCRIPTION:{1}:{2}
SEQUENCE:0
STATUS:CONFIRMED
SUMMARY:{0} live at {2}
TRANSP:OPAQUE
END:VEVENT
END:VCALENDAR";
            var startTime = stream.StartTime.ToString("yyyyMMddTHHmmss");
            var endTime = stream.EndTime.ToString("yyyyMMddTHHmmss");

            var result = string.Format(calendar, artist.Name, stream.Name, stream.Description, startTime, endTime).Replace("\r\n","\\n");

            return result;
        }

    }
}