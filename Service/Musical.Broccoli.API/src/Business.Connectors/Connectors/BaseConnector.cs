using System;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using AutoMapper;
using Business.Connectors.Contracts;
using Business.Connectors.Petition;
using Business.Connectors.Response;
using Common.DTOs;
using Common.Exceptions;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories.Contracts;

namespace Business.Connectors
{
    /// <summary>
    /// This connects database to controller. One for each entity in the Database. All business logic
    /// </summary>
    /// <typeparam name="TDto">Data transfer object</typeparam>
    /// <typeparam name="TEntity">Database Entity</typeparam>
    public abstract class BaseConnector<TDto, TEntity> : IBaseConnector<TDto>
        where TDto : BaseDTO where TEntity : BaseEntity
    {
        #region Instance Properties

        protected readonly IBaseRepository<TEntity> Repository;
        protected readonly IMapper Mapper;

        #endregion

        #region Processing Functions
        /// <summary>
        /// Select the process through out the pettion type
        /// </summary>
        public Func<ReadBusinessPetition, BusinessResponse<TDto>> Get => ProcessGet;
        public Func<ReadWriteBusinessPetition<TDto>, BusinessResponse<TDto>> Save => ProcessSave;
        public Func<ReadWriteBusinessPetition<TDto>, BusinessResponse<TDto>> Delete => ProcessDelete;

        #endregion
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="repository">Database Repository</param>
        /// <param name="mapper">Configuration Mapper</param>
        protected BaseConnector(IBaseRepository<TEntity> repository, IMapper mapper)
        {
            Repository = repository;
            Mapper = mapper;
        }

        #region Petition Processing
        /// <summary>
        /// Procces GET Petitions
        /// </summary>
        /// <param name="petition">Requested information</param>
        /// <returns>Response with infomation</returns>
        protected BusinessResponse<TDto> ProcessGet(ReadBusinessPetition petition)
        {
            if (!Validate(petition, ValidateGet)) throw new AuthenticationException();

            var businessResponse = new BusinessResponse<TDto>();
            try
            {
                var data = Repository.GetQueryable().Where(petition.FilterString);
                businessResponse.Data = Mapper.Map<List<TDto>>(data);
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
        protected BusinessResponse<TDto> ProcessSave(ReadWriteBusinessPetition<TDto> petition)
        {
            if (!Validate(petition, ValidateSave)) throw new AuthenticationException();

            var businessResponse = new BusinessResponse<TDto>();

            try
            {
                var data = Mapper.Map<List<TEntity>>(petition.Data);
                var savedData = new List<TEntity>();

                data.ForEach(x => savedData.Add(Repository.AddOrUpdate(x)));
                Repository.SaveChanges();

                businessResponse.Data = Mapper.Map<List<TDto>>(savedData);
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
        protected BusinessResponse<TDto> ProcessDelete(ReadWriteBusinessPetition<TDto> petition)
        {
            if (!Validate(petition, ValidateDelete)) throw new AuthenticationException();

            var businessResponse = new BusinessResponse<TDto>();

            try
            {
                petition.Data.ForEach(x => Repository.Remove(x.Id));
                Repository.SaveChanges();
                businessResponse.IsSuccessful = true;
            }
            catch (Exception)
            {
                throw new InternalServerErrorException();
            }

            return businessResponse;
        }

        #endregion

        #region Validation Methods
        /// <summary>
        /// Execute Business Rules in Connector
        /// </summary>
        /// <typeparam name="T">DTO</typeparam>
        /// <param name="petition">Requested information</param>
        /// <param name="validator">Type of validation</param>
        /// <returns>Evaluation Result</returns>
        protected static bool Validate<T>(T petition, ValidatePetition<T> validator) where T : BusinessPetition
        {
            return validator.Invoke(petition);
        }
        /// <summary>
        /// Business Rules for GET pettions
        /// These are implemented in each Connector
        /// </summary>
        /// <param name="petition">Requested information</param>
        /// <returns>Evaluation</returns>
        protected abstract bool ValidateGet(ReadBusinessPetition petition);

        /// <summary>
        /// Business Rules for SAVE pettions
        /// These are implemented in each Connector
        /// </summary>
        /// <param name="petition">Requested information</param>
        /// <returns>Evaluation Result</returns>
        protected abstract bool ValidateSave(ReadWriteBusinessPetition<TDto> petition);

        /// <summary>
        /// Business Rules for DELETE pettions
        /// These are implemented in each Connector
        /// </summary>
        /// <param name="petition">Requested information</param>
        /// <returns>Evaluation Result</returns>
        protected abstract bool ValidateDelete(ReadWriteBusinessPetition<TDto> petition);

        #endregion

        public delegate bool ValidatePetition<in T>(T petition) where T : BusinessPetition;
    }
}