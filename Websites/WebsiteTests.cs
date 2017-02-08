using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace WebsiteLinkChecker.Websites
{
    [TestFixture]
    public class WebsiteTests
    {
        private Website _website;

        private const string Url = "http://www.test.com/";
        private const string Host = "www.test.com";
        private const string InternalUrl = "http://www.test.com/child";
        private const string ExternalUrl = "http://www.other.com";
        private const string SiteRelativeUrl = "/child";
        private const string PageRelativeUrl = "child";
        private const string MailToUrl = "mailto:email@test.com";

        [SetUp]
        public void SetUp()
        {
            _website = new Website(Url);
        }

        [Test]
        public void TestGetUrlShouldReturnUrl()
        {
            var result = _website.GetUrl();

            Assert.That(result, 
                Is.EqualTo(Url));
        }

        [Test]
        public void TestGetHostShouldReturnHost()
        {
            var result = _website.GetHost();

            Assert.That(result, 
                Is.EqualTo(Host));
        }

        [Test]
        [TestCase(InternalUrl, false)]
        [TestCase(MailToUrl, true)]
        public void TestIsMailToUrlShouldReturnCorrectValue(string url, bool expected)
        {
            var result = _website.IsMailToUrl(url);

            Assert.That(result,
                Is.EqualTo(expected));
        }

        [Test]
        [TestCase("test.html", true)]
        [TestCase("test.htm", true)]
        [TestCase("test/", true)]
        [TestCase("test.zip", false)]
        [TestCase("test", false)]
        public void TestIsHtmlUrlShouldReturnCorrectValue(string url, bool expected)
        {
            var result = _website.IsHtmlUrl(url);

            Assert.That(result, 
                Is.EqualTo(expected));
        }

        [Test]
        [TestCase(ExternalUrl, true)]
        [TestCase(InternalUrl, false)]
        public void TestIsExternalLinkShouldReturnCorrectValue(string url, bool expected)
        {
            var result = _website.IsExternalUrl(url);

            Assert.That(result, 
                Is.EqualTo(expected));
        }

        [Test]
        [TestCase(Url, false)]
        [TestCase(PageRelativeUrl, false)]
        [TestCase(SiteRelativeUrl, true)]
        public void TestIsSiteRelativeUrlShouldReturnCorrectValue(string url, bool expected)
        {
            var result = _website.IsSiteRelativeUrl(url);

            Assert.That(result,
                Is.EqualTo(expected));
        }

        [Test]
        [TestCase(Url, false)]
        [TestCase(SiteRelativeUrl,false)]
        [TestCase(PageRelativeUrl, true)]
        public void TestIsPageRelativeUrlShouldReturnCorrectValue(string url, bool expected)
        {
            var result = _website.IsPageRelativeUrl(url);

            Assert.That(result,
                Is.EqualTo(expected));
        }

        [Test]
        public void TestGetAbsoluteFromSiteRelativeUrlShouldReturnAbsoluteUrl()
        {
            var result = _website.GetAbsoluteFromSiteRelativeUrl(SiteRelativeUrl);

            Assert.That(result,
                Is.EqualTo(InternalUrl));
        }

        [Test]
        public void TestGetAbsoluteFromPageRelativeUrlShouldReturnAbsoluteUrl()
        {
            var result = _website.GetAbsoluteFromPageRelativeUrl(Url, PageRelativeUrl);

            Assert.That(result,
                Is.EqualTo(InternalUrl));
        }
    }
}
