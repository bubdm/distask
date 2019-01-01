using Distask.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Distask.Distributors
{
    public class ResponseMessage
    {
        public ResponseMessage(Status status, string result, string errorMessage, string stackTrace)
        {
            this.Status = status;
            this.Result = result;
            this.ErrorMessage = errorMessage;
            this.StackTrace = stackTrace;
        }

        public Status Status { get; }

        public string Result { get; }

        public string ErrorMessage { get; }

        public string StackTrace { get; }

        public static ResponseMessage Success(string result) => new ResponseMessage(Status.Success, result, null, null);

        public static ResponseMessage Success() => new ResponseMessage(Status.Success, null, null, null);

        public static ResponseMessage Error(string errorMessage) => new ResponseMessage(Status.Error, null, errorMessage, null);

        public static ResponseMessage Exception(Exception ex) => new ResponseMessage(Status.Error, null, ex.Message, ex.StackTrace);
    }
}
