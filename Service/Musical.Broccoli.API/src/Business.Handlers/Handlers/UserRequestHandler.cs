using Business.Connectors.Contracts;
using Business.Connectors.Petition;
using Business.Handlers.Authentication.contracts;
using Business.Handlers.Handlers.contracts;
using Business.Handlers.Request;
using Business.Handlers.Response;
using Business.Handlers.Validation;
using Business.Handlers.Validation.Dto;
using Business.Handlers.Validation.Request;
using Common.DTOs;

namespace Business.Handlers.Handlers
{
    public class UserRequestHandler : BaseRequestHandler<UserDTO>, IUserRequestHandler
    {
        private readonly ISessionConnector _sessionConnector;

        public UserRequestHandler(IUserConnector connector, IRequestAuthenticator authenticator,
            ISessionConnector sessionConnector)
            : base(connector, authenticator)
        {
            _sessionConnector = sessionConnector;
        }

        protected override BaseValidator<UserDTO> FullValidator => UserValidator.All();
        protected override BaseValidator<UserDTO> DeleteValidator => UserValidator.HasId();

        public Response<SessionDTO> HandleLoginRequest(ReadWriteRequest<UserDTO> request)
        {
            ValidateRequest(request, ReadWriteRequestValidator<UserDTO>.Build(
                UserValidator.EmailNotEmpty().And(UserValidator.PasswordNotEmpty())));

            var petition = (ReadWriteBusinessPetition<UserDTO>) request;

            var businessResponse = _sessionConnector.Save(petition);

            return (Response<SessionDTO>) businessResponse;
        }
    }
}