namespace BoroServer.MvcFramework.Tests
{
    using System;
    using System.IO;
    using System.Collections.Generic;

    using Xunit;

    using BoroServer.MvcFramework.ViewEngine;

    public class BoroViewEngineTests
    {
        [Theory]
        // happy path
        // interesting cases
        // complex cases or combination of tests
        // code coverage 100%
        [InlineData("CleanHtml")]
        [InlineData("Foreach")]
        [InlineData("IfElseFor")]
        [InlineData("ViewModel")]
        [InlineData("test")]
        public void TestGetHtml(string fileName)
        {
            var viewModel = new TestViewModel
            {
                DateOfBirth = new DateTime(2019, 6, 1),
                Name = "Doggo Arghentino",
                Price = 12345.67M,
            };

            IViewEngine viewEngine = new BoroViewEngine();
            var view = File.ReadAllText($"ViewTests/{fileName}.html");
            var actualResult = viewEngine.GetHtml(view, viewModel, null);
            var expectedResult = File.ReadAllText($"ViewTests/{fileName}.Result.html");
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void TestTemplateViewModel()
        {
            IViewEngine viewEngine = new BoroViewEngine();
            var actualResult = viewEngine.GetHtml(@"@foreach(var num in Model)
{
<p>@num</p>
}", new List<int> { 1, 2, 3 }, null);
            var expectedResult = @"<p>1</p>
<p>2</p>
<p>3</p>";

            Assert.Equal(expectedResult, actualResult);
        }
    }
}
