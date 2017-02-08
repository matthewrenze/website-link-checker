using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;

namespace WebsiteLinkChecker.Anchors
{
    public class Anchor : IAnchor
    {
        private readonly HtmlNode _node;

        public Anchor(HtmlNode node)
        {
            _node = node;
        }

        public string GetHref()
        {
            return _node.Attributes["href"]?.Value;            
        }

        public string GetTarget()
        {
            return _node.Attributes["target"]?.Value;
        }
    }
}
