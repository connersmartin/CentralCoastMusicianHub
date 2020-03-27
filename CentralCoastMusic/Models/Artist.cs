using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CentralCoastMusic.Models
{
    public class Artist
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ImageLink { get; set; }
        public string Youtube { get; set; }
        public string Instagram { get; set; }
        public string Facebook { get; set; }
        public string Patreon { get; set; }
        public string Twitter { get; set; }
        public string Donation { get; set; }
        public string Livestream { get; set; }
        public DateTime? NextLivestream { get; set; }
        public bool Verified { get; set; }
        public string CustomUrl { get; set; }

        //public List<string> Genres { get; set; }

    }
}
