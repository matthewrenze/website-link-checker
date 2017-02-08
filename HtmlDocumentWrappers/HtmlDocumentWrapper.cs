using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using WebsiteLinkChecker.Anchors;

namespace WebsiteLinkChecker.HtmlDocumentWrappers
{
    public class HtmlDocumentWrapper : IHtmlDocumentWrapper
    {
        public IEnumerable<IAnchor> GetAnchors(string html)
        {
            var document = new HtmlDocument();

            document.LoadHtml(html);

            var nodes = document.DocumentNode.Descendants("a");

            var anchors = nodes
                .Select(p => new Anchor(p));

            return anchors;
        }
    }
}
