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
        public List<string> Links { get; set; }
        public List<string> Genres { get; set; }

    }
}
