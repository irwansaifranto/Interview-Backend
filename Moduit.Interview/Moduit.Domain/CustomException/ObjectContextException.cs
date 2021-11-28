using Moduit.Domain.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moduit.Domain.CustomException
{
    [Serializable]
    public class ObjectContextException : Exception
    {
        public ObjectContextException(
            ErrorMessageConstant errorContext,
            string instanceScope,
            string customMessage,
            string requestPayload,
            string responseDetail,
            string stackTrace,
            int statusCode)
        {
            ErrorContext = errorContext;
            InstanceScope = instanceScope;
            CustomMessage = customMessage;
            RequestPayload = requestPayload;
            ResponseDetail = responseDetail;
            StackTrace = stackTrace;
            StatusCode = statusCode;
        }

        public ErrorMessageConstant ErrorContext { get; private set; }
        public string InstanceScope { get; private set; }
        public string CustomMessage { get; private set; }
        public string RequestPayload { get; set; }
        public string ResponseDetail { get; set; }
        public new string StackTrace { get; set; }
        public int StatusCode { get; private set; }
    }
}
