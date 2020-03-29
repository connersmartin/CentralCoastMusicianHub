using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CentralCoastMusic.Models
{
    public class TagRequest
    {
        public Dictionary<string,string> Auth { get; set; }
        public Tag Tag { get; set; }
    }
}
