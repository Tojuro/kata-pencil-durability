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
        public void WhenWriteToPaperTheContentIsAppended()
        {
            var pencil = new Pencil();
            var paper = new Paper();
            
            pencil.Write(TestText1, ref paper);
            
            Assert.Equal(TestText1, paper.Content);
        }
    }
}
