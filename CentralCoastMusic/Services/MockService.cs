using CentralCoastMusic.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CentralCoastMusic.Services
{
    public static class MockService
    {
        public static List<Artist> LoadJson()
        {
            using (StreamReader r = new StreamReader("MOCK_DATA.json"))
            {
                string json = r.ReadToEnd();
                return JsonConvert.DeserializeObject<List<Artist>>(json);
            }
        }

    }
}
