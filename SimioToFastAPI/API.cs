﻿using System;
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
        public string CallAPI(string path, string jsonRequest)
        {
            string ret;

            using (var client = new HttpClient())
            {
                var endpoint = new Uri(path);
                var content = new StringContent(jsonRequest, System.Text.Encoding.UTF8, "application/json");

                var result = client.PostAsync(endpoint, content).Result;

                var jsonResponse = result.Content.ReadAsStringAsync().Result;
                ret = jsonResponse;
            }

            return ret;
        }
    }
}
