using System;
using System.Collections.Generic;
using WebsiteLinkChecker.HtmlDocumentWrappers;
using WebsiteLinkChecker.WebClientWrappers;
using WebsiteLinkChecker.Websites;

namespace WebsiteLinkChecker.PageGetters
{
    public class PageGetter
    {
        private readonly IWebClientWrapper _client;
        private readonly IHtmlDocumentWrapper _document;
        private readonly IWebsite _website;
        private readonly List<string> _urls;

        public PageGetter(
            IWebClientWrapper client,
            IHtmlDocumentWrapper document,
            IWebsite website)
        {
            _client = client;
            _document = document;
            _website = website;
            _urls = new List<string>();
        }

        public List<string> GetPages(string url)
        {
            url = url.ToLower();

            if (_urls.Contains(url))
                return _urls;

            _urls.Add(url);

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

            return _urls;
        }
    }
}