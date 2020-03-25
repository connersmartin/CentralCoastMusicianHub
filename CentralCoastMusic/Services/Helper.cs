using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace CentralCoastMusic.Services
{
    public class Helper
    {
        //General mapper
        public T Mapper<T>(byte[] json)
        {
            return JsonSerializer.Deserialize<T>(json, null);
        }
       
    }
}
