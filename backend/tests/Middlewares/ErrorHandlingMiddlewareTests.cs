using NUnit.Framework;
using backend.src.Middlewares;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Text;

namespace backend.tests.Middlewares
{
    [TestFixture]
    public class ErrorHandlingMiddlewareTests
    {
        [Test]
        public async Task Invoke_DianteDeException_DeveEscreverRespostaComErro()
        {
            // Arrange: simulate a delegate that always throws exception
            RequestDelegate next = (HttpContext context) => throw new Exception("Test exception");

            var loggerMock = new Mock<ILogger<ErrorHandlingMiddleware>>();

            var middleware = new ErrorHandlingMiddleware(next, loggerMock.Object);

            var context = new DefaultHttpContext();
            var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            // Act
            await middleware.Invoke(context);

            // Assert
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(context.Response.Body);
            var responseText = await reader.ReadToEndAsync();

            Assert.AreEqual((int)HttpStatusCode.InternalServerError, context.Response.StatusCode);
            StringAssert.Contains("Ocorreu um erro interno", responseText);
        }
    }
}
