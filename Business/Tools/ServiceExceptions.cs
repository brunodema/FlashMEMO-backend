using System;
using System.Collections.Generic;

namespace Business.Tools
{
    public class ServiceValidationException : Exception
    {
        public ServiceValidationException() : base("The service could successfully validate the entity.") { }
        public List<string> ServiceValidationErrors { get; set; } = null;
    }

    public class InputValidationException : Exception
    {
        public InputValidationException() : base("The input provided contains problems.") { }
        public List<string> InputValidationErrors { get; set; } = null;
    }
}