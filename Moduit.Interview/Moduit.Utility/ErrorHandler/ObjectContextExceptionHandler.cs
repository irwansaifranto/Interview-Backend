using Moduit.Domain.CustomException;
using Moduit.Domain.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Moduit.Utility.ErrorHandler
{
    public static class ObjectContextExceptionHandler
    {
        public static T ExecuteHandler<T>(ErrorMessageConstant errorScope, string instanceScope, string requestPayload, string responseDetail, Func<T> action)
        {
            try
            {
                return action();
            }
            catch (TimeoutException exp)
            {
                throw new ObjectContextException(errorScope, instanceScope, exp.Message, requestPayload, responseDetail, exp.StackTrace, (int)HttpStatusCode.RequestTimeout);
            }
            catch (EntryPointNotFoundException exp)
            {
                throw new ObjectContextException(errorScope, instanceScope, exp.Message, requestPayload, responseDetail, exp.StackTrace, (int)HttpStatusCode.NotFound);
            }
            catch (UnauthorizedAccessException exp)
            {
                throw new ObjectContextException(errorScope, instanceScope, exp.Message, requestPayload, responseDetail, exp.StackTrace, (int)HttpStatusCode.Unauthorized);
            }
            catch (ObjectContextException exp)
            {
                throw new ObjectContextException(errorScope, instanceScope, exp.Message, requestPayload, exp.ResponseDetail, exp.StackTrace, (int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
