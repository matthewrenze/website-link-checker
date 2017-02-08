using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using NUnit.Framework;

namespace WebsiteLinkChecker.Anchors
{
    [TestFixture]
    public class AnchorTests
    {
        private Anchor _anchor;
        private HtmlNode _node;
        
        private const string Href = "http://www.url.com";
        private const string Target = "_blank";
        private const string InnerText = "Text";
        private const string EmptyAnchorText = "<a></a>";
        private readonly string FullAnchorText = $"<a href=\"{Href}\" target=\"{Target}\">{InnerText}</a>";

        public void SetUpAnchor(string anchorText)
        {
            _node = HtmlNode.CreateNode(anchorText);

            _anchor = new Anchor(_node);
        }

        [Test]
        public void TestGetHrefShouldReturnHref()
        {
            SetUpAnchor(FullAnchorText);

            var result = _anchor.GetHref();

            Assert.That(result,
                Is.EqualTo(Href));
        }

        [Test]
        public void TestGetEmptyHrefShouldReturnNull()
        {
            SetUpAnchor(EmptyAnchorText);

            var result = _anchor.GetHref();

            Assert.That(result, Is.Null);
        }

        [Test]
        public void TestGetTargetShouldReturnTarget()
        {
            SetUpAnchor(FullAnchorText);

            var result = _anchor.GetTarget();

            Assert.That(result,
                Is.EqualTo(Target));
        }

        [Test]
        public void TestGetEmptyTargetShouldReturnNull()
        {
            SetUpAnchor(EmptyAnchorText);

            var result = _anchor.GetTarget();

            Assert.That(result, Is.Null);
        }
    }
}
