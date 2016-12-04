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
    public abstract class BaseConnector<TDto, TEntity> : IBaseConnector<TDto>
        where TDto : BaseDTO where TEntity : BaseEntity
    {
        #region Instance Properties

        protected readonly IBaseRepository<TEntity> Repository;
        protected readonly IMapper Mapper;

        #endregion

        #region Processing Functions

        public Func<ReadBusinessPetition, BusinessResponse<TDto>> Get => ProcessGet;
        public Func<ReadWriteBusinessPetition<TDto>, BusinessResponse<TDto>> Save => ProcessSave;
        public Func<ReadWriteBusinessPetition<TDto>, BusinessResponse<TDto>> Delete => ProcessDelete;

        #endregion

        protected BaseConnector(IBaseRepository<TEntity> repository, IMapper mapper)
        {
            Repository = repository;
            Mapper = mapper;
        }

        #region Petition Processing

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

        protected static bool Validate<T>(T petition, ValidatePetition<T> validator) where T : BusinessPetition
        {
            return validator.Invoke(petition);
        }

        protected abstract bool ValidateGet(ReadBusinessPetition petition);

        protected abstract bool ValidateSave(ReadWriteBusinessPetition<TDto> petition);

        protected abstract bool ValidateDelete(ReadWriteBusinessPetition<TDto> petition);

        #endregion

        public delegate bool ValidatePetition<in T>(T petition) where T : BusinessPetition;
    }
}