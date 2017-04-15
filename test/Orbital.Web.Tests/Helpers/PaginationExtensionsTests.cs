using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Moq;
using Orbital.Web.Helpers;
using Xunit;

// ReSharper disable InvokeAsExtensionMethod - testing an exension method

namespace Orbital.Web.Tests.Helpers
{
    public class PaginationExtensionsTests
    {
        [Fact]
        public void TestGetPage_NoQueryParam()
        {
            var context = new ControllerContext { HttpContext = new DefaultHttpContext() };
            var controller = Mock.Of<Controller>(x => x.ControllerContext == context);

            var result = PaginationExtensions.GetPage(controller);

            Assert.Equal(1, result);
        }

        [Fact]
        public void TestGetPage_InvalidQueryParam()
        {
            var context = new ControllerContext { HttpContext = new DefaultHttpContext() };
            context.HttpContext.Request.Query = new QueryCollection(new Dictionary<string, StringValues> { { "page", new StringValues("abc") } });
            var controller = Mock.Of<Controller>(x => x.ControllerContext == context);

            var result = PaginationExtensions.GetPage(controller);

            Assert.Equal(1, result);
        }

        [Fact]
        public void TestGetPage_MulipleQueryParam()
        {
            var context = new ControllerContext { HttpContext = new DefaultHttpContext() };
            context.HttpContext.Request.Query = new QueryCollection(new Dictionary<string, StringValues> { { "page", new StringValues(new[] { "123", "456" }) } });
            var controller = Mock.Of<Controller>(x => x.ControllerContext == context);

            var result = PaginationExtensions.GetPage(controller);

            Assert.Equal(1, result);
        }

        [Fact]
        public void TestGetPage_ValidQueryParam()
        {
            var context = new ControllerContext { HttpContext = new DefaultHttpContext() };
            context.HttpContext.Request.Query = new QueryCollection(new Dictionary<string, StringValues> { { "page", new StringValues("123") } });
            var controller = Mock.Of<Controller>(x => x.ControllerContext == context);

            var result = PaginationExtensions.GetPage(controller);

            Assert.Equal(123, result);
        }
    }
}
