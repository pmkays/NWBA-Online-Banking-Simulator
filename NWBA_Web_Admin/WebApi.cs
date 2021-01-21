using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace NWBA_Web_Admin
{
    public class WebApi
    {
        //private const string ApiBaseUri = "https://localhost:44343";
        //private const string ApiBaseUri = "https://localhost:5001";
        //private const string ApiBaseUri = "http://nwba-api";

        public static HttpClient InitializeClient()
        {
            var conn = Startup.StaticConfig.GetConnectionString("APIConnectionString");
            var client = new HttpClient { BaseAddress = new Uri(conn) };
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return client;
        }
    }
}
