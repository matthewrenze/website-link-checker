using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using AutoMoq;
using Moq;
using NUnit.Framework;
using WebsiteLinkChecker.Anchors;
using WebsiteLinkChecker.HtmlDocumentWrappers;
using WebsiteLinkChecker.LinkDictionaries;
using WebsiteLinkChecker.Logs;
using WebsiteLinkChecker.WebClientWrappers;
using WebsiteLinkChecker.Websites;

namespace WebsiteLinkChecker.PageCheckers
{
    [TestFixture]
    public class PageCheckerTests
    {
        private PageChecker _checker;
        private AutoMoqer _mocker;
        private IEnumerable<IAnchor> _anchors;
        private Mock<IAnchor> _mockAnchor;

        private const string Url = "http://www.test.com";
        private const string ChildUrl = "http://www.test.com/child";
        private const string RelativeUrl = "/child";
        private const string Html = "</html>";

        [SetUp]
        public void SetUp()
        {
            _mocker = new AutoMoqer();

            _mockAnchor = new Mock<IAnchor>();

            _mockAnchor.Setup(p => p.GetHref())
                .Returns(ChildUrl);

            _anchors = new List<IAnchor>()
            {
                _mockAnchor.Object
            };

            _mocker.GetMock<ILinkDictionary>()
                .Setup(p => p.HasLink(Url))
                .Returns(false);

            _mocker.GetMock<IWebClientWrapper>()
                .Setup(p => p.DownloadString(Url))
                .Returns(Html);

            _mocker.GetMock<IHtmlDocumentWrapper>()
                .Setup(p => p.GetAnchors(Html))
                .Returns(_anchors);

            _mocker.GetMock<IWebsite>()
                .Setup(p => p.IsMailToUrl(ChildUrl))
                .Returns(false);

            _mocker.GetMock<IWebsite>()
                .Setup(p => p.IsSiteRelativeUrl(ChildUrl))
                .Returns(false);

            _mocker.GetMock<IWebsite>()
                .Setup(p => p.IsPageRelativeUrl(ChildUrl))
                .Returns(false);

            _mocker.GetMock<IWebClientWrapper>()
                .Setup(p => p.DownloadString(ChildUrl))
                .Returns(Html);
            
            _checker = _mocker.Create<PageChecker>();
        }

        [Test]
        public void TestCheckShouldLogPage()
        {
            _checker.Check(Url);

            _mocker.GetMock<ILog>()
                .Verify(p => p.LogPage(Url),
                    Times.Once);
        }

        [Test]
        public void TestCheckShouldSkipMailToUrl()
        {
            _mocker.GetMock<IWebsite>()
                .Setup(p => p.IsMailToUrl(ChildUrl))
                .Returns(true);

            _checker.Check(Url);

            _mocker.GetMock<ILog>()
                .Verify(p => p.LogPass(ChildUrl),
                    Times.Never);
        }

        [Test]
        public void TestCheckShouldConvertSiteRelativeUrl()
        {
            _mockAnchor
                .Setup(p => p.GetHref())
                .Returns(RelativeUrl);

            _mocker.GetMock<IWebsite>()
                .Setup(p => p.IsSiteRelativeUrl(RelativeUrl))
                .Returns(true);

            _mocker.GetMock<IWebsite>()
                .Setup(p => p.GetAbsoluteFromSiteRelativeUrl(RelativeUrl))
                .Returns(ChildUrl);

            _checker.Check(Url);

            _mocker.GetMock<ILog>()
                .Verify(p => p.LogPass(ChildUrl),
                    Times.Once);
        }

        [Test]
        public void TestCheckShouldConvertPageRelativeUrl()
        {
            _mockAnchor
                .Setup(p => p.GetHref())
                .Returns(RelativeUrl);

            _mocker.GetMock<IWebsite>()
                .Setup(p => p.IsPageRelativeUrl(RelativeUrl))
                .Returns(true);

            _mocker.GetMock<IWebsite>()
                .Setup(p => p.GetAbsoluteFromPageRelativeUrl(Url, RelativeUrl))
                .Returns(ChildUrl);

            _checker.Check(Url);

            _mocker.GetMock<ILog>()
                .Verify(p => p.LogPass(ChildUrl),
                    Times.Once);
        }

        [Test]
        public void TestCheckShouldLogDuplicateAsPassWithoutRedownloading()
        {
            _mocker.GetMock<ILinkDictionary>()
                .Setup(p => p.HasLink(ChildUrl))
                .Returns(true);

            _checker.Check(Url);

            _mocker.GetMock<IWebClientWrapper>()
                .Verify(p => p.DownloadString(ChildUrl),
                    Times.Never);

            _mocker.GetMock<ILog>()
                .Verify(p => p.LogPass(ChildUrl),
                    Times.Once);
        }

        [Test]
        public void TestCheckShouldAddPassingLogToDictionary()
        {
            _checker.Check(Url);

            _mocker.GetMock<ILinkDictionary>()
                .Verify(p => p.AddLink(ChildUrl),
                    Times.Once);
        }

        [Test]
        public void TestCheckShouldLogChildUrlAsPass()
        {
            _checker.Check(Url);

            _mocker.GetMock<ILog>()
                .Verify(p => p.LogPass(ChildUrl),
                    Times.Once);
        }
        
        [Test]
        public void TestCheckShouldLogWebException()
        {
            var ex = new WebException();

            _mocker.GetMock<IWebClientWrapper>()
                .Setup(p => p.DownloadString(ChildUrl))
                .Throws(ex);

            _checker.Check(Url);

            _mocker.GetMock<ILog>()
                .Verify(p => p.LogFail(ChildUrl, ex),
                    Times.Once);
        }
    }
}
