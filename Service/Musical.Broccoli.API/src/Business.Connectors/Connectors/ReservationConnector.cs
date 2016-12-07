using AutoMapper;
using Business.Connectors.Contracts;
using Business.Connectors.Petition;
using Common.DTOs;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories.Contracts;

namespace Business.Connectors
{
    /// <summary>
    /// Connector for Reservation Entity
    /// Business Logic at BaseConnector
    /// </summary>
    public class ReservationConnector: BaseConnector<ReservationDTO, Reservation>, IReservationConnector
    {

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="repository">Database Repository</param>
        /// <param name="mapper">Configuration Mapper</param>
        public ReservationConnector(IReservationRepository repository, IMapper mapper) : base(repository, mapper)
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
        protected override bool ValidateSave(ReadWriteBusinessPetition<ReservationDTO> petition)
        {
            return false;
        }

        /// <summary>
        /// Implemented Business Rules for DELETE pettions
        /// </summary>
        /// <param name="petition">Requested information</param>
        /// <returns>Evaluation</returns>
        protected override bool ValidateDelete(ReadWriteBusinessPetition<ReservationDTO> petition)
        {
            return petition.RequestingUser != null && petition.Data != null &&
                petition.Data.TrueForAll(x => x.UserId == petition.RequestingUser.Id); //TODO: Think! Can ratings be deleted?
        }

        #endregion
    }
}
