using System;
using Xunit;

namespace Kata.PencilService.Tests
{
    public class PencilTests
    {
        [Fact]
        public void CanCreateAPencil()
        {
            var pencil = new Pencil();

            Assert.NotNull(pencil);
            Assert.IsType(typeof(Pencil), pencil);
        }
    }
}
