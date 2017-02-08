using System;
using System.Collections.Generic;
using System.Linq;
using AutoMoq;
using Moq;
using NUnit.Framework;
using WebsiteLinkChecker.Anchors;
using WebsiteLinkChecker.HtmlDocumentWrappers;
using WebsiteLinkChecker.WebClientWrappers;
using WebsiteLinkChecker.Websites;

namespace WebsiteLinkChecker.PageGetters
{
    [TestFixture]
    public class PageGetterTests
    {
        private PageGetter _getter;
        private AutoMoqer _mocker;
        private List<IAnchor> _anchors;
        private Mock<IAnchor> _mockAnchor;

        private const string Url = "http://www.test.com";
        private const string ChildUrl = "http://www.test.com/child";        
        private const string Html = "</html>";

        [SetUp]
        public void SetUp()
        {
            _mockAnchor = new Mock<IAnchor>();

            _mockAnchor.Setup(p => p.GetHref())
                .Returns(ChildUrl);

            _anchors = new List<IAnchor>
            {
                _mockAnchor.Object
            };

            _mocker = new AutoMoqer();

            _mocker.GetMock<IWebClientWrapper>()
                .Setup(p => p.DownloadString(Url))
                .Returns(Html);

            _mocker.GetMock<IHtmlDocumentWrapper>()
                .Setup(p => p.GetAnchors(Html))
                .Returns(_anchors);

            _mocker.GetMock<IWebsite>()
                .Setup(p => p.IsExternalUrl(ChildUrl))
                .Returns(false);

            _mocker.GetMock<IWebsite>()
                .Setup(p => p.IsMailToUrl(ChildUrl))
                .Returns(false);

            _mocker.GetMock<IWebsite>()
                .Setup(p => p.IsHtmlUrl(ChildUrl))
                .Returns(true);

            _mocker.GetMock<IWebsite>()
                .Setup(p => p.IsSiteRelativeUrl(ChildUrl))
                .Returns(false);

            _getter = _mocker.Create<PageGetter>();
        }

        [Test]
        public void TestGetPagesShouldReturnRoot()
        {
            var results = _getter.GetPages(Url);

            Assert.That(results, 
                Contains.Item(Url));
        }

        [Test]
        public void TestGetPagesShouldReturnChildPage()
        {
            var results = _getter.GetPages(Url);

            Assert.That(results,
                Contains.Item(ChildUrl));
        }

        [Test]
        public void TestGetPagesShouldNotAddPageTwice()
        {
            _anchors.Add(_mockAnchor.Object);

            var results = _getter.GetPages(Url);

            var count = results.Count(p => p == ChildUrl);

            Assert.That(count, 
                Is.EqualTo(1));
        }

        [Test]
        public void TestGetPagesShouldNotAddMailToUrl()
        {
            _mocker.GetMock<IWebsite>()
                .Setup(p => p.IsMailToUrl(ChildUrl))
                .Returns(true);
        }

        [Test]
        public void TestGetPagesShouldNotAddExternalUrl()
        {
            _mocker.GetMock<IWebsite>()
                .Setup(p => p.IsExternalUrl(ChildUrl))
                .Returns(true);

            var result = _getter.GetPages(Url);

            Assert.That(result, 
                !Contains.Item(ChildUrl));
        }

        [Test]
        public void TestGetPagesShouldNotAddNonHtmlUrl()
        {
            _mocker.GetMock<IWebsite>()
                .Setup(p => p.IsHtmlUrl(ChildUrl))
                .Returns(false);

            var result = _getter.GetPages(Url);

            Assert.That(result,
                !Contains.Item(ChildUrl));
        }

        [Test]
        public void TestGetPagesShouldResolveSiteRelativeUrlToAbsoluteUrl()
        {
            _mocker.GetMock<IWebsite>()
                .Setup(p => p.IsSiteRelativeUrl(ChildUrl))
                .Returns(true);

            _mocker.GetMock<IWebsite>()
                .Setup(p => p.GetAbsoluteFromSiteRelativeUrl(ChildUrl))
                .Returns(ChildUrl);

            var results = _getter.GetPages(Url);

            Assert.That(results, 
                Contains.Item(ChildUrl));
        }

        [Test]
        public void TestGetPagesShouldResolvePageRelativeUrlToAbsoluteUrl()
        {
            _mocker.GetMock<IWebsite>()
                .Setup(p => p.IsPageRelativeUrl(ChildUrl))
                .Returns(true);

            _mocker.GetMock<IWebsite>()
                .Setup(p => p.GetAbsoluteFromPageRelativeUrl(Url, ChildUrl))
                .Returns(ChildUrl);

            var results = _getter.GetPages(Url);

            Assert.That(results,
                Contains.Item(ChildUrl));
        }
    }
}
