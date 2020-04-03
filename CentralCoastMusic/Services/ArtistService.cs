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

        //Not currently used
        public async Task<List<string>> GetGenres()
        {
            var genreResponse = await _dataService.ApiGoogle("GET", null, "Genres", null);
            var genres = _helper.Mapper<List<string>>(genreResponse);
            return genres;

        }
        /// <summary>
        /// Get single artist
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Artist> GetArtist(string id = null)
        {
            var artistResponse = await _dataService.ApiGoogle("GET", null, "Artists/" + id, null);
            var artist = _helper.Mapper<Artist>(artistResponse);
            return artist;
        }
        /// <summary>
        /// Get all artists
        /// </summary>
        /// <returns></returns>
        public async Task<List<Artist>> GetArtists()
        {
            //TODO paginate?
            var artistResponse = await _dataService.ApiGoogle("GET", null, "Artists", null);
            var artists = _helper.Mapper<Dictionary<string, Artist>>(artistResponse);

            var artistList = artists.Select(a => a.Value).ToList();            

            return artistList;

            //return MockService.LoadJson();
        }
        /// <summary>
        /// Search artists
        /// </summary>
        /// <param name="searchText"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Search via tags
        /// </summary>
        /// <param name="searchText"></param>
        /// <returns></returns>
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
                        foreach (var tag in tagList)
                        {
                            if (tag!=null)
                            {
                                if (tag.Contains(searchText.ToLower()))
                                {
                                    filteredArtists.Add(artist);
                                }
                            }
                        }
                    }
                }

                return filteredArtists;
            }
        }
        
        /// <summary>
        /// Adds the artist
        /// </summary>
        /// <param name="artistRequest"></param>
        /// <returns></returns>
        public async Task AddArtist(ArtistRequest artistRequest)
        {
            var json = JsonSerializer.Serialize(artistRequest.Artist);
            var artistResponse = await _dataService.ApiGoogle("PUT", json, "Artists/" + artistRequest.Auth["uid"], artistRequest.Auth);    
        }

        /// <summary>
        /// Edits the artist
        /// </summary>
        /// <param name="artistRequest"></param>
        /// <returns></returns>
        public async Task EditArtist(ArtistRequest artistRequest)
        {
            var json = JsonSerializer.Serialize(artistRequest.Artist);
            var artistResponse = await _dataService.ApiGoogle("PATCH", json, "Artists/"+artistRequest.Auth["uid"], artistRequest.Auth);
        }
        /*
        //Is this necessary?
        public async Task<List<Artist>> DeleteArtist(ArtistRequest artistRequest)
        {
            throw new NotImplementedException();
        }
        */
    }
}
