
namespace Schroders.Bus.NServiceBus.Tests
{
    using System;

    using global::NServiceBus;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    using Schroders.Bus.Core;
    using Schroders.Bus.Core.Contracts;

    [TestClass]
    public class BaseNServiceBusHandlerTests
    {
        [TestMethod]
        public void GivenMessageShouldCallAllHandlers()
        {
            var handlerProviderMock = new Mock<IBusHandlerProvider>();
            var busMock = new Mock<global::Schroders.Bus.Core.Contracts.IBus>();
            var messageHandlerContextMock = new Mock<IMessageHandlerContext>();

            var busHandlerMock1 = new Mock<IBusHandler>();
            var busHandlerMock2 = new Mock<IBusHandler>();

            busHandlerMock1.Setup(h => h.CanHandleMessage(It.IsAny<BusMessage>())).Returns(true);
            busHandlerMock2.Setup(h => h.CanHandleMessage(It.IsAny<BusMessage>())).Returns(true);
            handlerProviderMock.Setup(h => h.GetHandlers()).Returns(new[] { busHandlerMock1.Object, busHandlerMock2.Object });

            var handler = new BaseNServiceBusHandler<string>(
                handlerProviderMock.Object,
                busMock.Object,
                m => new BusMessage { Payload = m },
                bm => (string)bm.Payload);

            var task = handler.Handle("Test message", messageHandlerContextMock.Object);
            Assert.IsTrue(task.IsCompleted);

            busHandlerMock1.Verify(h => h.HandleMessage(It.IsAny<BusContext>(), It.Is<BusMessage>(bm => (string)bm.Payload == "Test message")), Times.Once());
            busHandlerMock2.Verify(h => h.HandleMessage(It.IsAny<BusContext>(), It.Is<BusMessage>(bm => (string)bm.Payload == "Test message")), Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GivenMessageIfHandlerFailsShouldThrow()
        {
            var handlerProviderMock = new Mock<IBusHandlerProvider>();
            var busMock = new Mock<global::Schroders.Bus.Core.Contracts.IBus>();
            var messageHandlerContextMock = new Mock<IMessageHandlerContext>();

            var busHandlerMock1 = new Mock<IBusHandler>();
            var busHandlerMock2 = new Mock<IBusHandler>();

            busHandlerMock1.Setup(h => h.CanHandleMessage(It.IsAny<BusMessage>())).Returns(true);
            busHandlerMock2.Setup(h => h.CanHandleMessage(It.IsAny<BusMessage>())).Returns(true);
            busHandlerMock2.Setup(h => h.HandleMessage(It.IsAny<BusContext>(), It.IsAny<BusMessage>())).Throws<ArgumentException>();
            handlerProviderMock.Setup(h => h.GetHandlers()).Returns(new[] { busHandlerMock1.Object, busHandlerMock2.Object });

            var handler = new BaseNServiceBusHandler<string>(
                handlerProviderMock.Object,
                busMock.Object,
                m => new BusMessage { Payload = m },
                bm => (string)bm.Payload);

            var task = handler.Handle("Test message", messageHandlerContextMock.Object);
            Assert.IsTrue(task.IsCompleted);

            busHandlerMock1.Verify(h => h.HandleMessage(It.IsAny<BusContext>(), It.Is<BusMessage>(bm => (string)bm.Payload == "Test message")), Times.Once());
        }
    }
}