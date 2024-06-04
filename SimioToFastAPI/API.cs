using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace SimioToFastAPI
{
    internal class API
    {
        public string CallAPI(string path)
        {
            string ret;

            using (var client = new HttpClient())
            {
                var endpoint = new Uri(path);
                var result = client.GetAsync(endpoint).Result;
                var json = result.Content.ReadAsStringAsync().Result;

                ret = json;
            }

            return ret;
        }
    }
}
