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
        private readonly Helper _helper;

        public StreamService(DataService dataService,Helper helper)
        {
            _dataService = dataService;
            _helper = helper;
        }
        /// <summary>
        /// Get the streams for an artists id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Dictionary<string, Stream>> GetStreams(string id)
        {
            var response = await _dataService.ApiGoogle("GET", null, "Links/" + id, null);
            var streamList = _helper.Mapper<Dictionary<string, Stream>>(response);
            return streamList;
        }
        /// <summary>
        /// Get a single stream by its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Stream> GetSingleStream(string id)
        {
            var singleStream = new Stream();
            var response = await _dataService.ApiGoogle("GET", null, "Links", null);
            var streamList = _helper.Mapper<Dictionary<string, Dictionary<string, Stream>>>(response);
            var streams = streamList.Select(l => l.Value);
            foreach (var s in streams)
            {
                if (s.ContainsKey(id))
                {
                    singleStream = s[id];
                }
            }

            return singleStream;
        }
        /// <summary>
        /// Adds the stream
        /// </summary>
        /// <param name="streamRequest"></param>
        /// <returns></returns>
        public async Task<string> AddStream(StreamRequest streamRequest)
        {
            streamRequest.Stream.Id = Guid.NewGuid().ToString();
            var path = "Links/" + streamRequest.Auth["uid"] + "/" + streamRequest.Stream.Id;
            var json = JsonSerializer.Serialize(streamRequest.Stream);
            var response = await _dataService.ApiGoogle("PUT", json, path, streamRequest.Auth);

            var stream = _helper.Mapper<Stream>(response);

            return stream.Id;
        }
        /// <summary>
        /// Deletes the stream
        /// </summary>
        /// <param name="streamRequest"></param>
        /// <returns></returns>
        public async Task RemoveStream(StreamRequest streamRequest)
        {
            var path = "Links/" + streamRequest.Auth["uid"] + "/" + streamRequest.Stream.Id;
            var response = await _dataService.ApiGoogle("DELETE", null, path, streamRequest.Auth);

        }
        /// <summary>
        /// Adds the upcoming stream data to an artist model
        /// </summary>
        /// <param name="artists"></param>
        /// <returns></returns>
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
                        artist.LiveStreamId = nextLivestream.Id;
                    }

                }
            }
            return artists;
        }
    }
}
