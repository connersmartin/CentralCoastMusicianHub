using CentralCoastMusic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
            return new List<string>();

        }

        public async Task<List<Artist>> GetArtists(string id = null)
        {
            var artistResponse = await _dataService.ApiGoogle("GET", null, "Artists", null);
            var artists = _helper.Mapper<List<Artist>>(artistResponse);
            return new List<Artist>();

        }

        public async Task<Artist> AddArtist(ArtistRequest artistRequest)
        {
            throw new NotImplementedException();
        }

        public async Task<Artist> EditArtist(ArtistRequest artistRequest)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Artist>> DeleteArtist(ArtistRequest artistRequest)
        {
            throw new NotImplementedException();
        }
    }
}
