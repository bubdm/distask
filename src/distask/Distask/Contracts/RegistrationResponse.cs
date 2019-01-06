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

namespace Distask.Contracts
{
    partial class RegistrationResponse
    {
        #region Public Methods

        /// <summary>
        /// Represents an error response of the registration request.
        /// </summary>
        /// <param name="rejectMessage">The error message.</param>
        /// <returns>The <c>RegistrationResponse</c> which carries the error flag.</returns>
        public static RegistrationResponse Error(string rejectMessage) =>
            new RegistrationResponse
            {
                Status = StatusCode.Error,
                RejectMessage = rejectMessage,
                Reason = Types.RejectReason.General
            };

        /// <summary>
        /// Represents a successful response of the registration request.
        /// </summary>
        /// <returns>The <c>RegistrationResponse</c> which carries the successful flag.</returns>
        public static RegistrationResponse Success() =>
            new RegistrationResponse
            {
                Status = StatusCode.Success,
                Reason = Types.RejectReason.None
            };

        public static RegistrationResponse AlreadyExists(string rejectMessage) =>
            new RegistrationResponse
            {
                Status = StatusCode.Error,
                RejectMessage = rejectMessage,
                Reason = Types.RejectReason.AlreadyExists
            };

        #endregion Public Methods
    }
}