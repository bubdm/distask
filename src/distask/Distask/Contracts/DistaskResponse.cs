using Distask.Distributors;
using System;
using System.Collections.Generic;
using System.Text;

namespace Distask.Contracts
{
    partial class DistaskResponse
    {
        public static DistaskResponse Exception(Exception ex) => new DistaskResponse
        {
            Status = StatusCode.Error,
            ErrorMessage = ex.Message,
            StackTrace = ex.StackTrace
        };

        public static DistaskResponse Success() => new DistaskResponse
        {
            Status = StatusCode.Success
        };

        public static DistaskResponse Success(string result) => new DistaskResponse
        {
            Status = StatusCode.Success,
            Result = result
        };

        public static DistaskResponse Error(string errorMessage) => new DistaskResponse
        {
            Status = StatusCode.Error,
            ErrorMessage = errorMessage
        };

        public static DistaskResponse Warning(string warningMessage) => new DistaskResponse
        {
            Status = StatusCode.Warning,
            ErrorMessage = warningMessage
        };

        public ResponseMessage ToResponseMessage()
        {
            var status = Distributors.Status.Success;
            switch (this.Status)
            {
                case StatusCode.Error:
                    status = Distributors.Status.Error;
                    break;
                case StatusCode.Warning:
                    status = Distributors.Status.Warning;
                    break;
                default: break;
            }

            return new ResponseMessage(status, this.Result, this.ErrorMessage, this.StackTrace);
        }
    }
}
