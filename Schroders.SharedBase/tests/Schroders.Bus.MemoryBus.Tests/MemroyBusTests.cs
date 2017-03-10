namespace Schroders.Bus.MemoryBus.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    using Schroders.Bus.Core;
    using Schroders.Bus.Core.Contracts;

    [TestClass]
    public class MemroyBusTests
    {
        [TestMethod]
        public void GivenBus_WhenSendingMessage_ShouldBeHandled()
        {
            var mockHandler1 = new Mock<IBusHandler>();
            var mockHandler2 = new Mock<IBusHandler>();

            var providerMock = new Mock<IBusHandlerProvider>();
            providerMock.Setup(p => p.GetHandlers()).Returns(new[] { mockHandler1.Object, mockHandler2.Object });

            mockHandler1.Setup(h => h.CanHandleMessage(It.IsAny<BusMessage>())).Returns(true);
            mockHandler2.Setup(h => h.CanHandleMessage(It.IsAny<BusMessage>())).Returns(true);

            var bus = new MemoryBus(providerMock.Object);
            var busMessage = new BusMessage { TopicName = "test", Payload = new { prop1 = "test prop 1" } };
            bus.Send(busMessage);

            mockHandler1.Verify(h => h.HandleMessage(It.Is<BusContext>(bc => bc != null), It.Is<BusMessage>(bm => bm.TopicName == "test" && bm.Payload != null)), Times.Once());
            mockHandler2.Verify(h => h.HandleMessage(It.Is<BusContext>(bc => bc != null), It.Is<BusMessage>(bm => bm.TopicName == "test" && bm.Payload != null)), Times.Once());
        }

        [TestMethod]
        public void GivenBus_SubscribedPublishers_ShouldGetNotification()
        {
            var mockHandler1 = new Mock<IBusHandler>();
            var mockHandler2 = new Mock<IBusHandler>();
            var mockHandler3 = new Mock<IBusHandler>();

            var providerMock = new Mock<IBusHandlerProvider>();
            providerMock.Setup(p => p.GetHandlers()).Returns(new[] { mockHandler1.Object, mockHandler2.Object, mockHandler3.Object });

            var bus = new MemoryBus(providerMock.Object);
            bus.Subscribe("TestNotification", (context, message) => mockHandler1.Object.HandleMessage(context, message));
            bus.Subscribe("TestNotificationOther", (context, message) => mockHandler2.Object.HandleMessage(context, message));
            bus.Subscribe("TestNotification", (context, message) => mockHandler3.Object.HandleMessage(context, message));

            var busMessage = new BusEvent { Payload = new { prop1 = "test prop 1" } };
            bus.Publish("TestNotification", busMessage);

            mockHandler1.Verify(h => h.HandleMessage(It.Is<BusContext>(bc => bc != null), It.Is<BusMessage>(bm => bm.Payload != null)), Times.Once());
            mockHandler2.Verify(h => h.HandleMessage(It.Is<BusContext>(bc => bc != null), It.Is<BusMessage>(bm => bm.Payload != null)), Times.Never());
            mockHandler3.Verify(h => h.HandleMessage(It.Is<BusContext>(bc => bc != null), It.Is<BusMessage>(bm => bm.Payload != null)), Times.Once());
        }

        [TestMethod]
        public void GivenBus_SubscribedPublishers_ShouldNotGetNotificationAfterUnsubscribe()
        {
            var mockHandler1 = new Mock<IBusHandler>();
            var mockHandler2 = new Mock<IBusHandler>();

            var providerMock = new Mock<IBusHandlerProvider>();
            providerMock.Setup(p => p.GetHandlers()).Returns(new[] { mockHandler1.Object, mockHandler2.Object });

            mockHandler1.Setup(h => h.CanHandleMessage(It.IsAny<BusMessage>())).Returns(true);
            mockHandler2.Setup(h => h.CanHandleMessage(It.IsAny<BusMessage>())).Returns(true);

            var bus = new MemoryBus(providerMock.Object);
            var subscription = bus.Subscribe("TestNotification", (context, message) => mockHandler1.Object.HandleMessage(context, message));
            bus.Unsubscribe(subscription);

            var busMessage = new BusEvent { Payload = new { prop1 = "test prop 1" } };
            bus.Publish("TestNotification", busMessage);

            mockHandler1.Verify(h => h.HandleMessage(It.Is<BusContext>(bc => bc != null), It.Is<BusMessage>(bm => bm.Payload != null)), Times.Never());
            mockHandler2.Verify(h => h.HandleMessage(It.Is<BusContext>(bc => bc != null), It.Is<BusMessage>(bm => bm.Payload != null)), Times.Never());
        }
    }
}