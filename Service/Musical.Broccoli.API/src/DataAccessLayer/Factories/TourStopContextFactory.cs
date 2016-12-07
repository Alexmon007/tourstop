using Common.AppSettings;
using DataAccessLayer.Context;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Factories
{
    public static class TourStopContextFactory
    {
        /// <summary>
        /// Creates an instance of Context with specific options
        /// </summary>
        /// <returns></returns>
        public static TourStopContext Create()
        {
            var optionsBuilder = new DbContextOptionsBuilder<TourStopContext>();
            optionsBuilder.UseMySql(AppSettings.ConnectionString);
            var context = new TourStopContext(optionsBuilder.Options);
            context.Database.EnsureCreated();
            return context;
        }
    }
}