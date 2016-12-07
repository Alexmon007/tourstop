using System.Linq;
using Business.Connectors.Contracts;
using Business.Connectors.Petition;
using Business.Handlers.Authentication.contracts;
using Common.DTOs;

namespace Business.Handlers.Authentication
{
    /// <summary>
    /// Where the Authorization Token is turn into user
    /// </summary>
    public class RequestAuthenticator : IRequestAuthenticator
    {
        private readonly ISessionConnector _connector;

        /// <summary>
        /// Contructor
        /// </summary>
        /// <param name="connector">UserConnector</param>
        public RequestAuthenticator(ISessionConnector connector)
        {
            _connector = connector;
        }

        /// <summary>
        /// Turns Authorization Token to user if is valid.
        /// </summary>
        /// <param name="authToken">Authorization Token</param>
        /// <returns>Requested User</returns>
        public UserDTO Authenticate(string authToken)
        {
            if (string.IsNullOrEmpty(authToken)) return null;

            var filterString = string.Format(@"AuthToken=""{0}""", authToken);
            var petition = new ReadBusinessPetition
            {
                FilterString = filterString
            };

            var session = _connector.Get(petition).Data.Single();
            return session.User;
        }
    }
}