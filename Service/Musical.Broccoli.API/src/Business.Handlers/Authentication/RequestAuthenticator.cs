using System.Linq;
using Business.Connectors.Contracts;
using Business.Connectors.Petition;
using Business.Handlers.Authentication.contracts;
using Common.DTOs;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Business.Handlers.Authentication
{
    public class RequestAuthenticator : IRequestAuthenticator
    {
        private readonly ISessionConnector _connector;

        public RequestAuthenticator(ISessionConnector connector)
        {
            _connector = connector;
        }


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