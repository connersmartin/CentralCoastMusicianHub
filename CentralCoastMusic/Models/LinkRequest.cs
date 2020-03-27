using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CentralCoastMusic.Models
{
    public class LinkRequest
    {
        public Dictionary<string,string> Auth { get; set; }
        public Link Link { get; set; }
    }
}
