using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetroLive.Common
{
    public class JsonSeralizer
    {
        public static async Task<T> DeserializeObject<T>(string jsonText)
        {
            T retObj = await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<T>(jsonText));
            return retObj;
        }

        public static async Task<string> SerializeObject<T>(T obj)
        {
            string retText = await Task.Factory.StartNew(() => JsonConvert.SerializeObject(obj));
            return retText;
        }
    }
}
