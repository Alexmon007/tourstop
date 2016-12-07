using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text.RegularExpressions;
using AutoMapper;
using Business.Connectors.Contracts;
using Business.Connectors.Petition;
using Business.Connectors.Response;
using Common.DTOs;
using Common.Exceptions;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories.Contracts;
using AuthenticationException = System.Security.Authentication.AuthenticationException;

namespace Business.Connectors
{
    /// <summary>
    /// Connector for Message Entity
    /// Business Logic at BaseConnector
    /// </summary>
    public class SessionConnector : BaseConnector<SessionDTO, Session>, ISessionConnector
    {
        protected new ISessionRepository Repository => (ISessionRepository) base.Repository;
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// Constructor
        /// UserRepostory is also added
        /// </summary>
        /// <param name="repository">Database Repository</param>
        /// <param name="userRepository">User Repistory</param>
        /// <param name="mapper">Configuration Mapper</param>
        public SessionConnector(ISessionRepository repository, IMapper mapper, IUserRepository userRepository)
            : base(repository, mapper)
        {
            _userRepository = userRepository;
        }

        #region Processing Functions

        /// <summary>
        /// Select the process through out the pettion type
        /// </summary>
        public new Func<ReadBusinessPetition, BusinessResponse<SessionDTO>> Get => ProcessGet;
        public new Func<ReadWriteBusinessPetition<UserDTO>, BusinessResponse<SessionDTO>> Save => ProcessSave;
        public new Func<ReadWriteBusinessPetition<SessionDTO>, BusinessResponse<SessionDTO>> Delete => ProcessDelete;

        #endregion

        #region Petition Processing
        /// <summary>
        /// Procces GET Petitions
        /// </summary>
        /// <param name="petition">Requested information</param>
        /// <returns>Response with infomation</returns>
        private new BusinessResponse<SessionDTO> ProcessGet(ReadBusinessPetition petition)
        {
            if (!Validate(petition, ValidateGet)) throw new AuthenticationException();

            var businessResponse = new BusinessResponse<SessionDTO>();

            try
            {
                var session = Repository.GetQueryable().Where(petition.FilterString).Single();
                var sessionDto = Mapper.Map<SessionDTO>(session);

                businessResponse.Data = new List<SessionDTO>
                {
                    sessionDto
                };
            }
            catch (Exception)
            {
                throw new InternalServerErrorException();
            }

            return businessResponse;
        }

        /// <summary>
        /// Procces SAVE Petitions
        /// </summary>
        /// <param name="petition">Requested information</param>
        /// <returns>Response</returns>
        private BusinessResponse<SessionDTO> ProcessSave(ReadWriteBusinessPetition<UserDTO> petition)
        {
            if (!Validate(petition, ValidateSave)) throw new AuthenticationException();

            var businessResponse = new BusinessResponse<SessionDTO>();

            try
            {
                var userDto = petition.Data.ElementAt(0);
                var user =
                    _userRepository.GetQueryable()
                        .Single(x => x.Email == userDto.Email && x.Password == userDto.Password);

                var session = new Session
                {
                    AuthToken = Guid.NewGuid(),
                    DateCreated = DateTime.Now,
                    UserID = user.Id
                };

                session = Repository.AddOrUpdate(session);
                Repository.SaveChanges();

                businessResponse.Data = new List<SessionDTO>
                {
                    Mapper.Map<SessionDTO>(session)
                };
            }
            catch (Exception)
            {
                throw new InternalServerErrorException();
            }

            return businessResponse;
        }

        /// <summary>
        /// Procces DELETE Petitions
        /// </summary>
        /// <param name="petition">Requested information</param>
        /// <returns>Response</returns>
        private new BusinessResponse<SessionDTO> ProcessDelete(ReadWriteBusinessPetition<SessionDTO> petition)
        {
            if (!Validate(petition, ValidateDelete)) throw new AuthenticationException();

            throw new InternalServerErrorException();
        }

        #endregion

        #region Validate Methods

        /// <summary>
        /// Implemented Business Rules for GET pettions
        /// </summary>
        /// <param name="petition">Requested information</param>
        /// <returns>Evaluation</returns>
        protected override bool ValidateGet(ReadBusinessPetition petition)
        {
            const string pattern = @"(?:authtoken=\S*.)";
            var regex = new Regex(pattern, RegexOptions.IgnoreCase);
            var matches = regex.Matches(petition.FilterString);

            return matches.Count == 1;
        }


        /// <summary>
        /// Implemented Business Rules for SAVE pettions
        /// </summary>
        /// <param name="petition">Requested information</param>
        /// <returns>Evaluation</returns>
        protected override bool ValidateSave(ReadWriteBusinessPetition<SessionDTO> petition)
        {
            return false;
        }

        /// <summary>
        /// Implemented Business Rules for SAVE pettions
        /// </summary>
        /// <param name="petition">Requested information</param>
        /// <returns>Evaluation</returns>
        protected bool ValidateSave(ReadWriteBusinessPetition<UserDTO> petition)
        {
            return true;
        }

        /// <summary>
        /// Implemented Business Rules for DELETE pettions
        /// </summary>
        /// <param name="petition">Requested information</param>
        /// <returns>Evaluation</returns>
        protected override bool ValidateDelete(ReadWriteBusinessPetition<SessionDTO> petition)
        {
            return false;
        }

        #endregion
    }
}