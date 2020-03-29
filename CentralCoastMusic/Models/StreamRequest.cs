using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CentralCoastMusic.Models
{
    public class StreamRequest
    {
        public Dictionary<string, string> Auth { get; set; }
        public Stream Stream { get; set; }
    }
}
