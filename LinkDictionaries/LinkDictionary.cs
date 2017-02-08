using System.Collections.Generic;

namespace WebsiteLinkChecker.LinkDictionaries
{
    public class LinkDictionary : ILinkDictionary
    {
        private List<string> _links;

        public LinkDictionary()
        {
            _links = new List<string>();
        }

        public bool HasLink(string url)
        {
            return _links.Contains(url);
        }

        public void AddLink(string url)
        {
            _links.Add(url);
        }
    }
}