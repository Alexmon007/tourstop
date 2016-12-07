using System;

namespace Business.Connectors.Exceptions
{
    public class InternalServerException : Exception
    {
        /// <summary>
        /// DataAccesLayer Exception, caused in Repositories.
        /// </summary>
        public InternalServerException(string message) : base(message)
        {
        }
    }
}