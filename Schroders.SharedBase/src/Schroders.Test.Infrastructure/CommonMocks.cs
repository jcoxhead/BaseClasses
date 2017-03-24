using Moq;
using Microsoft.Extensions.Logging;

namespace Schroders.Test.Infrastructure.Common
{
    public static class CommonMocks
    {
        public static Mock<ILoggerFactory> GetLoggerFactory()
        {
            var loggerMock = new Mock<ILogger>();
            var loggerFactoryMock = new Mock<ILoggerFactory>();
            loggerFactoryMock.Setup(x => x.CreateLogger(It.IsAny<string>()))
                .Returns(loggerMock.Object);

            return loggerFactoryMock;
        }
    }
}