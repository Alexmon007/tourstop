using AutoMapper;
using Business.Connectors.Contracts;
using Business.Connectors.Petition;
using Common.DTOs;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories.Contracts;

namespace Business.Connectors
{
    public class ReservationConnector: BaseConnector<ReservationDTO, Reservation>, IReservationConnector
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IMapper _mapper;

        public ReservationConnector(IReservationRepository repository, IMapper mapper) : base(repository, mapper)
        {
            _reservationRepository = repository;
            _mapper = mapper;
        }
        #region Validate Methods

        protected override bool ValidateGet(ReadBusinessPetition petition)
        {
            return petition.RequestingUser != null;
        }

        protected override bool ValidateSave(ReadWriteBusinessPetition<ReservationDTO> petition)
        {
            return false;
        }

        protected override bool ValidateDelete(ReadWriteBusinessPetition<ReservationDTO> petition)
        {
            return petition.RequestingUser != null && petition.Data != null &&
                petition.Data.TrueForAll(x => x.UserId == petition.RequestingUser.Id); //TODO: Think! Can ratings be deleted?
        }

        #endregion
    }
}
