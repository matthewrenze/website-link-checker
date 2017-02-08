namespace WebsiteLinkChecker.WebClientWrappers
{
    public interface IWebClientWrapper
    {
        string DownloadString(string url);
    }
}