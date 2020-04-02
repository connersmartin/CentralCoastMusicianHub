using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CentralCoastMusic.Services;
using CentralCoastMusic.Models;
using System.Text;

namespace CentralCoastMusic.Controllers
{
    public class LinkController : Controller
    {
        private readonly TagService _tagService;
        private readonly StreamService _streamService;
        private readonly ArtistService _artistService;
        private string adminEmail= AppSettings.AppSetting["adminEmail"];
        const string textLineFeed = @"\" + "n";

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

        public async Task<string> GetStreamAttachment(string id)
        {
            var streamResponse = await _streamService.GetSingleStream(id);
            return streamResponse.Calendar;
        }

        public string PopulateCalendar(Artist artist, Stream stream)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("BEGIN:VCALENDAR");
            sb.AppendLine("PRODID:-//Google Inc//Google Calendar 70.9054//EN");
            sb.AppendLine("VERSION:2.0");
            sb.AppendLine("CALSCALE:GREGORIAN");
            sb.AppendLine("METHOD:REQUEST");
            sb.AppendLine("BEGIN:VEVENT");
            sb.AppendLine("DTSTART:{3}");
            sb.AppendLine("DTEND:{4}");
            sb.AppendLine("ORGANIZER;CN={0}:mailto:{5}");
            sb.AppendLine("DESCRIPTION:{1}:{2}");
            sb.AppendLine("SEQUENCE:0");
            sb.AppendLine("STATUS:CONFIRMED");
            sb.AppendLine("SUMMARY:{0} live at {2}");
            sb.AppendLine("TRANSP:OPAQUE");
            sb.AppendLine("END:VEVENT");
            sb.AppendLine("END:VCALENDAR");
            var startTime = stream.StartTime.ToString("yyyyMMddTHHmmss");
            var endTime = stream.EndTime.ToString("yyyyMMddTHHmmss");

            var result = string.Format(sb.ToString(), artist.Name, stream.Name, stream.Description, startTime, endTime,adminEmail);

            return result;
        }

    }
}