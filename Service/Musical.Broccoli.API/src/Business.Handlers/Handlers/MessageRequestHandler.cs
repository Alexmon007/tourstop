using Business.Connectors.Contracts;
using Business.Handlers.Authentication.contracts;
using Business.Handlers.Handlers.contracts;
using Business.Handlers.Validation;
using Business.Handlers.Validation.Dto;
using Common.DTOs;

namespace Business.Handlers.Handlers
{
    public class MessageRequestHandler : BaseRequestHandler<MessageDTO>, IMessageRequestHandler
    {
        public MessageRequestHandler(IMessageConnector connector, IRequestAuthenticator authenticator)
            : base(connector, authenticator)
        {
        }

        protected override BaseValidator<MessageDTO> FullValidator => MessageValidator.All();
        protected override BaseValidator<MessageDTO> DeleteValidator => MessageValidator.HasId();
    }
}