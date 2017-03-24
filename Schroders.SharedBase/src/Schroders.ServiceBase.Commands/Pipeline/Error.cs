using System;
using Newtonsoft.Json;

namespace Schroders.ServiceBase.Commands.Pipeline
{
    public class Error
    {
        public Error(string errorCode, string messageShort, string messageLong = null, Uri errorUrl = null)
        {
            this.ErrorCode = errorCode;
            this.MessageShort = messageShort;
            this.MessageLong = messageLong;
            this.ErrorUrl = errorUrl;
        }

        [JsonProperty(PropertyName = "errorCode")]
        public string ErrorCode { get; private set; }

        [JsonProperty(PropertyName = "messageShort")]
        public string MessageShort { get; private set; }

        [JsonProperty(PropertyName = "messageLong")]
        public string MessageLong { get; private set; }

        [JsonProperty(PropertyName = "errorUrl")]
        public Uri ErrorUrl { get; private set; }
    }
}
