using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WebsiteLinkChecker.HtmlDocumentWrappers;
using WebsiteLinkChecker.LinkDictionaries;
using WebsiteLinkChecker.Logs;
using WebsiteLinkChecker.PageCheckers;
using WebsiteLinkChecker.PageGetters;
using WebsiteLinkChecker.WebClientWrappers;
using WebsiteLinkChecker.Websites;

namespace WebsiteLinkChecker
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
                ShowHelp();

            if (args.Length > 0 
                && (args[0] == "-h"  
                    || args[0] == "--help"))
                ShowHelp();

            var url = args[0];

            var enableFullLogging = args.Length > 1 
                && (args[1] == "-f" 
                    || args[1] == "--full");

            var getter = new PageGetter(
                new WebClientWrapper(), 
                new HtmlDocumentWrapper(),
                new Website(url));

            var pages = getter.GetPages(url);

            pages.Sort();

            var checker = new PageChecker(
                new LinkDictionary(), 
                new WebClientWrapper(), 
                new HtmlDocumentWrapper(),
                new Log(enableFullLogging), 
                new Website(url));

            foreach (var page in pages)
                checker.Check(page);

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        private static void ShowHelp()
        {
            Console.WriteLine("Checks a website for broken links");
            Console.WriteLine();
            Console.WriteLine("Usage: websitelinkchecker.exe [url] [-f]");
            Console.WriteLine();
            Console.WriteLine(" url - the URL for the website to be checked");
            Console.WriteLine(" -f  - enables full logging");
            Console.WriteLine("");
            Console.WriteLine("Example: websitelinkchecker.exe http://www.matthewrenze.com -f");
            Console.WriteLine();
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
            Environment.Exit(0);

        }
    }
}
