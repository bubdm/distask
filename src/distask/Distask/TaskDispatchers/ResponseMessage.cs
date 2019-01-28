/****************************************************************************
 *           ___      __             __
 *      ____/ (_)____/ /_____ ______/ /__
 *     / __  / / ___/ __/ __ `/ ___/ //_/
 *    / /_/ / (__  ) /_/ /_/ (__  ) ,<
 *    \__,_/_/____/\__/\__,_/____/_/|_|
 *
 * Copyright (C) 2018-2019 by daxnet, https://github.com/daxnet/distask
 * All rights reserved.
 * Licensed under MIT License.
 * https://github.com/daxnet/distask/blob/master/LICENSE
 ****************************************************************************/

using System;
using System.Collections.Generic;

namespace Distask.TaskDispatchers
{
    /// <summary>
    /// Represents the response message returned from any of the registered broker.
    /// </summary>
    public class ResponseMessage
    {
        #region Public Constructors

        public ResponseMessage(ResponseStatus status, string result, string errorMessage, string stackTrace)
        {
            this.Status = status;
            this.Result = result;
            this.ErrorMessage = errorMessage;
            this.StackTrace = stackTrace;
        }

        #endregion Public Constructors

        #region Public Properties

        public string ErrorMessage { get; }
        public string Result { get; }
        public string StackTrace { get; }
        public ResponseStatus Status { get; }

        #endregion Public Properties

        #region Public Methods

        public static ResponseMessage Error(string errorMessage) => new ResponseMessage(ResponseStatus.Error, null, errorMessage, null);

        public static ResponseMessage Exception(Exception ex) => new ResponseMessage(ResponseStatus.Error, null, ex.Message, ex.StackTrace);

        public static ResponseMessage Success(string result) => new ResponseMessage(ResponseStatus.Success, result, null, null);

        public static ResponseMessage Success() => new ResponseMessage(ResponseStatus.Success, null, null, null);

        public override bool Equals(object obj)
        {
            var message = obj as ResponseMessage;
            return message != null &&
                   Status == message.Status &&
                   Result == message.Result &&
                   ErrorMessage == message.ErrorMessage &&
                   StackTrace == message.StackTrace;
        }

        public override int GetHashCode()
        {
            var hashCode = -1347505953;
            hashCode = hashCode * -1521134295 + Status.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Result);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ErrorMessage);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(StackTrace);
            return hashCode;
        }

        public override string ToString()
        {
            switch (Status)
            {
                case ResponseStatus.Success:
                    return $"[{Status}] {Result}";
                case ResponseStatus.Warning:
                    return $"[{Status}] {ErrorMessage}";
                case ResponseStatus.Error:
                    return $"[{Status}] {ErrorMessage} {StackTrace}";
            }

            return null;
        }

        #endregion Public Methods
    }
}
