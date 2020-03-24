using IthWeb.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace IthWeb.Tests
{
    public class DummyServiceTests
    {
        [Theory]
        [InlineData(1, 2)]
        [InlineData(0.5, 1.5)]
        [InlineData(-10, 0.12345)]
        public void Add_ShouldReturnSum<T>(T a, T b)
        {
            var sut = DummyService.Add(a, b);   // sut = System Under Test

            var expected = (dynamic)a + b;

            Assert.Equal(expected, sut);
        }

        [Fact]
        public void CalculateFullName_ShouldReturnFullName()
        {
            var sut = DummyService.CalculateFullName("Marcus", "Olkerud");

            Assert.Equal("Marcus Olkerud", sut);
        }
    }
}
