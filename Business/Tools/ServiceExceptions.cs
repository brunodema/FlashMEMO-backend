using System;
using System.Collections.Generic;

namespace Business.Tools.Exceptions
{
    public class EntityValidationException : Exception
    {
        /// <summary>
        /// Should be thrown whenever an entity is being manipulated and its properties have invalid data (ex: News object with a creation date after today's date).
        /// </summary>
        public EntityValidationException() : base("The service could successfully validate the entity.") { }
        public List<string> ServiceValidationErrors { get; set; } = null;
    }

    /// <summary>
    /// Should be thrown whenever input for a service or controller function (such as query parameters) are not in accordance to what is expected of it.
    /// </summary>
    public class InputValidationException : Exception
    {
        public InputValidationException() : base("The input provided contains problems.") { }
        public List<string> InputValidationErrors { get; set; } = null;
    }
}