using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CentralCoastMusic.Models;
using CentralCoastMusic.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CentralCoastMusic.Controllers
{
    public class ArtistController : Controller
    {
        private readonly ILogger<ArtistController> _logger;
        private readonly ArtistService _artistService;

        public ArtistController(ILogger<ArtistController> logger,
                                ArtistService artistService)
        {
            _logger = logger;
            _artistService = artistService;
        }

        // GET: Artist
        public ActionResult Index()
        {
            return View();
        }

        // GET: Artist/Details/5
        public async Task<IActionResult> Details(string id)
        {
            await _artistService.GetArtists(id);
            return View();
        }

        // GET: Artist/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Artist/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Artist artist)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Artist/Edit/5
        public ActionResult Edit(string id)
        {
            return View();
        }

        // POST: Artist/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Artist artist)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Artist/Delete/5
        public ActionResult Delete(string id)
        {
            return View();
        }

        // POST: Artist/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}