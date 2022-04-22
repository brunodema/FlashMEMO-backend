using System;

namespace Data.Tools.Exceptions
{
    namespace Repository
    {
        /// <summary>
        /// Exception to be thrown when an object is not found by the repository using a provided Id.
        /// </summary>
        public class ObjectNotFoundWithId<TKey> : Exception
        {
            public const string DefaultExceptionMessage = "An object with the provided Id has not been found in the database. Please check if the Id provided is valid";
            public TKey FaultyId { get; set; } = default(TKey);

            public ObjectNotFoundWithId(TKey id) : base(DefaultExceptionMessage) { FaultyId = id; }
        }
    }
}
