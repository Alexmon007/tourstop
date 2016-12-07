using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Context
{
    public sealed class TourStopContext : DbContext
    {
        /// <summary>
        /// This creates the connection between the models and the Database, if the Database does not exists, it creates a new one.
        /// </summary>
        /// <param name="options">Databse options</param>
        public TourStopContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }

        /// <summary>
        /// Database tables
        /// </summary>
        public DbSet<Address> Addresses { get; set; }
        public DbSet<CheckPoint> CheckPoints { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Movement> Movements { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<PaymentInfo> PaymentInfos { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Tour> Tours { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Session> Sessions { get; set; }

        /// <summary>
        /// Only update if there is any changes to the Db records.
        /// </summary>
        /// <returns></returns>
        public override int SaveChanges()
        {            
            ChangeTracker.DetectChanges();
            return base.SaveChanges();
        }

        /// <summary>
        /// Describe the table's relations and special attributes while creating the database.
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {    

            #region AddressConfig

            #endregion

            #region CheckpointCondig

            #endregion

            #region MessageConfig

            modelBuilder.Entity<Message>().HasMany(x => x.MessageHasRecievers);

            #endregion

            #region MessageHasRecieverConfig

            modelBuilder.Entity<MessageHasReciever>().HasKey(x => new {x.MessageId, x.RecieverId});
            modelBuilder.Entity<MessageHasReciever>()
                .HasOne(x => x.Message)
                .WithMany(x => x.MessageHasRecievers)
                .HasForeignKey(x => x.MessageId);
            modelBuilder.Entity<MessageHasReciever>()
                .HasOne(x => x.Reciever)
                .WithMany(x => x.Messages)
                .HasForeignKey(x => x.RecieverId);

            #endregion

            #region MovementCondig

            #endregion

            #region OrderConfig

            modelBuilder.Entity<Order>().HasMany(x => x.Reservations);

            #endregion

            #region PaymentInfoConfig

            //PENDING

            #endregion

            #region PromotionConfig

            #endregion

            #region RatingConfig

            #endregion

            #region ReservationConfig

            #endregion

            #region TourConfig

            modelBuilder.Entity<Tour>().HasMany(x => x.Ratings);
            modelBuilder.Entity<Tour>().HasMany(x => x.Reservations);

            #endregion

            #region UserConfig

            modelBuilder.Entity<User>().HasMany(x => x.Orders);
            modelBuilder.Entity<User>().HasMany(x => x.Ratings);
            modelBuilder.Entity<User>().HasMany(x => x.Reservations);
            modelBuilder.Entity<User>().HasMany(x => x.Tours);
            modelBuilder.Entity<User>().HasMany(x => x.Messages).WithOne(x => x.Reciever);
            modelBuilder.Entity<User>().HasAlternateKey(x => x.Email);

            #endregion

            #region SessionConfig

            modelBuilder.Entity<Session>().HasAlternateKey(x => x.AuthToken);

            #endregion
        }
    }
}