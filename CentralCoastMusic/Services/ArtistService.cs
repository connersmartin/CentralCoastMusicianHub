using CentralCoastMusic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace CentralCoastMusic.Services
{
    public class ArtistService
    {
        private readonly DataService _dataService;
        private readonly Helper _helper;

        public ArtistService(DataService dataService, Helper helper)
        {
            _dataService = dataService;
            _helper = helper;
        }

        public async Task<List<string>> GetGenres()
        {
            var genreResponse = await _dataService.ApiGoogle("GET", null, "Genres", null);
            var genres = _helper.Mapper<List<string>>(genreResponse);
            return genres;

        }

        public async Task<List<Artist>> GetArtists(string id = null)
        {
            string artistId = null;
            if (id!=null)
            {
                artistId = "/" + id;
            }
            var artistResponse = await _dataService.ApiGoogle("GET", null, "Artists"+artistId, null);
            var artists = _helper.Mapper<List<Artist>>(artistResponse);
            return artists;

        }

        public async Task AddArtist(ArtistRequest artistRequest)
        {
            var json = JsonSerializer.Serialize(artistRequest);
            var artistResponse = await _dataService.ApiGoogle("PUT", json, "Artists", artistRequest.Auth);

           
        }

        public async Task EditArtist(ArtistRequest artistRequest)
        {
            var json = JsonSerializer.Serialize(artistRequest);
            var artistResponse = await _dataService.ApiGoogle("PATCH", json, "Artists/"+artistRequest.Auth["uid"], artistRequest.Auth);
        }
        /*
        public async Task<List<Artist>> DeleteArtist(ArtistRequest artistRequest)
        {
            throw new NotImplementedException();
        }
        */
    }
}
