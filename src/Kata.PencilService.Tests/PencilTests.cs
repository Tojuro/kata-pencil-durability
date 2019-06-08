using System;
using Xunit;

namespace Kata.PencilService.Tests
{
    public class PencilTests
    {
        private const string TestText1 = "TestText1";
        private const string TestText2 = "TestText2";
        private const string TenPointString = "abcdefghij";
        private const string FivePointString = "abcde";
        private const string ErasingTestText = "How much wood would a woodchuck chuck if a woodchuck could chuck wood?";

        [Fact]
        public void CanCreateAPencil()
        {
            var pencil = new Pencil();

            Assert.NotNull(pencil);
            Assert.IsType(typeof(Pencil), pencil);
        }

        [Fact]
        public void CanCreatePencilAndSetDurability()
        {
            var pencil = new Pencil(100);

            Assert.Equal(100, pencil.Durability);
        }

        [Fact]
        public void CanCreatePencilAndSetLength()
        {
            var pencil = new Pencil(100, 5);

            Assert.Equal(5, pencil.Length);
        }

        [Theory, 
            InlineData(""),
            InlineData(TestText2),]
        public void WhenWritingToPaperTheContentIsAppended(string paperInitialContent)
        {
            var pencil = new Pencil();
            var paper = new Paper { Content = paperInitialContent };

            pencil.Write(TestText1, ref paper);
            
            Assert.Equal(string.Concat(paperInitialContent, TestText1), paper.Content);
        }

        [Theory,
            InlineData("o", 1),
            InlineData("O", 2),
            InlineData("abcde", 5),
            InlineData("ABCDE", 10),
            InlineData("ABCDEfghij", 15),
            InlineData("abcdefghijklmnopqrstuvwxyz", 26),
            InlineData("ABCDEFGHIJKLMNOPQRSTUVWXYZ", 52),
            InlineData("AzAzByBy", 12),]
        public void WhenWritingThePointDegrades(string text, int degradation)
        {
            var durabilityStart = 100;
            var pencil = new Pencil(durabilityStart);
            var paper = new Paper();

            pencil.Write(text, ref paper);

            Assert.Equal(durabilityStart-degradation, pencil.Durability);
        }

        [Theory,
            InlineData("o", "o"),
            InlineData("O", "O"),
            InlineData("abcdef", "abcdef"),
            InlineData("abcdefghij", "abcdefghij"),
            InlineData("abcdefghijklmno", "abcdefghij     "),
            InlineData("ABCDE", "ABCDE"),
            InlineData("ABCDEFGHIJ", "ABCDE     "),
            InlineData("AAAAaaaa", "AAAAaa  "),
            InlineData("aaaaAAAA", "aaaaAAA "),
            InlineData("a test", "a test"),
            InlineData("i am ok", "i am ok"),
            InlineData("a a a   ", "a a a   "),]
        public void WhenWritingToPaperOnlyBlankSpacesAreShownIfDurabilityIsLessOrEqualZero(string text, string result)
        {
            var durabilityStart = 10;
            var pencil = new Pencil(durabilityStart);
            var paper = new Paper();

            pencil.Write(text, ref paper);

            Assert.Equal(result, paper.Content);
        }

        [Fact]
        public void WhenSharpeningTheDurabilityIsReset()
        {
            var initialDurability = 10;
            var pencil = new Pencil(initialDurability);
            var paper = new Paper();

            pencil.Write(FivePointString, ref paper);
            pencil.Sharpen();

            Assert.Equal(initialDurability, pencil.Durability);
        }

        [Fact]
        public void WhenSharpeningTheLengthIsReduced()
        {
            var length = 5;
            var pencil = new Pencil(10, length);
            var paper = new Paper();

            pencil.Write(TenPointString, ref paper);
            pencil.Sharpen();

            Assert.Equal(length-1, pencil.Length);
        }

        [Fact]
        public void WhenSharpeningTheTotalDurabilityStopsSharpeningWhenExhausted()
        {
            var length = 0;
            var pencil = new Pencil(10, length);
            var paper = new Paper();

            pencil.Write(TenPointString, ref paper);
            pencil.Sharpen();

            Assert.Equal(0, pencil.Durability);
        }

        [Fact]
        public void WhenErasingTheLastWordIsRemoved()
        {
            var pencil = new Pencil();
            var paper = new Paper();

            pencil.Write(ErasingTestText, ref paper);
            pencil.Erase("chuck", ref paper);

            Assert.Equal(paper.Content, "How much wood would a woodchuck chuck if a woodchuck could       wood?");
        }

        [Fact]
        public void WhenErasingTwiceTheLastTwoWordsAreRemoved()
        {
            var pencil = new Pencil();
            var paper = new Paper();

            pencil.Write(ErasingTestText, ref paper);
            pencil.Erase("chuck", ref paper);
            pencil.Erase("chuck", ref paper);

            Assert.Equal(paper.Content, "How much wood would a woodchuck chuck if a wood      could       wood?");
        }

        [Fact]
        public void WhenErasingTheTextMightBeFirstWord()
        {
            var pencil = new Pencil();
            var paper = new Paper();

            pencil.Write(ErasingTestText, ref paper);
            pencil.Erase("How", ref paper);

            Assert.Equal(paper.Content, "    much wood would a woodchuck chuck if a woodchuck could chuck wood?");
        }

        [Fact]
        public void WhenErasingTheTextMightBeLastCharacter()
        {
            var pencil = new Pencil();
            var paper = new Paper();

            pencil.Write(ErasingTestText, ref paper);
            pencil.Erase("?", ref paper);

            Assert.Equal(paper.Content, "How much wood would a woodchuck chuck if a woodchuck could chuck wood ");
        }

        [Fact]
        public void WhenErasingTheTextMightNotBeThere()
        {
            var pencil = new Pencil();
            var paper = new Paper();

            pencil.Write(ErasingTestText, ref paper);
            pencil.Erase("buck", ref paper);

            Assert.Equal(paper.Content, ErasingTestText);
        }

    }
}
