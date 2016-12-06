using System.Text.RegularExpressions;
using AutoMapper;
using Business.Connectors.Contracts;
using Business.Connectors.Petition;
using Common.DTOs;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories.Contracts;

namespace Business.Connectors
{
    public class OrderConnector : BaseConnector<OrderDTO, Order>, IOrderConnector
    {
        public OrderConnector( IOrderRepository repository, IMapper mapper ) : base( repository, mapper )
        {
        }

        #region Validate Methods

        protected override bool ValidateGet( ReadBusinessPetition petition )
        {
            if (string.IsNullOrEmpty( petition.FilterString )) { return false; }
            var matches = Regex.Matches( petition.FilterString, @"[UserId=](\d)", RegexOptions.IgnoreCase );
            return petition.RequestingUser != null
                && matches.Count == 1
                && int.Parse( matches[0].Groups[1].Value ) == petition.RequestingUser.Id;
        }

        protected override bool ValidateSave( ReadWriteBusinessPetition<OrderDTO> petition )
        {
            return petition.RequestingUser != null && petition.Data.TrueForAll( x => x.UserId == petition.RequestingUser.Id );
        }

        protected override bool ValidateDelete( ReadWriteBusinessPetition<OrderDTO> petition )
        {
            return false;
        }

        #endregion
    }
}