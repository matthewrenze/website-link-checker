using System;
using System.Net;
using System.Collections.Generic;
using WebsiteLinkChecker.HtmlDocumentWrappers;
using WebsiteLinkChecker.Logs;
using WebsiteLinkChecker.WebClientWrappers;
using WebsiteLinkChecker.Websites;

namespace WebsiteLinkChecker.PageGetters
{
    public class PageGetter
    {
        private readonly IWebClientWrapper _client;
        private readonly IHtmlDocumentWrapper _document;
        private readonly IWebsite _website;
        private readonly ILog _log;
        private readonly List<string> _urls;

        public PageGetter(
            IWebClientWrapper client,
            IHtmlDocumentWrapper document,
            IWebsite website,
            ILog log)
        {
            _client = client;
            _document = document;
            _website = website;
            _log = log;
            _urls = new List<string>();            
        }

        public List<string> GetPages(string url)
        {
            url = url.ToLower();

            if (_urls.Contains(url))
                return _urls;

            _urls.Add(url);

            try
            {
                var html = _client.DownloadString(url);

                var anchors = _document.GetAnchors(html);

                foreach (var anchor in anchors)
                {
                    var childUrl = anchor.GetHref();

                    if (_website.IsMailToUrl(childUrl))
                        continue;

                    if (_website.IsExternalUrl(childUrl))
                        continue;

                    if (!_website.IsHtmlUrl(childUrl))
                        continue;

                    if (_website.IsSiteRelativeUrl(childUrl))
                        childUrl = _website.GetAbsoluteFromSiteRelativeUrl(childUrl);

                    if (_website.IsPageRelativeUrl(childUrl))
                        childUrl = _website.GetAbsoluteFromPageRelativeUrl(url, childUrl);

                    GetPages(childUrl);
                }
            }
            catch (WebException ex)
            {
                _log.LogFail(url, ex);
            }
            
            return _urls;
        }
    }
}