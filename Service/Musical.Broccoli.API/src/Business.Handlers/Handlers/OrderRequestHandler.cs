using Business.Connectors.Contracts;
using Business.Handlers.Authentication.contracts;
using Business.Handlers.Handlers.contracts;
using Business.Handlers.Validation;
using Business.Handlers.Validation.Dto;
using Common.DTOs;

namespace Business.Handlers.Handlers
{
    public class OrderRequestHandler : BaseRequestHandler<OrderDTO>, IOrderRequestHandler
    {
        public OrderRequestHandler(IOrderConnector connector, IRequestAuthenticator authenticator)
            : base(connector, authenticator)
        {
        }

        protected override BaseValidator<OrderDTO> FullValidator => OrderValidator.All();
        protected override BaseValidator<OrderDTO> DeleteValidator => OrderValidator.HasId();
    }
}