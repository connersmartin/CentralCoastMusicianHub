using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CentralCoastMusic.Models;
using CentralCoastMusic.Services;

namespace CentralCoastMusic.Controllers
{
    public class MusicController : Controller
    {
        private readonly ILogger<MusicController> _logger;
        private readonly ArtistService _artistService;
        private readonly StreamService _streamService;

        public MusicController(ILogger<MusicController> logger,
                                ArtistService artistService,
                                StreamService streamService)
        {
            _logger = logger;
            _artistService = artistService;
            _streamService = streamService;
        }

        //Gets all artists
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetArtists()
        {
            var artists = await _artistService.GetArtists();

            artists = await _streamService.AddUpcomingStreamToArtists(artists);
            
            return PartialView(artists);
        }
        public async Task<IActionResult> ArtistSearch(string id)
        {
            var artists = await _artistService.SearchArtists(id);
            artists = await _streamService.AddUpcomingStreamToArtists(artists);
            return PartialView("GetArtists", artists);
        }

        public async Task<IActionResult> TagSearch(string id)
        {
            var artists = await _artistService.TagSearch(id);
            artists = await _streamService.AddUpcomingStreamToArtists(artists);
            return PartialView("GetArtists", artists);
        }


        public async Task<IActionResult> Details(string id)
        {
            var artist = await _artistService.GetArtist(id);
            if (artist != null)
            {
                var artists = await _streamService.AddUpcomingStreamToArtists(new List<Artist>() { artist});

                return PartialView(artists.FirstOrDefault());
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        public IActionResult About()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
