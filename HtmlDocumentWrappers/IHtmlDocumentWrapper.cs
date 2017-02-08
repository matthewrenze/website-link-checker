using System.Collections.Generic;
using WebsiteLinkChecker.Anchors;

namespace WebsiteLinkChecker.HtmlDocumentWrappers
{
    public interface IHtmlDocumentWrapper
    {
        IEnumerable<IAnchor> GetAnchors(string html);
    }
}