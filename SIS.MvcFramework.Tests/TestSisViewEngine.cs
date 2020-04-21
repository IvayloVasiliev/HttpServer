using SIS.MvcFramework.Tests;
using SIS.MvcFramework.ViewEngine;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace SIS.MvcFramework.Tests
{
    public class TestSisViewEngine
    {
        [Theory]
        [InlineData("TestWithOutCSharpCode")]
        [InlineData("UseForForeachAndIf")]
        [InlineData("UseModelData")]
        public void TestGetHtml(string testFileName)
        {
            IViewEngine viewEngine = new SisViewEngine();
            string viewFileName = "ViewTests/" + testFileName + ".html";
            string expectedResultFileName = "ViewTests/" + testFileName + ".Result.html";
            string viewContent = File.ReadAllText(viewFileName);
            string expectedResult = File.ReadAllText(expectedResultFileName);
            string actualResult = viewEngine.GetHtml(viewContent, (object)new TestViewModel
            {
                StringValue = "str",
                ListValues = new List<string>
            {
                "123",
                "val1",
                string.Empty
            }
            });
            Assert.Equal(expectedResult.TrimEnd(), actualResult.TrimEnd());
        }
    }
}
