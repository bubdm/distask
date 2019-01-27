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

using Distask.TaskDispatchers;
using System;

namespace Distask.Contracts
{
    partial class DistaskResponse
    {
        #region Public Methods

        /// <summary>
        /// Constructs a <c>DistaskResponse</c> instance which represents an error
        /// response with the specified error message.
        /// </summary>
        /// <param name="errorMessage">The error message.</param>
        /// <returns>The Distask response instance.</returns>
        public static DistaskResponse Error(string errorMessage) => new DistaskResponse
        {
            Status = StatusCode.Error,
            ErrorMessage = errorMessage
        };

        /// <summary>
        /// Constructs a <c>DistaskResponse</c> instance which represents an error
        /// response with the specified exception.
        /// </summary>
        /// <param name="ex">The exception which caused the error response.</param>
        /// <returns>The Distask response instance.</returns>
        public static DistaskResponse Exception(Exception ex) => new DistaskResponse
        {
            Status = StatusCode.Error,
            ErrorMessage = ex.Message,
            StackTrace = ex.StackTrace,
            ErrorType = ex.GetType().Name,
            ErrorDetails = ex.ToString()
        };

        /// <summary>
        /// Constructs a <c>DistaskResponse</c> instance which represents a success response.
        /// </summary>
        /// <returns>The Distask response instance.</returns>
        public static DistaskResponse Success() => new DistaskResponse
        {
            Status = StatusCode.Success
        };

        /// <summary>
        /// Constructs a <c>DistaskResponse</c> instance which represents a success response
        /// that contains the result data.
        /// </summary>
        /// <param name="result">The result data, in <see cref="string"/> representation.</param>
        /// <returns>The Distask response instance.</returns>
        public static DistaskResponse Success(string result) => new DistaskResponse
        {
            Status = StatusCode.Success,
            Result = result
        };

        /// <summary>
        /// Constructs a <c>DistaskResponse</c> instance which represents a warning response
        /// with the specified warning message.
        /// </summary>
        /// <param name="warningMessage">The warning message.</param>
        /// <returns>The Distask response instance.</returns>
        public static DistaskResponse Warning(string warningMessage) => new DistaskResponse
        {
            Status = StatusCode.Warning,
            ErrorMessage = warningMessage
        };

        /// <summary>
        /// Converts the current <c>DistaskResponse</c> message to the <see cref="ResponseMessage"/> instance.
        /// </summary>
        /// <returns>The converted <see cref="ResponseMessage"/> instance.</returns>
        public ResponseMessage ToResponseMessage()
        {
            var status = ResponseStatus.Success;
            switch (this.Status)
            {
                case StatusCode.Error:
                    status = ResponseStatus.Error;
                    break;

                case StatusCode.Warning:
                    status = ResponseStatus.Warning;
                    break;

                default: break;
            }

            return new ResponseMessage(status, this.Result, this.ErrorMessage, this.StackTrace);
        }

        #endregion Public Methods
    }
}