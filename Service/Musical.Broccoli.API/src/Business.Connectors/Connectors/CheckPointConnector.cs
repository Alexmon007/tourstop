using AutoMapper;
using Business.Connectors.Contracts;
using Business.Connectors.Petition;
using Common.DTOs;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories.Contracts;

namespace Business.Connectors
{
    /// <summary>
    /// Connector for Address CheckPoint
    /// Business Logic at BaseConnector
    /// </summary>
    public class CheckPointConnector : BaseConnector<CheckPointDTO, CheckPoint>, ICheckPointConnector
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="repository">Database Repository</param>
        /// <param name="mapper">Configuration Mapper</param>
        public CheckPointConnector(ICheckPointRepository repository, IMapper mapper) : base(repository, mapper)
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
        protected override bool ValidateSave(ReadWriteBusinessPetition<CheckPointDTO> petition)
        {
            return petition.RequestingUser != null;
        }

        /// <summary>
        /// Implemented Business Rules for DELETE pettions
        /// </summary>
        /// <param name="petition">Requested information</param>
        /// <returns>Evaluation</returns>
        protected override bool ValidateDelete(ReadWriteBusinessPetition<CheckPointDTO> petition)
        {
            return false;
        }

        #endregion
    }
}