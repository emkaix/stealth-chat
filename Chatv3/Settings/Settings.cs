using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Chatv3.JSON;
using Newtonsoft.Json;
using System.IO;

namespace Chatv3
{
    public static class Settings
    {
        public static IEnumerable<IPAddress> GetIpAddresses()
        {
            var jsonText = LoadSettingsFile();
            var buffer = JsonConvert.DeserializeObject<JsonHelper.RootObject>(jsonText).IPAddresses.ToList();
            return buffer.ConvertAll(x => IPAddress.Parse(x));
        }

        public static string GetUserName()
        {
            var jsonText = LoadSettingsFile();
            return JsonConvert.DeserializeObject<JsonHelper.RootObject>(jsonText).UserName;
        }

        private static string LoadSettingsFile()
        {
            return File.ReadAllText(Environment.CurrentDirectory + "\\settings.json");
        }
    }
}
