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
        private readonly TagService _tagService;
        private readonly Helper _helper;

        public ArtistService(DataService dataService, TagService tagService, Helper helper)
        {
            _dataService = dataService;
            _tagService = tagService;
            _helper = helper;
        }

        public async Task<List<string>> GetGenres()
        {
            var genreResponse = await _dataService.ApiGoogle("GET", null, "Genres", null);
            var genres = _helper.Mapper<List<string>>(genreResponse);
            return genres;

        }

        public async Task<Artist> GetArtist(string id = null)
        {
            var artistResponse = await _dataService.ApiGoogle("GET", null, "Artists/" + id, null);
            var artist = _helper.Mapper<Artist>(artistResponse);
            return artist;
        }

        public async Task<List<Artist>> GetArtists()
        {
            var artistResponse = await _dataService.ApiGoogle("GET", null, "Artists", null);
            var artists = _helper.Mapper<Dictionary<string, Artist>>(artistResponse);

            var artistList = artists.Select(a => a.Value).ToList();            

            return artistList;

            //return MockService.LoadJson();
        }

        public async Task<List<Artist>> SearchArtists(string searchText)
        {
            var filteredArtists = new List<Artist>();
            var artists = await GetArtists();
            if (searchText == null)
            {
                return artists;
            }
            else
            {
                foreach (var artist in artists)
                {
                    if (artist.Name.ToLower().Contains(searchText.ToLower()))
                    {
                        filteredArtists.Add(artist);
                    }
                }

                return filteredArtists;
            }
        }

        public async Task<List<Artist>> TagSearch(string searchText)
        {
            var filteredArtists = new List<Artist>();
            var artists = await GetArtists();
            if (searchText == null)
            {
                return artists;
            }
            else
            {
                foreach (var artist in artists)
                {
                    var tags = await _tagService.GetTags(artist.Id);
                    if (tags!=null)
                    {
                        var tagList = tags.Select(t => t.Value.Name).ToList();
                        if (tagList.Contains(searchText.ToLower()))
                        {
                            filteredArtists.Add(artist);
                        }
                    }
                }

                return filteredArtists;
            }
        }



        public async Task AddArtist(ArtistRequest artistRequest)
        {
            var json = JsonSerializer.Serialize(artistRequest.Artist);
            var artistResponse = await _dataService.ApiGoogle("PUT", json, "Artists/" + artistRequest.Auth["uid"], artistRequest.Auth);    
        }

        public async Task EditArtist(ArtistRequest artistRequest)
        {
            var json = JsonSerializer.Serialize(artistRequest.Artist);
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
