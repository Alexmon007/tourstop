using Business.Connectors.Contracts;
using Business.Connectors.Petition;
using Business.Handlers.Authentication.contracts;
using Business.Handlers.Handlers.contracts;
using Business.Handlers.Request;
using Business.Handlers.Response;
using Business.Handlers.Validation;
using Business.Handlers.Validation.Request;
using Common.DTOs;
using Common.Exceptions;

namespace Business.Handlers.Handlers
{
    /// <summary>
    /// Where Controller Request turns into BusinessPetition
    /// </summary>
    public abstract class BaseRequestHandler<T> : IBaseRequestHandler<T> where T : BaseDTO
    {
        #region Instance Properties

        private readonly IBaseConnector<T> _connector;
        private readonly IRequestAuthenticator _authenticator;

        #endregion

        #region Validator Properties

        protected abstract BaseValidator<T> FullValidator { get; }
        protected abstract BaseValidator<T> DeleteValidator { get; }

        #endregion

        /// <summary>
        /// Contructor
        /// </summary>
        /// <param name="connector">Generic Connector</param>
        /// <param name="authenticator">Aunthenticator</param>
        protected BaseRequestHandler(IBaseConnector<T> connector,
            IRequestAuthenticator authenticator)
        {
            _connector = connector;
            _authenticator = authenticator;
        }

        #region Handle Methods

        /// <summary>
        ///  Read Request into Response
        /// </summary>
        /// <param name="request">Service Request</param>
        /// <returns>Business Response</returns>
        public Response<T> HandleReadRequest(ReadRequest request)
        {
            ValidateRequest(request, ReadRequestValidator.GetValidator());

            var petition = (ReadBusinessPetition) request;
            petition.RequestingUser = _authenticator.Authenticate(request.AuthToken);

            var businessResponse = _connector.Get(petition);
            var response = (Response<T>) businessResponse;
            return response;
        }

        /// <summary>
        ///  Read/Write Request into Response
        /// </summary>
        /// <param name="request">Service Request</param>
        /// <returns>Business Response</returns>
        public Response<T> HandleReadWriteRequest(ReadWriteRequest<T> request)
        {
            ValidateRequest(request, ReadWriteRequestValidator<T>.Build(FullValidator));

            var petition = ParseReadWriteRequest(request);

            var businessResponse = _connector.Save(petition);
            var response = (Response<T>) businessResponse;
            return response;
        }

        /// <summary>
        ///  Delete Request into Response
        /// </summary>
        /// <param name="request">Service Request</param>
        /// <returns>Business Response</returns>
        public Response<T> HandleDeleteRequest(ReadWriteRequest<T> request)
        {
            ValidateRequest(request, ReadWriteRequestValidator<T>.Build(DeleteValidator));

            var petition = ParseReadWriteRequest(request);

            var businessResponse = _connector.Delete(petition);
            var response = (Response<T>) businessResponse;
            return response;
        }

        #endregion

        /// <summary>
        ///  Validate Request befor Response
        /// </summary>
        /// <param name="request">Service Request</param>
        /// <returns>nothing or Exception</returns>
        protected static void ValidateRequest<TRequest>(TRequest request, BaseValidator<TRequest> validator)
        {
            if (!validator.Validate(request).IsValid) throw new InvalidRequestException();
        }

        /// <summary>
        /// Turns Request into Petition
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Business petition</returns>
        protected ReadWriteBusinessPetition<T> ParseReadWriteRequest(ReadWriteRequest<T> request)
        {
            var petition = (ReadWriteBusinessPetition<T>) request;
            petition.RequestingUser = _authenticator.Authenticate(request.AuthToken);
            return petition;
        }
    }
}