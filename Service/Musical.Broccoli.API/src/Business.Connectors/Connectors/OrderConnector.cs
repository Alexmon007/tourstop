using System.Linq;
using AutoMapper;
using Business.Connectors.Contracts;
using Business.Connectors.Petition;
using Common.DTOs;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories.Contracts;
using System.Text.RegularExpressions;

namespace Business.Connectors
{
    public class OrderConnector : BaseConnector<OrderDTO, Order>, IOrderConnector
    {
        public OrderConnector(IOrderRepository repository, IMapper mapper) : base(repository, mapper)
        {
        }

        #region Validate Methods

        protected override bool ValidateGet(ReadBusinessPetition petition)
        {
            if (string.IsNullOrEmpty(petition.FilterString)) { return false; }
            var matches = Regex.Matches(petition.FilterString, @"[UserId=](\d*.)", RegexOptions.ExplicitCapture);
            return petition.RequestingUser != null &&
                matches.Count == 4 &&
                petition.FilterString.Contains(string.Format("UserId = {0}", petition.RequestingUser.Id)); //TODO: Really only the owner user can see the Order THINK MY FRIEND
        }

        protected override bool ValidateSave(ReadWriteBusinessPetition<OrderDTO> petition)
        {
            return petition.RequestingUser != null;
        }

        protected override bool ValidateDelete(ReadWriteBusinessPetition<OrderDTO> petition)
        {
            return false;
        }

        #endregion
    }
}