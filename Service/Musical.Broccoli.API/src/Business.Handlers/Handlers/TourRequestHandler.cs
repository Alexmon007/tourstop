using Business.Connectors.Contracts;
using Business.Handlers.Authentication.contracts;
using Business.Handlers.Handlers.contracts;
using Business.Handlers.Validation;
using Business.Handlers.Validation.Dto;
using Common.DTOs;

namespace Business.Handlers.Handlers
{
    /// <summary>
    /// Where OrderController Requests turns into BusinessPetitions
    /// Logic in BaseRequestHandler
    /// </summary>
    public class TourRequestHandler : BaseRequestHandler<TourDTO>, ITourRequestHandler
    {
        public TourRequestHandler(ITourConnector connector, IRequestAuthenticator authenticator)
            : base(connector, authenticator)
        {
        }

        protected override BaseValidator<TourDTO> FullValidator => TourValidator.All();
        protected override BaseValidator<TourDTO> DeleteValidator => TourValidator.HasId();
    }
}