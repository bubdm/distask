using Distask.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Distask.TaskDispatchers
{
    /// <summary>
    /// Represents the response message returned from any of the registered broker.
    /// </summary>
    public class ResponseMessage
    {
        public ResponseMessage(ResponseStatus status, string result, string errorMessage, string stackTrace)
        {
            this.Status = status;
            this.Result = result;
            this.ErrorMessage = errorMessage;
            this.StackTrace = stackTrace;
        }

        public ResponseStatus Status { get; }

        public string Result { get; }

        public string ErrorMessage { get; }

        public string StackTrace { get; }

        public static ResponseMessage Success(string result) => new ResponseMessage(ResponseStatus.Success, result, null, null);

        public static ResponseMessage Success() => new ResponseMessage(ResponseStatus.Success, null, null, null);

        public static ResponseMessage Error(string errorMessage) => new ResponseMessage(ResponseStatus.Error, null, errorMessage, null);

        public static ResponseMessage Exception(Exception ex) => new ResponseMessage(ResponseStatus.Error, null, ex.Message, ex.StackTrace);
    }
}
