using System;
using Business.Connectors.Petition;
using Business.Connectors.Response;
using Common.DTOs;

namespace Business.Connectors.Contracts
{
    public interface ISessionConnector : IBaseConnector<SessionDTO>
    {
        new Func<ReadWriteBusinessPetition<UserDTO>, BusinessResponse<SessionDTO>> Save { get; }
    }
}