using System;
using System.Collections.Generic;
using System.Linq;
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
using Xunit;

namespace Business.Connectors.Tests.ConnectorsTests
{
    /*
    TODO: Every method should be tested in the following ways:
        - NoFilter
        - NoUser
        - NoData
        Each with it's valid counterpart and combine them
    */

    public class UserConnector_Tests
    {
        #region Get

        [Fact]
        public void Get_NoFiltersNoUser_ThrowsAuthenticationException()
        {
            var mapperConfiguration = new MapperConfiguration(x => x.AddProfile(new AutoMapperConfiguration()));
            var mapper = mapperConfiguration.CreateMapper();
            var repositoryMock = new Mock<IUserRepository>();

            var connector = new UserConnector(repositoryMock.Object, mapper);

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
                .UseInMemoryDatabase("User_Get_TourStop_Db")
                .Options;

            //Add some Registries
            using (var context = new TourStopContext(options))
            {
                context.Add(new User {Email = "foo1@bar.com"});
                context.Add(new User {Email = "foo2@bar.com"});
                context.Add(new User {Email = "foo3@bar.com"});
                context.SaveChanges();
            }

            //Get Registries
            using (var context = new TourStopContext(options))
            {
                var repository = new UserRepository(context);

                var connector = new UserConnector(repository, mapper);

                var petition = new ReadBusinessPetition
                {
                    Action = PetitionAction.Read,
                    FilterString = "email = \"foo1@bar.com\"",
                    RequestingUser = new UserDTO
                    {
                        Id = 1
                    }
                };

                var result = connector.Get(petition).Data;

                Assert.Equal(1, result.Count);
                Assert.All(result, x => Assert.Equal(x.Email, "foo1@bar.com"));
            }
        }

        #endregion

        #region Save

        [Fact]
        public void Save_ValidDataNoUser_NoException()
        {
            var mapperConfiguration = new MapperConfiguration(x => x.AddProfile(new AutoMapperConfiguration()));
            var mapper = mapperConfiguration.CreateMapper();
            var options = new DbContextOptionsBuilder<TourStopContext>()
                .UseInMemoryDatabase("User_SaveNoUser_TourStop_Db")
                .Options;

            using (var context = new TourStopContext(options))
            {
                var repository = new UserRepository(context);
                var connector = new UserConnector(repository, mapper);

                var petition = new ReadWriteBusinessPetition<UserDTO>
                {
                    Action = PetitionAction.ReadWrite,
                    Data = new List<UserDTO>
                    {
                        new UserDTO {Email = "fooSaveValid@bar.com"}
                    }
                };

                connector.Save(petition);
            }

            using (var context = new TourStopContext(options))
            {
                Assert.Equal(1, context.Users.Count());
                Assert.Equal("fooSaveValid@bar.com", context.Users.Single().Email);
            }
        }

        [Fact]
        public void Update_ValidDataValidUser_NoException()
        {
            var mapperConfiguration = new MapperConfiguration(x => x.AddProfile(new AutoMapperConfiguration()));
            var mapper = mapperConfiguration.CreateMapper();
            var options = new DbContextOptionsBuilder<TourStopContext>()
                .UseInMemoryDatabase("User_UpdateValid_TourStop_Db")
                .Options;

            User originalEntity;

            //Add some Registries
            using (var context = new TourStopContext(options))
            {
                originalEntity = context.Add(new User {Email = "fooUpdate1@bar.com"}).Entity;
                context.Add(new User {Email = "fooUpdate2@bar.com"});
                context.Add(new User {Email = "fooUpdate3@bar.com"});
                context.SaveChanges();
            }

            var mappedDto = mapper.Map<UserDTO>(originalEntity);
            mappedDto.FirstName = "UpdatedName";

            using (var context = new TourStopContext(options))
            {
                var repository = new UserRepository(context);
                var connector = new UserConnector(repository, mapper);

                var petition = new ReadWriteBusinessPetition<UserDTO>
                {
                    Data = new List<UserDTO>
                    {
                        mappedDto
                    },
                    RequestingUser = mappedDto
                };

                connector.Save(petition);
            }

            using (var context = new TourStopContext(options))
            {
                Assert.Equal(3, context.Users.Count());
                Assert.Equal(mappedDto.FirstName, context.Users.Single(x => x.Email == "fooUpdate1@bar.com").FirstName);
            }
        }

        [Fact]
        public void Update_ValidDataNoUser_ThrowsAuthenticationException()
        {
            var mapperConfiguration = new MapperConfiguration(x => x.AddProfile(new AutoMapperConfiguration()));
            var mapper = mapperConfiguration.CreateMapper();
            var repositoryMock = new Mock<IUserRepository>();

            var connector = new UserConnector(repositoryMock.Object, mapper);

            var petition = new ReadWriteBusinessPetition<UserDTO>
            {
                Data = new List<UserDTO>
                {
                    new UserDTO
                    {
                        Id = 1,
                        FirstName = "FirstName",
                        LastName = "LastName",
                        Email = "fooUpdate1@bar.com"
                    }
                }
            };

            Assert.Throws<AuthenticationException>(() => connector.Save(petition));
        }

        [Fact]
        public void Update_ValidDataNoCorrespondingUser_ThrowsAuthenticationException()
        {
            var mapperConfiguration = new MapperConfiguration(x => x.AddProfile(new AutoMapperConfiguration()));
            var mapper = mapperConfiguration.CreateMapper();
            var repositoryMock = new Mock<IUserRepository>();
            var connector = new UserConnector(repositoryMock.Object, mapper);

            var petition = new ReadWriteBusinessPetition<UserDTO>
            {
                Data = new List<UserDTO>
                {
                    new UserDTO
                    {
                        Id = 1,
                        FirstName = "FirstName",
                        LastName = "LastName",
                        Email = "fooUpdate@bar.com"
                    }
                },
                RequestingUser = new UserDTO
                {
                    Id = new Random().Next()
                }
            };

            Assert.Throws<AuthenticationException>(() => connector.Save(petition));
        }

        #endregion

        #region Delete

        [Fact]
        public void Delete_ValidDataNoUser_ThrowAuthenticationException()
        {
            var mapperConfiguration = new MapperConfiguration(x => x.AddProfile(new AutoMapperConfiguration()));
            var mapper = mapperConfiguration.CreateMapper();

            var repository = new Mock<IUserRepository>().Object;
            var connector = new UserConnector(repository, mapper);

            var petition = new ReadWriteBusinessPetition<UserDTO>
            {
                Action = PetitionAction.Delete,
                Data = new List<UserDTO>
                {
                    new UserDTO
                    {
                        Id = 1
                    }
                }
            };

            Assert.Throws<AuthenticationException>
            (()
                    =>
                        connector.Delete
                            (petition)
            );
        }

        [Fact]
        public void Delete_ValidDataValidUser_NoException()
        {
            var mapperConfiguration = new MapperConfiguration(x => x.AddProfile(new AutoMapperConfiguration()));
            var mapper = mapperConfiguration.CreateMapper();

            var options = new DbContextOptionsBuilder<TourStopContext>()
                .UseInMemoryDatabase("User_DeleteValidUser_TourStop_Db")
                .Options;

            int entityId;

            //Add some Registries
            using
            (var context
                =
                new TourStopContext(options))
            {
                entityId = context.Add(new User {Email = "fooDelete1@bar.com"}).Entity.Id;
                context.Add(new User {Email = "fooDelete2@bar.com"});
                context.Add(new User {Email = "fooDelete3@bar.com"});
                context.SaveChanges();
            }
            using
            (var context =
                new
                    TourStopContext(options))
            {
                var repository = new UserRepository(context);
                var connector = new UserConnector(repository, mapper);

                var petition = new ReadWriteBusinessPetition<UserDTO>
                {
                    Action = PetitionAction.Delete,
                    Data = new List<UserDTO>
                    {
                        new UserDTO
                        {
                            Id = entityId
                        }
                    },
                    RequestingUser = new UserDTO
                    {
                        Id = entityId
                    }
                };

                connector.Delete
                    (petition);
            }
            using
            (var context =
                new
                    TourStopContext(options))
            {
                Assert.Equal
                (
                    2,
                    context.Users.Count
                        ()
                );
                Assert.All
                (context.Users, x
                    =>
                        Assert.NotEqual
                        (
                            "fooDelete1@bar.com",
                            x.Email
                        ));
            }
        }

        [Fact]
        public void Delete_ValidDataNoCorrespondingUser_ThrowsAuthenticationException()
        {
            var mapperConfiguration = new MapperConfiguration(x => x.AddProfile(new AutoMapperConfiguration()));
            var mapper = mapperConfiguration.CreateMapper();

            var repository = new Mock<IUserRepository>().Object;
            var connector = new UserConnector(repository, mapper);

            var petition = new ReadWriteBusinessPetition<UserDTO>
            {
                Action = PetitionAction.Delete,
                Data = new List<UserDTO>
                {
                    new UserDTO
                    {
                        Id = 1
                    }
                },
                RequestingUser = new UserDTO
                {
                    Id = 2
                }
            };

            Assert.Throws<AuthenticationException>(() => connector.Delete(petition));
        }

        [Fact]
        public void Delete_NoDataValidUser_ThrowsAuthenticationException()
        {
            var mapperConfiguration = new MapperConfiguration(x => x.AddProfile(new AutoMapperConfiguration()));
            var mapper = mapperConfiguration.CreateMapper();

            var repository = new Mock<IUserRepository>().Object;
            var connector = new UserConnector(repository, mapper);

            var petition = new ReadWriteBusinessPetition<UserDTO>
            {
                Action = PetitionAction.Delete,
                RequestingUser = new UserDTO
                {
                    Id = 1
                }
            };

            Assert.Throws<AuthenticationException>(() => connector.Delete(petition));
        }

        #endregion
    }
}