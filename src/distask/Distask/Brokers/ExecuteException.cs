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

namespace Distask.Brokers
{
    /// <summary>
    /// Represents the error that occurs when executing a broker task.
    /// </summary>
    /// <seealso cref="Distask.DistaskException" />
    public sealed class ExecuteException : DistaskException
    {
        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ExecuteException"/> class.
        /// </summary>
        public ExecuteException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExecuteException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public ExecuteException(string message)
            : base(message)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExecuteException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public ExecuteException(string message, Exception innerException)
            : base(message, innerException)
        { }
        #endregion Public Constructors

    }
}