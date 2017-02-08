namespace WebsiteLinkChecker.Websites
{
    public interface IWebsite
    {
        string GetUrl();

        string GetHost();

        bool IsSiteRelativeUrl(string url);

        bool IsPageRelativeUrl(string url);

        bool IsMailToUrl(string url);

        bool IsHtmlUrl(string url);

        bool IsExternalUrl(string url);

        string GetAbsoluteFromSiteRelativeUrl(string url);

        string GetAbsoluteFromPageRelativeUrl(string pageUrl, string relativeUrl);
    }
}