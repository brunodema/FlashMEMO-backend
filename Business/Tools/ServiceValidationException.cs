using System;

namespace Business.Tools
{
    public class ServiceValidationException : Exception
    {
        public ServiceValidationException() : base("The service could successfully validate the entity.") { }
        public string[] Errors { get; set; } = { };
    }
}