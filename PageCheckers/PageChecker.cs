using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using WebsiteLinkChecker.HtmlDocumentWrappers;
using WebsiteLinkChecker.LinkDictionaries;
using WebsiteLinkChecker.Logs;
using WebsiteLinkChecker.WebClientWrappers;
using WebsiteLinkChecker.Websites;

namespace WebsiteLinkChecker.PageCheckers
{
    public class PageChecker : IPageChecker
    {
        private readonly ILinkDictionary _dictionary;
        private readonly IWebClientWrapper _client;
        private readonly IHtmlDocumentWrapper _document;        
        private readonly ILog _logger;
        private readonly IWebsite _website;

        public PageChecker(
            ILinkDictionary dictionary,
            IWebClientWrapper client,
            IHtmlDocumentWrapper document,
            ILog logger,
            IWebsite website)
        {
            _dictionary = dictionary;
            _client = client;
            _document = document;            
            _logger = logger;
            _website = website;
        }

        public void Check(string url)
        {
            _logger.LogPage(url);

            var html = _client.DownloadString(url);

            var anchors = _document.GetAnchors(html);

            foreach (var anchor in anchors)
            {
                var childUrl = anchor.GetHref();

                if (_website.IsMailToUrl(childUrl))
                    continue;

                if (_website.IsSiteRelativeUrl(childUrl))
                    childUrl = _website.GetAbsoluteFromSiteRelativeUrl(childUrl);

                if (_website.IsPageRelativeUrl(childUrl))
                    childUrl = _website.GetAbsoluteFromPageRelativeUrl(url, childUrl);

                if (_dictionary.HasLink(childUrl))
                {
                    _logger.LogPass(childUrl);

                    continue;
                }

                try
                {
                    _client.DownloadString(childUrl);

                    _dictionary.AddLink(childUrl);

                    _logger.LogPass(childUrl);
                }
                catch (WebException ex)
                {
                    _logger.LogFail(childUrl, ex);
                }
            }
        }
    }
}
