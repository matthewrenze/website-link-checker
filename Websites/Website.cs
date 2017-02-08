using System;
using NUnit.Framework.Constraints;

namespace WebsiteLinkChecker.Websites
{
    public class Website : IWebsite
    {
        private readonly Uri _uri;

        public Website(string url)
        {
            _uri = new Uri(url, UriKind.Absolute);
        }

        public string GetUrl()
        {
            return _uri.ToString();
        }

        public string GetHost()
        {
            return _uri.Host;
        }

        public bool IsMailToUrl(string url)
        {
            return url.StartsWith("mailto");
        }

        public bool IsExternalUrl(string url)
        {
            return url.StartsWith("http")
                && !url.Contains(_uri.Host);
        }

        public bool IsSiteRelativeUrl(string url)
        {
            return url.StartsWith("/");
        }

        public bool IsPageRelativeUrl(string url)
        {
            return !url.StartsWith("http")
                && !url.StartsWith("/");
        }

        public string GetAbsoluteFromSiteRelativeUrl(string url)
        {
            var relativeUri = new Uri(url, UriKind.Relative);

            var uri = new Uri(_uri, relativeUri);

            return uri.ToString();
        }

        public bool IsHtmlUrl(string url)
        {
            return url.EndsWith(".html")
                   || url.EndsWith(".htm")
                   || url.EndsWith("/");
        }

        public string GetAbsoluteFromPageRelativeUrl(string pageUrl, string relativeUrl)
        {
            var pageUri = new Uri(pageUrl, UriKind.Absolute);

            var relativeUri = new Uri(relativeUrl, UriKind.Relative);

            var uri = new Uri(pageUri, relativeUri);

            return uri.ToString();
        }
    }
}