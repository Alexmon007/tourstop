using System.Linq;
using Common.Enums;
using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DataAccess.Tests
{
    public class BaseRepository_Tests
    {
        [Fact]
        public void AddOrUpdate_NewUser_UserAdded()
        {
            var options = new DbContextOptionsBuilder<TourStopContext>()
                .UseInMemoryDatabase("Add_Test_TourStop_Db").Options;

            using (var context = new TourStopContext(options))
            {
                var repository = new UserRepository(context);
                var user = new User
                {
                    FirstName = "FirstName",
                    LastName = "LastName",
                    Email = "fooAdd@bar.com",
                    AddressId = 1,
                    LanguageCode = LanguageCode.EN,
                    Password = "passwod",
                    Phone = "64612358",
                    UserType = UserType.Promoter
                };

                repository.AddOrUpdate(user);
                repository.SaveChanges();
            }
            using (var context = new TourStopContext(options))
            {
                Assert.Equal(1, context.Users.Count());
                Assert.Equal("fooAdd@bar.com", context.Users.Single().Email);
            }
        }

        [Fact]
        public void AddOrUpdate_User_UserChanged()
        {
            var options = new DbContextOptionsBuilder<TourStopContext>()
                .UseInMemoryDatabase("Update_Test_TourStop_Db").Options;
            User existentUser;
            using (var context = new TourStopContext(options))
            {
                existentUser = context.Users.Add(new User {Email = "fooUpdate@bar.com", FirstName = "Update"}).Entity;
                context.SaveChanges();
            }

            using (var context = new TourStopContext(options))
            {
                var repository = new UserRepository(context);

                existentUser.FirstName = "Updated";
                repository.AddOrUpdate(existentUser);
                repository.SaveChanges();
            }

            using (var context = new TourStopContext(options))
            {
                Assert.Equal(1, context.Users.Count());
                Assert.Equal("Updated", context.Users.Single().FirstName);
            }
        }

        [Fact]
        public void Delete_User_UserDeleted()
        {
            var options = new DbContextOptionsBuilder<TourStopContext>()
                .UseInMemoryDatabase("Delete_Test_TourStop_Db").Options;
            int userId;
            using (var context = new TourStopContext(options))
            {
                userId = context.Users.Add(new User {Email = "fooDelete1@bar.com"}).Entity.Id;
                context.Users.Add(new User {Email = "fooDelete2@bar.com"});
                context.Users.Add(new User {Email = "fooDelete3@bar.com"});

                context.SaveChanges();
            }

            using (var context = new TourStopContext(options))
            {
                var repository = new UserRepository(context);

                repository.Remove(userId);
                repository.SaveChanges();
            }

            using (var context = new TourStopContext(options))
            {
                Assert.Equal(2, context.Users.Count());
                Assert.All(context.Users, x => Assert.NotEqual("fooDelete1@bar.com", x.Email));
            }
        }

        [Fact]
        public void GetAll_User_TwoRegistries()
        {
            var options = new DbContextOptionsBuilder<TourStopContext>()
                .UseInMemoryDatabase("GetAll_Test_TourStop_Db").Options;

            using (var context = new TourStopContext(options))
            {
                context.Add(new User {Email = "fooGetAll1@bar.com"});
                context.Add(new User {Email = "fooGetAll2@bar.com"});

                context.SaveChanges();
            }

            using (var context = new TourStopContext(options))
            {
                var repository = new UserRepository(context);
                Assert.Equal(2, repository.GetAll().Count());
            }
        }
    }
}