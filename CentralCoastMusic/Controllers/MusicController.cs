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

        public MusicController(ILogger<MusicController> logger,
                                ArtistService artistService)
        {
            _logger = logger;
            _artistService = artistService;
        }

        //Gets all artists
        public async Task<IActionResult> Index()
        {
            /*
            ViewData["Genres"] = await _artistService.GetGenres();
            */
            return View();
        }

        public async Task<IActionResult> GetArtists()
        {
            var artists = await _artistService.GetArtists();

            return PartialView(artists);
        }

        public async Task<IActionResult> Details(string id)
        {
            var artist = await _artistService.GetArtist(id);
            if (artist != null)
            {
                return PartialView(artist);
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
