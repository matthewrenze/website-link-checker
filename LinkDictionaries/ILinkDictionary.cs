namespace WebsiteLinkChecker.LinkDictionaries
{
    public interface ILinkDictionary
    {
        bool HasLink(string url);

        void AddLink(string url);
    }
}