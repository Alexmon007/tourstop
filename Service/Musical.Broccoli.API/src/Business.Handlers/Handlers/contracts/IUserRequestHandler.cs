using Business.Handlers.Request;
using Business.Handlers.Response;
using Common.DTOs;

namespace Business.Handlers.Handlers.contracts
{
    public interface IUserRequestHandler : IBaseRequestHandler<UserDTO>
    {
        Response<SessionDTO> HandleLoginRequest(ReadWriteRequest<UserDTO> request);
    }
}