using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace WebsiteLinkChecker.LinkDictionaries
{
    [TestFixture]
    public class LinkDictionaryTests
    {
        private LinkDictionary _dictionary;

        public const string Url = "http://www.test.com";

        [SetUp]
        public void SetUp()
        {
            _dictionary = new LinkDictionary();            
        }

        [Test]
        public void TestHasLinkShouldReturnTrueIfLinkExists()
        {
            _dictionary.AddLink(Url);

            var result = _dictionary.HasLink(Url);

            Assert.That(result, Is.True);
        }

        [Test]
        public void TestHasLinkShouldReturnFalseIfLinkDoesNotExist()
        {
            var result = _dictionary.HasLink(Url);

            Assert.That(result, Is.False);
        }

        [Test]
        public void TestAddLinkShouldAddLink()
        {
            _dictionary.AddLink(Url);

            var result = _dictionary.HasLink(Url);

            Assert.That(result, Is.True);
        }
    }
}
