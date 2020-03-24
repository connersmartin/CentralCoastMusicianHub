using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CentralCoastMusic.Models
{
    public class Link
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string Attribute { get; set; }
        public Types Type { get; set; }

        public enum Types
        {
            Link=0,
            Text=1,
            YouTube=2,
            Instagram=3,
            Facebook=4,
            Venmo=5,
            PayPal=6,
            Patreon=7,
            Spotify=8,
            Image=9,
            Biography=10
        }
    }


}
