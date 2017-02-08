using System.Net;

namespace WebsiteLinkChecker.Logs
{
    public interface ILog
    {
        void LogPage(string url);

        void LogPass(string url);

        void LogFail(string url, WebException ex);
    }
}