using System;
using System.Collections.Generic;
using System.Text;

namespace Distask.Masters
{
    public class ResponseMessage
    {
        public ResponseMessage(Status status, string result, string errorMessage)
        {
            this.Status = status;
            this.Result = result;
            this.ErrorMessage = errorMessage;
        }

        public Status Status { get; }

        public string Result { get; }

        public string ErrorMessage { get; }
    }
}
