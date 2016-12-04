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
    public class SessionConnector : BaseConnector<SessionDTO, Session>, ISessionConnector
    {
        protected new ISessionRepository Repository => (ISessionRepository) base.Repository;
        private readonly IUserRepository _userRepository;

        public SessionConnector(ISessionRepository repository, IMapper mapper, IUserRepository userRepository)
            : base(repository, mapper)
        {
            _userRepository = userRepository;
        }

        #region Processing Functions

        public new Func<ReadBusinessPetition, BusinessResponse<SessionDTO>> Get => ProcessGet;
        public new Func<ReadWriteBusinessPetition<UserDTO>, BusinessResponse<SessionDTO>> Save => ProcessSave;
        public new Func<ReadWriteBusinessPetition<SessionDTO>, BusinessResponse<SessionDTO>> Delete => ProcessDelete;

        #endregion

        #region Petition Processing

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

        private new BusinessResponse<SessionDTO> ProcessDelete(ReadWriteBusinessPetition<SessionDTO> petition)
        {
            if (!Validate(petition, ValidateDelete)) throw new AuthenticationException();

            throw new InternalServerErrorException();
        }

        #endregion

        #region Validate Methods

        protected override bool ValidateGet(ReadBusinessPetition petition)
        {
            const string pattern = @"(?:authtoken=\S*.)";
            var regex = new Regex(pattern, RegexOptions.IgnoreCase);
            var matches = regex.Matches(petition.FilterString);

            return matches.Count == 1;
        }

        protected override bool ValidateSave(ReadWriteBusinessPetition<SessionDTO> petition)
        {
            return false;
        }

        protected bool ValidateSave(ReadWriteBusinessPetition<UserDTO> petition)
        {
            return true;
        }

        protected override bool ValidateDelete(ReadWriteBusinessPetition<SessionDTO> petition)
        {
            return false;
        }

        #endregion
    }
}