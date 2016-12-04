using AutoMapper;
using Business.Connectors.Contracts;
using Business.Connectors.Petition;
using Common.DTOs;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories.Contracts;

namespace Business.Connectors
{
    public class RatingConnector : BaseConnector<RatingDTO, Rating>, IRatingConnector
    {
        public RatingConnector(IRatingRepository repository, IMapper mapper) : base(repository, mapper)
        {
        }

        #region Validate Methods

        protected override bool ValidateGet(ReadBusinessPetition petition)
        {
            return petition.RequestingUser != null;
        }

        protected override bool ValidateSave(ReadWriteBusinessPetition<RatingDTO> petition)
        {
            return true;
        }

        protected override bool ValidateDelete(ReadWriteBusinessPetition<RatingDTO> petition)
        {
            return petition.RequestingUser != null && petition.Data != null &&
                petition.Data.TrueForAll(x=>x.UserId==petition.RequestingUser.Id); //TODO: Think! Can ratings be deleted?
        }

        #endregion
    }
}