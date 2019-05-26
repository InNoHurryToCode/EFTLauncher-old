using System.IO;
using Newtonsoft.Json;

namespace EFTServer
{
    public static class JsonHelper
    {
        public static T LoadJson<T>(string file)
        {
            T data;

            // read the json data
            using (StreamReader sr = new StreamReader(file))
            {
                string json = sr.ReadToEnd();
                data = JsonConvert.DeserializeObject<T>(json);
            }

            return data;
        }
    }
}
