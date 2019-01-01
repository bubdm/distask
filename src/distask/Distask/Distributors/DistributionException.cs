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
using System.Runtime.Serialization;

namespace Distask.Distributors
{
    /// <summary>
    /// Represents the error that occurs when a task is being distributed.
    /// </summary>
    /// <seealso cref="Distask.DistaskException" />
    public class DistributionException : DistaskException
    {
        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DistributionException"/> class.
        /// </summary>
        public DistributionException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DistributionException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public DistributionException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DistributionException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public DistributionException(string message, Exception innerException) : base(message, innerException)
        {
        }

        #endregion Public Constructors

        #region Protected Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DistributionException"/> class.
        /// </summary>
        /// <param name="info">The information.</param>
        /// <param name="context">The context.</param>
        protected DistributionException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }

        #endregion Protected Constructors
    }
}