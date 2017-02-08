using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace WebsiteLinkChecker.WebClientWrappers
{
    public class WebClientWrapper : IWebClientWrapper
    {
        public string DownloadString(string url)
        {
            var client = new WebClient();

            // NOTE: I disabled this because it causes issues with Amazon Associate URLs
            // NOTE: However, some websites (e.g. carlfranklin.net) complain if there is no User-Agent
            //client.Headers["User-Agent"] 
            //    = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) "
            //    + "AppleWebKit/537.36 " 
            //    + "(KHTML, like Gecko) "
            //    + "Chrome/55.0.2883.87 "
            //    + "Safari/537.36 "
            //    + ".NET CLR 4.6.01586";

            return client.DownloadString(url);
        }
    }
}
