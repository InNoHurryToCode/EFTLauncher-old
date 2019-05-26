using System.IO;
using Newtonsoft.Json;

namespace EFTLauncher.Utility
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

        public static string SaveJson<T>(string file, T obj)
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.NullValueHandling = NullValueHandling.Ignore;

            using (StreamWriter sw = new StreamWriter(file))
            {
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    serializer.Serialize(sw, obj);
                }
            }

            return "";
        }

        public static string NormalizeJson<T>(T obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public static string EncodeTo64(string toEncode)
        {
            byte[] toEncodeAsBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(toEncode);
            return System.Convert.ToBase64String(toEncodeAsBytes);
        }
    }
}
