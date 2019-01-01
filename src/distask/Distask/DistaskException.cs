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

namespace Distask
{
    /// <summary>
    /// Represents the error that occurs in the Distask framework.
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class DistaskException : Exception
    {
        #region Public Constructors

        public DistaskException()
        {
        }

        public DistaskException(string message) : base(message)
        {
        }

        public DistaskException(string message, Exception innerException) : base(message, innerException)
        {
        }

        #endregion Public Constructors

        #region Protected Constructors

        protected DistaskException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }

        #endregion Protected Constructors
    }
}