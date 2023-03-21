﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Newshore.Journeys.Domain.Exceptions
{
    public class DomainException : Exception
    {
        public DomainException() { }

        public DomainException(string message)
            : base(message) { }

        public DomainException(string message, Exception inner)
            : base(message, inner) { }
    }
}
