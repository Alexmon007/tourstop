using AutoMapper;
using Business.Connectors.Helpers;
using Business.Connectors.Petition;
using Common.DTOs;
using Common.Exceptions;
using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories;
using DataAccessLayer.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Business.Connectors.Tests.ConnectorsTests
{
    public class MessageConnector_Tests
    {
        #region Get

        [Fact]
        public void Get_NoFiltersNoUser_ThrowsAuthenticationException()
        {
            var mapperConfiguration = new MapperConfiguration(x => x.AddProfile(new AutoMapperConfiguration()));
            var mapper = mapperConfiguration.CreateMapper();
            var repositoryMock = new Mock<IMessageRepository>();

            var connector = new MessageConnector(repositoryMock.Object, mapper);

            var petition = new ReadBusinessPetition
            {
                Action = PetitionAction.Read
            };

            Assert.Throws<AuthenticationException>(() => connector.Get(petition));
        }

        [Fact]
        public void Get_FilteredValidUser_FilteredUsers()
        {
            var mapperConfiguration = new MapperConfiguration(x => x.AddProfile(new AutoMapperConfiguration()));
            var mapper = mapperConfiguration.CreateMapper();
            var options = new DbContextOptionsBuilder<TourStopContext>()
                .UseInMemoryDatabase("Message_Get_TourStop_Db")
                .Options;

            //Add some Registries
            using (var context = new TourStopContext(options))
            {
                var user1 = context.Add(new User { Email = "foo1@bar.com" }).Entity;
                var user2 = context.Add(new User { Email = "foo2@bar.com" }).Entity;
                context.Add(new Message { Content = "Test1", Sender = user1, MessageHasRecievers = new List<MessageHasReciever> { new MessageHasReciever { Reciever = user2 } } });
                context.SaveChanges();
            }

            //Get Registries
            using (var context = new TourStopContext(options))
            {
                var repository = new MessageRepository(context);

                var connector = new MessageConnector(repository, mapper);

                var petition = new ReadBusinessPetition
                {
                    Action = PetitionAction.Read,
                    FilterString = "SenderId = 1",
                    RequestingUser = new UserDTO
                    {
                        Id = 1
                    }
                };

                var result = connector.Get(petition).Data;
                

                Assert.Equal(1, result.Count);
                Assert.All(result, x => Assert.Equal(x.Content, "Test1"));
            }
        }

        #endregion

        #region Save (YOU CANNOT UPDATE A MESSAGE)

        [Fact]
        public void Save_ValidDataNoUser_NoException()
        {
            var mapperConfiguration = new MapperConfiguration(x => x.AddProfile(new AutoMapperConfiguration()));
            var mapper = mapperConfiguration.CreateMapper();
            var options = new DbContextOptionsBuilder<TourStopContext>()
                .UseInMemoryDatabase("Message_SaveNoUser_TourStop_Db")
                .Options;

            using (var context = new TourStopContext(options))
            {
                var repository = new MessageRepository(context);
                var connector = new MessageConnector(repository, mapper);

                var petition = new ReadWriteBusinessPetition<MessageDTO>
                {
                    Action = PetitionAction.ReadWrite,
                    Data = new List<MessageDTO>
                    {
                        new MessageDTO { Content = "Test1", Sender = new UserDTO { Email= "foo@bar.com" }, Receivers= new List<UserDTO> { new UserDTO { Email= "fooSaveValid@bar.com" } } }
                    },
                    RequestingUser = new UserDTO { Id = 1 }

                };

                connector.Save(petition);
            }

            using (var context = new TourStopContext(options))
            {
                Assert.Equal(1, context.Messages.Count());
                Assert.Equal("Test1", context.Messages.Single().Content);
            }
        }

        [Fact]
        public void Update_ValidDataValidUser_ThrowsAuthenticationException() 
        {
            var mapperConfiguration = new MapperConfiguration(x => x.AddProfile(new AutoMapperConfiguration()));
            var mapper = mapperConfiguration.CreateMapper();
            var repositoryMock = new Mock<IMessageRepository>();
            var connector = new MessageConnector(repositoryMock.Object, mapper);

            var petition = new ReadWriteBusinessPetition<MessageDTO>
            {
                Action = PetitionAction.ReadWrite,
                Data = new List<MessageDTO>
                    {
                        new MessageDTO {Id=1, Content = "Test1", Sender = new UserDTO { Email= "foo@bar.com" }, Receivers= new List<UserDTO> { new UserDTO { Email= "fooSaveValid@bar.com" } } }
                    },
                RequestingUser = new UserDTO { Id = 1 }

            };

            Assert.Throws<AuthenticationException>(() => connector.Save(petition));
        }

        #endregion

        #region Delete (YOU CANNOT DELETE A MESSAGE)

        [Fact]
        public void Delete_ValidDataValidUser_ThrowAuthenticationException()
        {
            var mapperConfiguration = new MapperConfiguration(x => x.AddProfile(new AutoMapperConfiguration()));
            var mapper = mapperConfiguration.CreateMapper();

            var repository = new Mock<IMessageRepository>().Object;
            var connector = new MessageConnector(repository, mapper);

            var petition = new ReadWriteBusinessPetition<MessageDTO>
            {
                Action = PetitionAction.Delete,
                Data = new List<MessageDTO>
                    {
                        new MessageDTO { Content = "Test1", Sender = new UserDTO { Email= "foo@bar.com" }, Receivers= new List<UserDTO> { new UserDTO { Email= "fooSaveValid@bar.com" } } }
                    },
                RequestingUser = new UserDTO { Id = 1 }

            };

            Assert.Throws<AuthenticationException>(()=>connector.Delete(petition));
        }

        #endregion
    }
}
