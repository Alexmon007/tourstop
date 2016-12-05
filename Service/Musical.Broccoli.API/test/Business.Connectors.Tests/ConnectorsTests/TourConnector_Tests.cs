using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Business.Connectors.Helpers;
using Business.Connectors.Petition;
using Common.DTOs;
using Common.Enums;
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

    public class TourConnector_Tests
    {
        #region Get

        [Fact]
        public void Get_NotFiltersNoUser_ThrowsAuthenticationException()
        {
            var mapperConfiguration = new MapperConfiguration(x => x.AddProfile(new AutoMapperConfiguration()));
            var mapper = mapperConfiguration.CreateMapper();
            var repositoryMock = new Mock<ITourRepository>();

            var connector = new TourConnector(repositoryMock.Object, mapper);

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
                .UseInMemoryDatabase("Tour_Get_TourStop_Db")
                .Options;

            //Add some Registries
            using (var context = new TourStopContext(options))
            {
                context.Add(new Tour
                {
                    Title = "Tour1",
                    MaxReservation = 10,
                    Status = true,
                    DateCreated = DateTime.Now,
                    User = new User {Email = "foo1@bar.com"}
                });
                context.Add(new Tour
                {
                    Title = "Tour2",
                    MaxReservation = 10,
                    Status = true,
                    DateCreated = DateTime.Now,
                    User = new User {Email = "foo2@bar.com"}
                });
                context.Add(new Tour
                {
                    Title = "Tour3",
                    MaxReservation = 10,
                    Status = true,
                    DateCreated = DateTime.Now,
                    User = new User {Email = "foo3@bar.com"}
                });
                context.SaveChanges();
            }

            //Get Registries
            using (var context = new TourStopContext(options))
            {
                var repository = new TourRepository(context);

                var connector = new TourConnector(repository, mapper);

                var petition = new ReadBusinessPetition
                {
                    Action = PetitionAction.Read,
                    FilterString = "Title = \"Tour1\"",
                    RequestingUser = new UserDTO
                    {
                        Id = 1
                    }
                };

                var result = connector.Get(petition).Data;

                Assert.Equal(1, result.Count);
                Assert.All(result, x => Assert.Equal(x.Title, "Tour1"));
            }
        }

        #endregion

        #region Save

        [Fact]
        public void Save_ValidDataUser_NoException()
        {
            var mapperConfiguration = new MapperConfiguration(x => x.AddProfile(new AutoMapperConfiguration()));
            var mapper = mapperConfiguration.CreateMapper();
            var options = new DbContextOptionsBuilder<TourStopContext>()
                .UseInMemoryDatabase("Tour_SaveNoUser_TourStop_Db")
                .Options;

            using (var context = new TourStopContext(options))
            {
                var user = new User {Email = "foo1@bar.com", UserType = UserType.Promoter};
                context.Add(user);
                context.SaveChanges();
                var repository = new TourRepository(context);
                var connector = new TourConnector(repository, mapper);

                var petition = new ReadWriteBusinessPetition<TourDTO>
                {
                    Action = PetitionAction.ReadWrite,
                    Data = new List<TourDTO>
                    {
                        new TourDTO
                        {
                            Title = "Tour1",
                            MaxReservation = 10,
                            Status = true,
                            DateCreated = DateTime.Now,
                            UserId = user.Id
                        }
                    },
                    RequestingUser = new UserDTO {Id = user.Id, UserType = user.UserType}
                };
                connector.Save(petition);
            }
            using (var context = new TourStopContext(options))
            {
                Assert.Equal(1, context.Tours.Count());
            }
        }

        [Fact]
        public void Save_ValidDataNoUser_ThrowsAuthenticationException()
        {
            var mapperConfiguration = new MapperConfiguration(x => x.AddProfile(new AutoMapperConfiguration()));
            var mapper = mapperConfiguration.CreateMapper();

            var repositoryMock = new Mock<ITourRepository>();
            var connector = new TourConnector(repositoryMock.Object, mapper);

            var petition = new ReadWriteBusinessPetition<TourDTO>
            {
                Action = PetitionAction.ReadWrite,
                Data = new List<TourDTO>
                {
                    new TourDTO
                    {
                        Title = "Tour1",
                        MaxReservation = 10,
                        Status = true,
                        DateCreated = DateTime.Now,
                        User = new UserDTO {Email = "foo1@bar.com"}
                    }
                }
            };

            Assert.Throws<AuthenticationException>(() => connector.Save(petition));
        }

        [Fact]
        public void Update_ValidDataValidUser_NoException()
        {
            var mapperConfiguration = new MapperConfiguration(x => x.AddProfile(new AutoMapperConfiguration()));
            var mapper = mapperConfiguration.CreateMapper();

            var options = new DbContextOptionsBuilder<TourStopContext>()
                .UseInMemoryDatabase("Tour_UpdateValid_TourStop_Db")
                .Options;

            Tour originalEntity;

            //Add some Registries
            using (var context = new TourStopContext(options))
            {
                originalEntity = context.Add(new Tour
                {
                    Title = "Tour1",
                    MaxReservation = 10,
                    Status = true,
                    DateCreated = DateTime.Now,
                    User = new User {Email = "foo1@bar.com"}
                }).Entity;
                context.Add(new Tour
                {
                    Title = "Tour2",
                    MaxReservation = 10,
                    Status = true,
                    DateCreated = DateTime.Now,
                    User = new User {Email = "foo2@bar.com"}
                });
                context.Add(new Tour
                {
                    Title = "Tour3",
                    MaxReservation = 10,
                    Status = true,
                    DateCreated = DateTime.Now,
                    User = new User {Email = "foo3@bar.com"}
                });
                context.SaveChanges();
            }

            var mappedDto = mapper.Map<TourDTO>(originalEntity);
            mappedDto.Title
                = "UpdatedTour";

            using (var context = new TourStopContext(options))
            {
                var repository = new TourRepository(context);
                var connector = new TourConnector(repository, mapper);

                var petition = new ReadWriteBusinessPetition<TourDTO>
                {
                    Data = new List<TourDTO>
                    {
                        mappedDto
                    },
                    RequestingUser = new UserDTO {Id = 1, UserType = UserType.Promoter}
                };

                connector.Save(petition);
            }

            using (var context = new TourStopContext(options))
            {
                Assert.Equal(3, context.Tours.Count());
                Assert.Equal(mappedDto.Title, context.Tours.Single(x => x.Id == 1).Title);
            }
        }

        [Fact]
        public void Update_ValidDataNoUser_ThrowsAuthenticationException()
        {
            var mapperConfiguration = new MapperConfiguration(x => x.AddProfile(new AutoMapperConfiguration()));
            var mapper = mapperConfiguration.CreateMapper();

            var repositoryMock = new Mock<ITourRepository>();
            var connector = new TourConnector(repositoryMock.Object, mapper);

            var petition = new ReadWriteBusinessPetition<TourDTO>
            {
                Data = new List<TourDTO>
                {
                    new TourDTO
                    {
                        Id = 1,
                        MaxReservation = new Random().Next(),
                        ReservationPrice = new Random().Next(),
                        UserId = 1
                    }
                }
            };
            Assert.Throws<AuthenticationException>(() => connector.Save(petition));
        }

        [Fact]
        public void Update_ValidDataNoPromoterUser_ThrowsAuthenticationException()
        {
            var mapperConfiguration = new MapperConfiguration(x => x.AddProfile(new AutoMapperConfiguration()));
            var mapper = mapperConfiguration.CreateMapper();

            var repositoryMock = new Mock<ITourRepository>();
            var connector = new TourConnector(repositoryMock.Object, mapper);

            var petition = new ReadWriteBusinessPetition<TourDTO>
            {
                Data = new List<TourDTO>
                {
                    new TourDTO
                    {
                        Id = 1,
                        MaxReservation = new Random().Next(),
                        ReservationPrice = new Random().Next(),
                        UserId = 1
                    }
                },
                RequestingUser = new UserDTO {Id = 1, UserType = UserType.User}
            };

            Assert.Throws<AuthenticationException>(() => connector.Save(petition));
        }

        #endregion

        #region Delete

        [Fact]
        public
            void Delete_ValidDataNoUser_ThrowAuthenticationException()
        {
            var mapperConfiguration = new MapperConfiguration(x => x.AddProfile(new AutoMapperConfiguration()));
            var mapper = mapperConfiguration.CreateMapper();

            var repository = new Mock<ITourRepository>().Object;
            var connector = new TourConnector(repository, mapper);

            var petition = new ReadWriteBusinessPetition<TourDTO>
            {
                Action = PetitionAction.Delete,
                Data = new List<TourDTO>
                {
                    new TourDTO
                    {
                        Id = 1
                    }
                }
            };

            Assert.Throws<AuthenticationException>(() => connector.Delete(petition));
        }

        [Fact]
        public
            void Delete_ValidDataValidUser_NoException()
        {
            var mapperConfiguration = new MapperConfiguration(x => x.AddProfile(new AutoMapperConfiguration()));
            var mapper = mapperConfiguration.CreateMapper();

            var options = new DbContextOptionsBuilder<TourStopContext>()
                .UseInMemoryDatabase("Tour_DeleteValidUser_TourStop_Db")
                .Options;

            int entityId, userId;

            //Add some Registries
            using (var context = new TourStopContext(options))
            {
                var user = new User {Email = "foo1@bar.com", UserType = UserType.Promoter};
                userId = context.Add(user).Entity.Id;
                entityId = context.Add(new Tour
                {
                    Title = "Tour1",
                    MaxReservation = 10,
                    Status = true,
                    DateCreated = DateTime.Now,
                    User = user
                }).Entity.Id;
                context.Add(new Tour
                {
                    Title = "Tour2",
                    MaxReservation = 10,
                    Status = true,
                    DateCreated = DateTime.Now,
                    User = new User {Email = "foo2@bar.com"}
                });
                context.Add(new Tour
                {
                    Title = "Tour3",
                    MaxReservation = 10,
                    Status = true,
                    DateCreated = DateTime.Now,
                    User = new User {Email = "foo3@bar.com"}
                });
                context.SaveChanges();
            }
            using (var context = new TourStopContext(options))
            {
                var repository = new TourRepository(context);
                var connector = new TourConnector(repository, mapper);

                var petition = new ReadWriteBusinessPetition<TourDTO>
                {
                    Action = PetitionAction.Delete,
                    Data = new List<TourDTO>
                    {
                        new TourDTO
                        {
                            Id = entityId,
                            UserId = userId
                        }
                    },
                    RequestingUser = new UserDTO
                    {
                        Id = userId
                    }
                };

                connector.Delete(petition);
            }
            using (var context = new TourStopContext(options))
            {
                Assert.Equal(2, context.Tours.Count());
                Assert.All(context.Tours, x => Assert.NotEqual("Tour1", x.Title));
            }
        }

        [Fact]
        public void Delete_ValidDataNoCorrespondingUser_ThrowsAuthenticationException()
        {
            var mapperConfiguration = new MapperConfiguration(x => x.AddProfile(new AutoMapperConfiguration()));
            var mapper = mapperConfiguration.CreateMapper();

            var repository = new Mock<ITourRepository>().Object;
            var connector = new TourConnector(repository, mapper);

            var petition = new ReadWriteBusinessPetition<TourDTO>
            {
                Action = PetitionAction.Delete,
                Data = new List<TourDTO>
                {
                    new TourDTO
                    {
                        Id = 1,
                        UserId = 1
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

            var repository = new Mock<ITourRepository>().Object;
            var connector = new TourConnector(repository, mapper);

            var petition = new ReadWriteBusinessPetition<TourDTO>
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