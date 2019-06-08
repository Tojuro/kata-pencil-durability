using System;
using Xunit;

namespace Kata.PencilService.Tests
{
    public class PencilTests
    {
        private const string TestText1 = "TestText1";
        private const string TestText2 = "TestText2";

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
            InlineData("abcdefghijklmno", "abcdefghij&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"),
            InlineData("ABCDE", "ABCDE"),
            InlineData("ABCDEFGHIJ", "ABCDE&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"),
            InlineData("AAAAaaaa", "AAAAaa&nbsp;&nbsp;"),
            InlineData("aaaaAAAA", "aaaaAAA&nbsp;"),
            InlineData("a test", "a&nbsp;test"),
            InlineData("i am ok", "i&nbsp;am&nbsp;ok"),
            InlineData("a a a   ", "a&nbsp;a&nbsp;a&nbsp;&nbsp;&nbsp;"),]
        public void WhenWritingToPaperOnlyBlankSpacesAreShownIfDurabilityIsLessOrEqualZero(string text, string result)
        {
            var durabilityStart = 10;
            var pencil = new Pencil(durabilityStart);
            var paper = new Paper();

            pencil.Write(text, ref paper);

            Assert.Equal(result, paper.Content);
        }

    }
}
