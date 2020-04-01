using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using CentralCoastMusic.Models;

namespace CentralCoastMusic.Services
{
    public class StreamService
    {
        private readonly DataService _dataService;

        public StreamService(DataService dataService)
        {
            _dataService = dataService;
        }
        public async Task<Dictionary<string, Stream>> GetStreams(string id)
        {
            var response = await _dataService.ApiGoogle("GET", null, "Links/" + id, null);
            var streamList = JsonSerializer.Deserialize<Dictionary<string, Stream>>(response);
            return streamList;
        }

        public async Task<string> AddStream(StreamRequest streamRequest)
        {
            streamRequest.Stream.Id = Guid.NewGuid().ToString();
            var path = "Links/" + streamRequest.Auth["uid"] + "/" + streamRequest.Stream.Id;
            var json = JsonSerializer.Serialize(streamRequest.Stream);
            var response = await _dataService.ApiGoogle("PUT", json, path, streamRequest.Auth);

            var stream = JsonSerializer.Deserialize<Stream>(response);

            return stream.Id;
        }

        public async Task RemoveStream(StreamRequest streamRequest)
        {
            var path = "Links/" + streamRequest.Auth["uid"] + "/" + streamRequest.Stream.Id;
            var response = await _dataService.ApiGoogle("DELETE", null, path, streamRequest.Auth);

        }

        public async Task<List<Artist>> AddUpcomingStreamToArtists(List<Artist> artists)
        {
            foreach (var artist in artists)
            {
                var streams = await GetStreams(artist.Id);
                if (streams != null)
                {
                    var streamList = streams.Select(l => l.Value).ToList();
                    var nextLivestream = streamList.OrderBy(s => s.StartTime).Where(s => s.EndTime > DateTime.Now).FirstOrDefault();
                    if (nextLivestream != null)
                    {
                        artist.Livestream = nextLivestream.Description;
                        artist.NextLivestream = nextLivestream.StartTime;

                    }

                }
            }
            return artists;
        }
    }
}
