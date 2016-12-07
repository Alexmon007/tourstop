using System.Text.RegularExpressions;
using AutoMapper;
using Business.Connectors.Contracts;
using Business.Connectors.Petition;
using Common.DTOs;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories.Contracts;
using System.Linq;

namespace Business.Connectors
{
    /// <summary>
    /// Connector for Order Entity
    /// Business Logic at BaseConnector
    /// </summary>
    public class OrderConnector : BaseConnector<OrderDTO, Order>, IOrderConnector
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="repository">Database Repository</param>
        /// <param name="mapper">Configuration Mapper</param>
        public OrderConnector(IOrderRepository repository, IMapper mapper) : base(repository, mapper)
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
            if (string.IsNullOrEmpty( petition.FilterString )) { return false; }
            var matches = Regex.Matches( petition.FilterString, @"[UserId=](\d)", RegexOptions.IgnoreCase );
            return petition.RequestingUser != null
                && matches.Count == 1
                && int.Parse( matches[0].Groups[1].Value ) == petition.RequestingUser.Id;
        }
        /// <summary>
        /// Implemented Business Rules for SAVE pettions
        /// </summary>
        /// <param name="petition">Requested information</param>
        /// <returns>Evaluation</returns>
        protected override bool ValidateSave(ReadWriteBusinessPetition<OrderDTO> petition)
        {
            return petition.RequestingUser != null && petition.Data.TrueForAll( x => x.UserId == petition.RequestingUser.Id );
        }

        /// <summary>
        /// Implemented Business Rules for DELETE pettions
        /// </summary>
        /// <param name="petition">Requested information</param>
        /// <returns>Evaluation</returns>
        protected override bool ValidateDelete(ReadWriteBusinessPetition<OrderDTO> petition)
        {
            return false;
        }

        #endregion
    }
}