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
    public class OrderConnector_Tests
    {
        #region Get

        [Fact]
        public void Get_NoFiltersNoUser_ThrowsAuthenticationException()
        {
            var mapperConfiguration = new MapperConfiguration(x => x.AddProfile(new AutoMapperConfiguration()));
            var mapper = mapperConfiguration.CreateMapper();
            var repositoryMock = new Mock<IOrderRepository>();

            var connector = new OrderConnector(repositoryMock.Object, mapper);

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
                .UseInMemoryDatabase("Get_TourStop_Db")
                .Options;

            int userId;
            //Add some Registries
            using (var context = new TourStopContext(options))
            {
                var user1 = new User { Email = "foo1@foo.com" };
                var user2 = new User { Email = "foo2@foo.com" };
                var tour = new Tour { Title = "Tour1", MaxReservation = 10, Status = true, DateCreated = System.DateTime.Now, User = new User { Email = "fooTour@bar.com" } };
                var reservation1 = new Reservation { User = user1, Tour = tour };
                var reservation2 = new Reservation { User = user2, Tour = tour };
                context.Add(new Order
                {
                    DateCreated = DateTime.Now,
                    TotalAmount = 25,
                    PaymentType = Common.Enums.PaymentType.Cash,
                    User = user1,
                    Reservations = new List<Reservation> { reservation1 },
                    Movements = new List<Movement> { new Movement { MovementType = Common.Enums.MovementType.Submitted, Reservation = reservation1 } }
                });
                context.Add(new Order
                {
                    DateCreated = DateTime.Now,
                    TotalAmount = 25,
                    PaymentType = Common.Enums.PaymentType.Cash,
                    User = user2,
                    Reservations = new List<Reservation> { reservation2 },
                    Movements = new List<Movement> { new Movement { MovementType = Common.Enums.MovementType.Submitted, Reservation = reservation2 } }
                });
                context.SaveChanges();
                userId = user1.Id;
            }

            //Get Registries
            using (var context = new TourStopContext(options))
            {
                var repository = new OrderRepository(context);

                var connector = new OrderConnector(repository, mapper);

                var petition = new ReadBusinessPetition
                {
                    Action = PetitionAction.Read,
                    FilterString =string.Format("UserId = {0}",userId),
                    RequestingUser = new UserDTO
                    {
                        Id = userId
                    }
                };

                var result = connector.Get(petition).Data;

                Assert.Equal(1, result.Count);
                Assert.All(result, x => Assert.Equal(x.UserId, userId));
            }
        }

        [Fact]
        public void Get_InvalidFilteredValidUser_FilteredUsers()
        {
            var mapperConfiguration = new MapperConfiguration(x => x.AddProfile(new AutoMapperConfiguration()));
            var mapper = mapperConfiguration.CreateMapper();
            var options = new DbContextOptionsBuilder<TourStopContext>()
                .UseInMemoryDatabase("Get_TourStop_Db")
                .Options;

            //Add some Registries
            using (var context = new TourStopContext(options))
            {
                var user1 = new User { Email = "foo1@foo.com" };
                var user2 = new User { Email = "foo2@foo.com" };
                var tour = new Tour { Title = "Tour1", MaxReservation = 10, Status = true, DateCreated = System.DateTime.Now, User = new User { Email = "fooTour@bar.com" } };
                var reservation1 = new Reservation { User = user1, Tour = tour };
                var reservation2 = new Reservation { User = user2, Tour = tour };
                context.Add(new Order
                {
                    DateCreated = DateTime.Now,
                    TotalAmount = 25,
                    PaymentType = Common.Enums.PaymentType.Cash,
                    User = user1,
                    Reservations = new List<Reservation> { reservation1 },
                    Movements = new List<Movement> { new Movement { MovementType = Common.Enums.MovementType.Submitted, Reservation = reservation1 } }
                });
                context.Add(new Order
                {
                    DateCreated = DateTime.Now,
                    TotalAmount = 25,
                    PaymentType = Common.Enums.PaymentType.Cash,
                    User = user2,
                    Reservations = new List<Reservation> { reservation2 },
                    Movements = new List<Movement> { new Movement { MovementType = Common.Enums.MovementType.Submitted, Reservation = reservation2 } }
                });
                context.SaveChanges();
            }

            //Get Registries
            using (var context = new TourStopContext(options))
            {
                var repository = new OrderRepository(context);

                var connector = new OrderConnector(repository, mapper);

                var petition = new ReadBusinessPetition
                {
                    Action = PetitionAction.Read,
                    FilterString = "UserId = 1 Or User = 2",
                    RequestingUser = new UserDTO
                    {
                        Id = 1
                    }
                };

                Assert.Throws<AuthenticationException>(() => connector.Get(petition));
            }
        }

        #endregion

        #region Save

        [Fact]
        public void Save_ValidDataNoUser_Exception()
        {
            var mapperConfiguration = new MapperConfiguration(x => x.AddProfile(new AutoMapperConfiguration()));
            var mapper = mapperConfiguration.CreateMapper();
            var options = new DbContextOptionsBuilder<TourStopContext>()
                .UseInMemoryDatabase("SaveNoUser_TourStop_Db")
                .Options;


            int userId, tourId;
            using (var context = new TourStopContext(options))
            {
                userId = context.Add(new User { Email = "foo1@foo.com" }).Entity.Id;
                tourId = context.Add(new Tour { Title = "Tour1", MaxReservation = 10, Status = true, DateCreated = System.DateTime.Now, User = new User { Email = "fooTour@bar.com" } }).Entity.Id;
                var repository = new OrderRepository(context);
                var connector = new OrderConnector(repository, mapper);
                var reservation1 = new ReservationDTO { UserId = userId, TourId = tourId };
                var petition = new ReadWriteBusinessPetition<OrderDTO>
                {
                    Action = PetitionAction.ReadWrite,
                    Data = new List<OrderDTO>
                    {
                        new OrderDTO
                    {
                    DateCreated = DateTime.Now,
                    TotalAmount = 25,
                    PaymentType = Common.Enums.PaymentType.Cash,
                    UserId = userId,
                    Reservations = new List<ReservationDTO> { reservation1 },
                    Movements= new List<MovementDTO> { new MovementDTO { MovementType=Common.Enums.MovementType.Submitted,Reservation= reservation1 } }
                    }
                    }
                };
                Assert.Throws<AuthenticationException>(() => connector.Save(petition));
                
            }
        }

        [Fact]
        public void Save_ValidDataUser_NoException()
        {
            var mapperConfiguration = new MapperConfiguration(x => x.AddProfile(new AutoMapperConfiguration()));
            var mapper = mapperConfiguration.CreateMapper();
            var options = new DbContextOptionsBuilder<TourStopContext>()
                .UseInMemoryDatabase("SaveNoUser_TourStop_Db")
                .Options;


            int userId, tourId;
            using (var context = new TourStopContext(options))
            {
                userId = context.Add(new User { Email = "foo1@foo.com" }).Entity.Id;
                tourId = context.Add(new Tour { Title = "Tour1", MaxReservation = 10, Status = true, DateCreated = System.DateTime.Now, User = new User { Email = "fooTour@bar.com" } }).Entity.Id;
                var repository = new OrderRepository(context);
                var connector = new OrderConnector(repository, mapper);
                var reservation1 = new ReservationDTO { UserId = userId, TourId = tourId };
                var petition = new ReadWriteBusinessPetition<OrderDTO>
                {
                    Action = PetitionAction.ReadWrite,
                    Data = new List<OrderDTO>
                    {
                        new OrderDTO
                    {
                    DateCreated = DateTime.Now,
                    TotalAmount = 25,
                    PaymentType = Common.Enums.PaymentType.Cash,
                    UserId = userId,
                    Reservations = new List<ReservationDTO> { reservation1 },
                    Movements= new List<MovementDTO> { new MovementDTO { MovementType=Common.Enums.MovementType.Submitted,Reservation= reservation1 } }
                    }
                    },
                    RequestingUser= new UserDTO { Id=1}
                };
                
                connector.Save(petition);
            }

            using (var context = new TourStopContext(options))
            {
                Assert.Equal(1, context.Orders.Count());
                Assert.Equal(userId, context.Orders.Single().UserId);
            }
        }

        [Fact]
        public void Update_ValidDataValidUser_NoException()
        {
            var mapperConfiguration = new MapperConfiguration(x => x.AddProfile(new AutoMapperConfiguration()));
            var mapper = mapperConfiguration.CreateMapper();
            var options = new DbContextOptionsBuilder<TourStopContext>()
                .UseInMemoryDatabase("UpdateValid_TourStop_Db")
                .Options;

            Order originalEntity;
            int userId, tourId;
            //Add some Registries
            using (var context = new TourStopContext(options))
            {
                var user1 = new User { Email = "foo1@foo.com" };
                var user2 = new User { Email = "foo2@foo.com" };
                var tour = new Tour { Title = "Tour1", MaxReservation = 10, Status = true, DateCreated = System.DateTime.Now, User = new User { Email = "fooTour@bar.com" } };
                var reservation1 = new Reservation { User = user1, Tour = tour };
                var reservation2 = new Reservation { User = user2, Tour = tour };
                originalEntity=context.Add(new Order
                {
                    DateCreated = DateTime.Now,
                    TotalAmount = 25,
                    PaymentType = Common.Enums.PaymentType.Cash,
                    User = user1,
                    Reservations = new List<Reservation> { reservation1,reservation2},
                    Movements = new List<Movement> { new Movement { MovementType = Common.Enums.MovementType.Submitted, Reservation = reservation1 } }
                }).Entity;
                context.SaveChanges();
                userId = user1.Id;
                tourId = tour.Id;
            }

            var mappedDto = mapper.Map<OrderDTO>(originalEntity);
            mappedDto.Movements.Add(new MovementDTO { OrderId = mappedDto.Id, Reservation = mappedDto.Reservations.Last(),MovementType=Common.Enums.MovementType.Subtraction});
            mappedDto.Reservations.Remove(mappedDto.Reservations.Last());

            using (var context = new TourStopContext(options))
            {
                var repository = new OrderRepository(context);
                var connector = new OrderConnector(repository, mapper);

                var petition = new ReadWriteBusinessPetition<OrderDTO>
                {
                    Data = new List<OrderDTO>
                    {
                        mappedDto
                    },
                    RequestingUser = new UserDTO { Id= userId }
                };

                connector.Save(petition);
            }

            using (var context = new TourStopContext(options))
            {
                Assert.Equal(mappedDto.Reservations.Count, context.Orders.Single(x => x.Id ==1).Reservations.Count);
            }
        }

        [Fact]
        public void Update_ValidDataNoUser_ThrowsAuthenticationException()
        {
            var mapperConfiguration = new MapperConfiguration(x => x.AddProfile(new AutoMapperConfiguration()));
            var mapper = mapperConfiguration.CreateMapper();
            var options = new DbContextOptionsBuilder<TourStopContext>()
                .UseInMemoryDatabase("UpdateValid_TourStop_Db")
                .Options;

            Order originalEntity;
            int userId, tourId;
            //Add some Registries
            using (var context = new TourStopContext(options))
            {
                var user1 = new User { Email = "foo1@foo.com" };
                var user2 = new User { Email = "foo2@foo.com" };
                var tour = new Tour { Title = "Tour1", MaxReservation = 10, Status = true, DateCreated = System.DateTime.Now, User = new User { Email = "fooTour@bar.com" } };
                var reservation1 = new Reservation { User = user1, Tour = tour };
                var reservation2 = new Reservation { User = user2, Tour = tour };
                originalEntity = context.Add(new Order
                {
                    DateCreated = DateTime.Now,
                    TotalAmount = 25,
                    PaymentType = Common.Enums.PaymentType.Cash,
                    User = user1,
                    Reservations = new List<Reservation> { reservation1, reservation2 },
                    Movements = new List<Movement> { new Movement { MovementType = Common.Enums.MovementType.Submitted, Reservation = reservation1 } }
                }).Entity;
                context.SaveChanges();
                userId = user1.Id;
                tourId = tour.Id;
            }

            var mappedDto = mapper.Map<OrderDTO>(originalEntity);
            mappedDto.Movements.Add(new MovementDTO { OrderId = mappedDto.Id, Reservation = mappedDto.Reservations.Last(), MovementType = Common.Enums.MovementType.Subtraction });
            mappedDto.Reservations.Remove(mappedDto.Reservations.Last());

            using (var context = new TourStopContext(options))
            {
                var repository = new OrderRepository(context);
                var connector = new OrderConnector(repository, mapper);

                var petition = new ReadWriteBusinessPetition<OrderDTO>
                {
                    Data = new List<OrderDTO>
                    {
                        mappedDto
                    }
                };
                Assert.Throws<AuthenticationException>(() => connector.Save(petition));
            }
        }

        [Fact]
        public void Update_ValidDataNoCorrespondingUser_ThrowsAuthenticationException()
        {
            var mapperConfiguration = new MapperConfiguration(x => x.AddProfile(new AutoMapperConfiguration()));
            var mapper = mapperConfiguration.CreateMapper();
            var options = new DbContextOptionsBuilder<TourStopContext>()
                .UseInMemoryDatabase("UpdateValid_TourStop_Db")
                .Options;

            Order originalEntity;
            int userId, tourId;
            //Add some Registries
            using (var context = new TourStopContext(options))
            {
                var user1 = new User { Email = "foo1@foo.com" };
                var user2 = new User { Email = "foo2@foo.com" };
                var tour = new Tour { Title = "Tour1", MaxReservation = 10, Status = true, DateCreated = System.DateTime.Now, User = new User { Email = "fooTour@bar.com" } };
                var reservation1 = new Reservation { User = user1, Tour = tour };
                var reservation2 = new Reservation { User = user2, Tour = tour };
                originalEntity = context.Add(new Order
                {
                    DateCreated = DateTime.Now,
                    TotalAmount = 25,
                    PaymentType = Common.Enums.PaymentType.Cash,
                    User = user1,
                    Reservations = new List<Reservation> { reservation1, reservation2 },
                    Movements = new List<Movement> { new Movement { MovementType = Common.Enums.MovementType.Submitted, Reservation = reservation1 } }
                }).Entity;
                context.SaveChanges();
                userId = user1.Id;
                tourId = tour.Id;
            }

            var mappedDto = mapper.Map<OrderDTO>(originalEntity);
            mappedDto.Movements.Add(new MovementDTO { OrderId = mappedDto.Id, Reservation = mappedDto.Reservations.Last(), MovementType = Common.Enums.MovementType.Subtraction });
            mappedDto.Reservations.Remove(mappedDto.Reservations.Last());

            using (var context = new TourStopContext(options))
            {
                var repository = new OrderRepository(context);
                var connector = new OrderConnector(repository, mapper);

                var petition = new ReadWriteBusinessPetition<OrderDTO>
                {
                    Data = new List<OrderDTO>
                    {
                        mappedDto
                    },
                    RequestingUser = new UserDTO { Id = 35 }
                };

            Assert.Throws<AuthenticationException>(() => connector.Save(petition));
            }
        }

        #endregion

        #region Delete

        [Fact]
        public void Delete_ValidDataNoUser_ThrowAuthenticationException()
        {
            var mapperConfiguration = new MapperConfiguration(x => x.AddProfile(new AutoMapperConfiguration()));
            var mapper = mapperConfiguration.CreateMapper();

            var repository = new Mock<IOrderRepository>().Object;
            var connector = new OrderConnector(repository, mapper);

            var petition = new ReadWriteBusinessPetition<OrderDTO>
            {
                Action = PetitionAction.Delete,
                Data = new List<OrderDTO>
                {
                    new OrderDTO
                    {
                        Id = 1
                    }
                }
            };

            Assert.Throws<AuthenticationException>(() => connector.Delete(petition));
        }

        [Fact]
        public void Delete_ValidDataValidUser_NoException()
        {
            var mapperConfiguration = new MapperConfiguration(x => x.AddProfile(new AutoMapperConfiguration()));
            var mapper = mapperConfiguration.CreateMapper();
            var options = new DbContextOptionsBuilder<TourStopContext>()
                .UseInMemoryDatabase("DeleteValidUser_TourStop_Db")
                .Options;

            int entityId, userId;

            //Add some Registries
            using (var context = new TourStopContext(options))
            {
                var user1 = new User { Email = "foo1@foo.com" };
                var user2 = new User { Email = "foo2@foo.com" };
                var tour = new Tour { Title = "Tour1", MaxReservation = 10, Status = true, DateCreated = System.DateTime.Now, User = new User { Email = "fooTour@bar.com" } };
                var reservation1 = new Reservation { User = user1, Tour = tour };
                var reservation2 = new Reservation { User = user2, Tour = tour };
                entityId = context.Add(new Order
                {
                    DateCreated = DateTime.Now,
                    TotalAmount = 25,
                    PaymentType = Common.Enums.PaymentType.Cash,
                    User = user1,
                    Reservations = new List<Reservation> { reservation1 },
                    Movements = new List<Movement> { new Movement { MovementType = Common.Enums.MovementType.Submitted, Reservation = reservation1 } }
                }).Entity.Id ;
                context.Add(new Order
                {
                    DateCreated = DateTime.Now,
                    TotalAmount = 25,
                    PaymentType = Common.Enums.PaymentType.Cash,
                    User = user2,
                    Reservations = new List<Reservation> { reservation2 },
                    Movements = new List<Movement> { new Movement { MovementType = Common.Enums.MovementType.Submitted, Reservation = reservation2 } }
                });
                context.SaveChanges();
                userId = user1.Id;
            }
            using (var context = new TourStopContext(options))
            {
                var repository = new OrderRepository(context);
                var connector = new OrderConnector(repository, mapper);

                var petition = new ReadWriteBusinessPetition<OrderDTO>
                {
                    Action = PetitionAction.Delete,
                    Data = new List<OrderDTO>
                    {
                        new OrderDTO
                        {
                            Id = entityId,
                            UserId=userId
                        }
                    },
                    RequestingUser = new UserDTO
                    {
                        Id = userId,
                        UserType=Common.Enums.UserType.Promoter
                    }
                };

                Assert.Throws<AuthenticationException>(() => connector.Delete(petition));
            }
        }

        [Fact]
        public void Delete_ValidDataNoCorrespondingUser_ThrowsAuthenticationException()
        {
            var mapperConfiguration = new MapperConfiguration(x => x.AddProfile(new AutoMapperConfiguration()));
            var mapper = mapperConfiguration.CreateMapper();

            var repository = new Mock<IOrderRepository>().Object;
            var connector = new OrderConnector(repository, mapper);

            var petition = new ReadWriteBusinessPetition<OrderDTO>
            {
                Action = PetitionAction.Delete,
                Data = new List<OrderDTO>
                {
                    new OrderDTO
                    {
                        Id = 1,
                        UserId=1
                    }
                },
                RequestingUser = new UserDTO
                {
                    Id = 2,
                    UserType = Common.Enums.UserType.Promoter
                }
            };

            Assert.Throws<AuthenticationException>(() => connector.Delete(petition));
        }

        [Fact]
        public void Delete_NoDataValidUser_ThrowsAuthenticationException()
        {
            var mapperConfiguration = new MapperConfiguration(x => x.AddProfile(new AutoMapperConfiguration()));
            var mapper = mapperConfiguration.CreateMapper();

            var repository = new Mock<IOrderRepository>().Object;
            var connector = new OrderConnector(repository, mapper);

            var petition = new ReadWriteBusinessPetition<OrderDTO>
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
