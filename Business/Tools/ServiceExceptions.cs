using System;
using System.Collections.Generic;

namespace Business.Tools.Exceptions
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

    public class APIRequestException : Exception
    {
        protected string APIName { get; set; }

        public APIRequestException(string apiName) : base("A problem occurred while making a request to the API") { APIName = apiName; }
        public List<string> APIRequestErrors { get; set; } = null;
    }
}