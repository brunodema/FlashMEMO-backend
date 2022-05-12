using System;
using System.Collections.Generic;

namespace Business.Tools.Validations
{
    public class ServiceValidationMessages
    {
        public static readonly string CreationDateMoreRecentThanLastUpdated = "The last updated date must be more recent than the creation date";
        public static readonly string InvalidLanguageCode = "The language code provided is not valid.";
        public static readonly string InvalidUserId = "The user provided does not seem to exist within FlashMEMO.";
        public static readonly string InvalidDeckId = "The deck provided does not seem to exist within FlashMEMO.";
    }
}

namespace Business.Tools.Exceptions
{
    /// <summary>
    /// Should be thrown whenever an entity is being manipulated and its properties have invalid data (ex: News object with a creation date after today's date).
    /// </summary>
    public class EntityValidationException : Exception
    {
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