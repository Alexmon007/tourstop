using System.Linq;
using AutoMapper;
using Business.Connectors.Contracts;
using Business.Connectors.Petition;
using Common.DTOs;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories.Contracts;

namespace Business.Connectors
{
    /// <summary>
    /// Connector for User Entity
    /// Business Logic at BaseConnector
    /// </summary>
    public class UserConnector : BaseConnector<UserDTO, User>, IUserConnector
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="repository">Database Repository</param>
        /// <param name="mapper">Configuration Mapper</param>
        public UserConnector(IUserRepository repository, IMapper mapper) : base(repository, mapper)
        {
        }

        #region Validate Methods

        /// <summary>
        /// Implemented Business Rules for GET pettions
        /// </summary>
        /// <param name="petition">Requested information</param>
        /// <returns>Evaluation</returns>
        protected override bool ValidateGet(ReadBusinessPetition petition)
        {
            return petition.RequestingUser != null;
        }

        /// <summary>
        /// Implemented Business Rules for SAVE pettions
        /// </summary>
        /// <param name="petition">Requested information</param>
        /// <returns>Evaluation</returns>
        protected override bool ValidateSave(ReadWriteBusinessPetition<UserDTO> petition)
        {
            return petition.Data.All(x => x.Id == 0) || petition.Data.All(x => x.Id == petition.RequestingUser?.Id);
        }

        /// <summary>
        /// Implemented Business Rules for DELETE pettions
        /// </summary>
        /// <param name="petition">Requested information</param>
        /// <returns>Evaluation</returns>
        protected override bool ValidateDelete(ReadWriteBusinessPetition<UserDTO> petition)
        {
            return petition.RequestingUser != null && petition.Data != null &&
                   petition.Data.All(x => x.Id == petition.RequestingUser.Id);
        }

        #endregion
    }
}