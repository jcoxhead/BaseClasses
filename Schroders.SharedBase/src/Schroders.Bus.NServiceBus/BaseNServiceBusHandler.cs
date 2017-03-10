namespace Schroders.Bus.NServiceBus
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading.Tasks;

    using global::NServiceBus;

    using Schroders.Bus.Core;
    using Schroders.Bus.Core.Contracts;

    using IBus = Schroders.Bus.Core.Contracts.IBus;

    public class BaseNServiceBusHandler<TMessage> : IHandleMessages<TMessage>
    {
        private readonly IBusHandlerProvider busHandlerProvider;

        private readonly IBus bus;

        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "nService bus is a name")]
        private readonly Func<TMessage, BusMessage> nServiceBusToMessageFunc;

        private readonly Func<BusMessage, TMessage> messageToNServiceBusFunc;

        private readonly Action<Exception> exceptionAction;

        /// <summary>
        /// Creates generic NServiceBus handler with capability to transform generic message to concrete one and back so we keep independence 
        /// from NService bus
        /// </summary>
        /// <param name="busHandlerProvider">Message handler provider</param>
        /// <param name="bus">Generic Bus</param>
        /// <param name="nServiceBusToMessageFunc">Conversion function from NServiceBus type to generic message</param>
        /// <param name="messageToNServiceBusFunc">Conversion function from generic message to concrete NServiceBus type</param>
        /// <param name="exceptionAction">Function that is called if handler exception happens, by default simply re-throws.
        /// Multiple calls can be done if more than 1 exception is received</param>
        public BaseNServiceBusHandler(
            IBusHandlerProvider busHandlerProvider,
            IBus bus,
            Func<TMessage, BusMessage> nServiceBusToMessageFunc,
            Func<BusMessage, TMessage> messageToNServiceBusFunc,
            Action<Exception> exceptionAction = null)
        {
            this.busHandlerProvider = busHandlerProvider;
            this.bus = bus;
            this.nServiceBusToMessageFunc = nServiceBusToMessageFunc;
            this.messageToNServiceBusFunc = messageToNServiceBusFunc;
            this.exceptionAction = exceptionAction ?? (e => { throw e; });
        }

        public Task Handle(TMessage message, IMessageHandlerContext context)
        {
            var busMessage = this.Convert(message);

            var handlers = this.busHandlerProvider.GetHandlers().Where(bh => bh.CanHandleMessage(busMessage));

            var busContext = new BusContext(this.bus);

            try
            {
                Task.WaitAll(handlers.Select(busHandler => Task.Run(() => busHandler.HandleMessage(busContext, busMessage))).Cast<Task>().ToArray());
            }
            catch (AggregateException ae)
            {
                foreach (var e in ae.Flatten().InnerExceptions)
                {
                    this.exceptionAction(e);
                }
            }

            return Task.CompletedTask;
        }

        protected virtual BusMessage Convert(TMessage fromObject)
        {
            return this.nServiceBusToMessageFunc(fromObject);
        }

        protected virtual TMessage Convert(BusMessage fromObject)
        {
            return this.messageToNServiceBusFunc(fromObject);
        }
    }
}