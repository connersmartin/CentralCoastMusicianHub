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

        public ArtistService(DataService dataService)
        {
            _dataService = dataService;
        }

        public async Task<List<string>> GetGenres()
        {
            throw new NotImplementedException();
        }

        public async Task<List<Artist>> GetArtists(string id = null)
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
