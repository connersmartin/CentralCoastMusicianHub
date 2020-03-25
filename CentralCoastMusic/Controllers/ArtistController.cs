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
        private readonly AuthService _authService;

        public ArtistController(ILogger<ArtistController> logger,
                                ArtistService artistService,
                                AuthService authService)
        {
            _logger = logger;
            _artistService = artistService;
            _authService = authService;
        }

        // GET: Artist
        public ActionResult Index()
        {
            ViewData["jsSettings"] = AppSettings.AppSetting["jsSettings"];
            var dict = GetCookies();
            //Check to see if logged in
            if (dict["uid"] != null && dict["token"] != null)
            {
                return View("Dashboard");
            }
            else
            {                
                return View();
            }
            
        }

        public ActionResult Dashboard()
        {
            //This will show their profile and edit options
            return View();
        }

        // GET: Artist/Details/5
        public async Task<IActionResult> Details(string id)
        {
            await _artistService.GetArtists(id);
            return View();
        }

        // GET: Artist/Create
        public async Task<ActionResult> Create()
        {
            return View();
        }

        // POST: Artist/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Artist artist)
        {
            try
            {
                // TODO: Add insert logic here
                await _artistService.AddArtist(new ArtistRequest());
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Artist/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            await _artistService.GetArtists(id);

            return View();
        }

        // POST: Artist/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Artist artist)
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
        public async Task<ActionResult> Delete(string id)
        {
            return View();
        }

        // POST: Artist/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(string id, IFormCollection collection)
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

        #region Cookie Management

        public async Task<bool> SetAuth(string auth, string uid)
        {
            var dict = GetCookies();

            var rq = HttpContext.Request.Headers;

            var a = rq["uid"];
            var b = rq["token"];
            var x = HttpContext.User;

            if (a.ToString() == null || b.ToString() == null)
            {
                a = dict["uid"];
                b = dict["token"];
            }

            if (a == await _authService.Google(b))
            {
                if (dict["uid"] == null && dict["token"] == null)
                {
                    Set("token", b, 720);
                    Set("uid", a, 720);
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public Dictionary<string, string> GetCookies()
        {
            return new Dictionary<string, string>()
            {
                {"uid", Get("uid")},
                {"token", Get("token") }
            };
        }

        /// <summary>  
        /// Get the cookie  
        /// </summary>  
        /// <param name="key">Key </param>  
        /// <returns>string value</returns>  
        public string Get(string key)
        {
            return Request.Cookies[key];
        }
        /// <summary>  
        /// set the cookie  
        /// </summary>  
        /// <param name="key">key (unique indentifier)</param>  
        /// <param name="value">value to store in cookie object</param>  
        /// <param name="expireTime">expiration time</param>  
        public void Set(string key, string value, int? expireTime)
        {
            CookieOptions option = new CookieOptions();
            if (expireTime.HasValue)
                option.Expires = DateTime.Now.AddMinutes(expireTime.Value);
            else
                option.Expires = DateTime.Now.AddMilliseconds(100000);
            Response.Cookies.Append(key, value, option);
        }
        /// <summary>  
        /// Delete the key  
        /// </summary>  
        /// <param name="key">Key</param>  
        public void Remove(string key)
        {
            Response.Cookies.Delete(key);
        }
        #endregion
    }
}