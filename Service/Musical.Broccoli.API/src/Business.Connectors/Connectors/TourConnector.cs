using System.Linq;
using AutoMapper;
using Business.Connectors.Contracts;
using Business.Connectors.Petition;
using Common.DTOs;
using Common.Enums;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories.Contracts;

namespace Business.Connectors
{
    /// <summary>
    /// Connector for Tour Entity
    /// Business Logic at BaseConnector
    /// </summary>
    public class TourConnector : BaseConnector<TourDTO, Tour>, ITourConnector
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="repository">Database Repository</param>
        /// <param name="mapper">Configuration Mapper</param>
        public TourConnector(ITourRepository repository, IMapper mapper) : base(repository, mapper)
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
        protected override bool ValidateSave(ReadWriteBusinessPetition<TourDTO> petition)
        {
            return petition.RequestingUser != null && petition.Data != null &&
                (petition.Data.All(x => x.Id == 0) || petition.Data.All(x => x.UserId == petition.RequestingUser.Id))
                   && petition.RequestingUser.UserType == UserType.Promoter;
        }

        /// <summary>
        /// Implemented Business Rules for DELETE pettions
        /// </summary>
        /// <param name="petition">Requested information</param>
        /// <returns>Evaluation</returns>
        protected override bool ValidateDelete(ReadWriteBusinessPetition<TourDTO> petition)
        {
            return petition.RequestingUser != null && petition.Data != null &&
                petition.Data.All(x => x.UserId == petition.RequestingUser.Id);
        }

        #endregion
    }
}