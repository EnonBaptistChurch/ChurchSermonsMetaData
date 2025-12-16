using OllamaSharp;
using OllamaSharp.Models;
using SermonData.Interfaces;
using SermonData.Services;
using System.Diagnostics.Metrics;
using System.Security.Cryptography;
using System.Threading;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SermonData.Tests
{
    public class BiblePassageExtractorTests
    {
        private readonly IBiblePassageExtractor _extractor;

        public BiblePassageExtractorTests()
        {
            _extractor = new BiblePassageExtractor();
        }

        [Fact]
        public void ExtractSingleVerse_ShouldReturnCorrectPassage()
        {
            string text = "Proverbs 22:28";
            var result = _extractor.ExtractPassages(text);
            Assert.Single(result);
            Assert.Equal("Proverbs 22:28", result[0]);
        }

        [Fact]
        public void ExtractSingleVerse_Romans_ShouldReturnCorrectPassage()
        {
            string text = "Romans 3:23";
            var result = _extractor.ExtractPassages(text);
            Assert.Single(result);
            Assert.Equal("Romans 3:23", result[0]);
        }

        [Fact]
        public void ExtractChapterVerseKeywords_ShouldReturnCorrectPassage()
        {
            string text = "Proverbs chapter 22 verse 28";
            var result = _extractor.ExtractPassages(text);
            Assert.Single(result);
            Assert.Equal("Proverbs 22:28", result[0]);
        }

        [Fact]
        public void ExtractMultipleVersesUsingAnd_ShouldReturnCorrectRange()
        {
            string text = "Proverbs 10 and 11";
            var result = _extractor.ExtractPassages(text);
            Assert.Single(result);
            Assert.Equal("Proverbs 10-11", result[0]);
        }

        [Fact]
        public void ExtractColonNotation_ShouldReturnCorrectRange()
        {
            string text = "Romans 3:23,24";
            var result = _extractor.ExtractPassages(text);
            Assert.Single(result);
            Assert.Equal("Romans 3:23-24", result[0]);
        }

        [Fact]
        public void ExtractCrossChapterRange_ShouldReturnCorrectRange()
        {
            string text = "Proverbs 22:28 to 23:10";
            var result = _extractor.ExtractPassages(text);
            Assert.Single(result);
            Assert.Equal("Proverbs 22:28-23:10", result[0]);
        }

        [Fact]
        public void ExtractCrossChapterRangeWithDash_ShouldReturnCorrectRange()
        {
            string text = "Proverbs 22:28-23:10";
            var result = _extractor.ExtractPassages(text);
            Assert.Single(result);
            Assert.Equal("Proverbs 22:28-23:10", result[0]);
        }

        [Fact]
        public void ExtractMultipleReferencesInText_ShouldReturnAllPassages()
        {
            string text = "Today we read Proverbs 22:28 and Romans 3:23. Some also consider Proverbs chapter 22 verse 28 to chapter 23:10.";
            var result = _extractor.ExtractPassages(text);
            Assert.Equal(3, result.Count);
            Assert.Equal("Proverbs 22:28", result[0]);
            Assert.Equal("Romans 3:23", result[1]);
            Assert.Equal("Proverbs 22:28-23:10", result[2]);
        }

        [Fact]
        public void ExtractMultipleBooks_ShouldReturnAllPassages()
        {
            string text = "Look at Luke 2:1-7 and Hebrews 11:1,3,6 for examples.";
            var result = _extractor.ExtractPassages(text);
            Assert.Equal(2, result.Count);
            Assert.Equal("Luke 2:1-7", result[0]);
            Assert.Equal("Hebrews 11:1,3,6", result[1]);
        }

        [Fact]
        public void ExtractBookVariants_ShouldReturnAllPassages()
        {
            string text = "1 John 3:5 and 2nd Peter 1:21";
            var result = _extractor.ExtractPassages(text);
            Assert.Equal(2, result.Count);
            Assert.Equal("1 John 3:5", result[0]);
            Assert.Equal("2 Peter 1:21", result[1]);
        }


        [Fact]
        public void ExtractBibleRefs()
        {
            string text = "We're going to consider three verses in Proverbs. Proverbs chapter 22, verse 28, and  Proverbs chapter 23, verse 10, and 11." +
                " And type that by messages, \"New or old, which is better.\" Think about that initially. " +
                "But as we turn to God's Word, let's just pray together.  " +
                "Let's pray.Lord our God and our Father, we thank You that we can gather together freely.We thank You for the freedoms that we have.We look back with thanksgiving as we acknowledge  your hand and the preservation of those freedoms. And Lord, as we come now to Your Word, may that Word speak to us this morning.Lord, may that Word teach us, encourage us. Help us each we pray as we ask it for Christ's sake. Amen. So we're going to, look, I'm just  going to read those three verses first of all. So Proverbs 22, verse 28, we do not remove  the ancient landmark, which your fathers have set. Their Proverbs 23, which was read for us  verse 10 and 11, \"Do not remove the ancient landmark nor enter the fields of the fatherless.For there, a demon is mighty.He will plead their cause against you.\" Well, we've looked at Proverbs different themes and we're going to come to some different themes this morning.  And two of those verses, very similar there, different in a sense. We're going to notice  that in a moment it says, \"Do not remove the ancient landmark or boundary.\" Well, why shouldn't  we do that? But we're going to think about that in a moment. Now children, sometimes when people of my age or when we pull out something old and we appear, you think,  \"Well, they\'re ancient, they\'re old.\" Sometimes I get something out an old book and you can\'t  quite believe how old the book is. Because you think, \"Well, it's old, it's ancient.\"  There\'s a temptation, isn't there, to think, \"Well, what's new is better or always better?\"  Well, just because something is old doesn't mean to say it doesn't have value.  The old can be better than you may be better. But think about something,  \"Oh, think about antiques, people play a lot of money.\" Perhaps an old picture,  perhaps an old piece of furniture. They played a lot of money because old is considered to be  better. Or perhaps a piece of equipment, those who operate them will probably say,  \"Well, a lot of old equipment was more reliable.\"" +
                " It wasn't as complicated, but it did the job.The old is better, it tends to last longer.But you know, and all of us know, the new is tempting,  isn't it? They advertise things all the time. You\'re bombarded by a verse, whether it\'s on your phone  or what you watch. There\'s adverts all around. And why do the marketing companies,  sometimes they don\'t change a lot with a product, but they make it in a new wrapper or something  like that, because they want you to buy it, don\'t they? So they want to tempt you, they want you to tell you that new, well, that new gadget, you must have that, it\'s got new technology. And if you  can be persuaded like that, you\'re more likely to buy it. Now, I\'m not going to give you an answer,  which is better, old on you, because it depends on the context. And certain times, the new is better,  certain times the old is better. But it\'s, we need to think about it, don\'t we? We can be taken in  by what is new, and we can forget the values and the principles from the past. So the answer is,  it depends, you must look at the evidence in the Bible, Jesus, he told the parable of a wine skin,  he says there, the old is better. This was a reference to wine, an illustration of religion.  Well, I would suggest if you want to understand that, go and look up Luke chapter five and read  the context, because we always need to look at the verses of the Bible in the context, which they are.  But that\'s a part of a Bible, which says the old is better.But then when you turn to Hebrews,  you\'ll find out what we\'re told that the new covenant is a better covenant.So the new is better  in that situation. So we need to think about it, I\'m not going to tell you, as I said, which is better,  but we are going to think about these proverbs, because we mustn\'t forget the principles, the  truths, which God has given us. To some people, they seem very old. We don\'t need the Bible,  we don\'t need God. People used to think about him years ago, but God\'s word is truth. It is ever";
            var result = _extractor.ExtractPassages(text);
            //Assert.Equal(2, result.Count);
            //Assert.Equal("1 John 3:5", result[0]);
            //Assert.Equal("2 Peter 1:21", result[1]);
        }


        [Fact]
        public void ExtractPassages_WithNoScripture_ShouldReturnEmptyList()
        {
            // Arrange
            string text = "This text contains no Bible references at all.";

            // Act
            List<string> result = _extractor.ExtractPassages(text);

            // Assert
            Assert.Empty(result);
        }

        [Theory]
        // Test malformed or partial references
        [InlineData("Proverbs twenty two:28", new string[] { "Proverbs 22:28" })] 
        [InlineData("Romans 3:abc", new string[] { "Romans 3" })]          
        [InlineData("John 3", new[] { "John 3" })]
        public void ExtractPassages_ShouldHandleMalformedReferences(string text, string[] expected)
        {
            // Act
            List<string> result = _extractor.ExtractPassages(text);

            // Assert
            Assert.Equal(expected.Length, result.Count);
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.Equal(expected[i], result[i]);
            }
        }

    }
}
