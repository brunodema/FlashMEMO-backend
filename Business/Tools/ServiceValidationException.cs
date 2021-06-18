using System;
using System.Collections.Generic;

namespace Business.Tools
{
    public class ServiceValidationException : Exception
    {
        public ServiceValidationException() : base("The service could successfully validate the entity.") { }
        public List<string> Errors { get; set; } = null;
    }
}