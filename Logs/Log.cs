using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WebsiteLinkChecker.Logs
{
    public class Log : ILog
    {
        private readonly bool _enableFullLogging;

        public Log(bool enableFullLogging)
        {
            _enableFullLogging = enableFullLogging;
        }

        public void LogPage(string url)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(url);
        }

        public void LogPass(string url)
        {
            if (_enableFullLogging)
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("PASS: " + url);
            }
        }

        public void LogFail(string url, WebException ex)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            var status = ex.Status;
            var code = (ex.Response as HttpWebResponse)?.StatusCode.ToString();
            Console.WriteLine("FAIL: " + url + " (" + status + " - " + code + ")");
        }
    }
}
