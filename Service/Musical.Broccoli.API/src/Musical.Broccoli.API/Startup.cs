using AutoMapper;
using Business.Connectors;
using Business.Connectors.Contracts;
using Business.Connectors.Helpers;
using Business.Handlers.Authentication;
using Business.Handlers.Authentication.contracts;
using Business.Handlers.Handlers;
using Business.Handlers.Handlers.contracts;
using DataAccessLayer.Context;
using DataAccessLayer.Repositories;
using DataAccessLayer.Repositories.Contracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Musical.Broccoli.API
{
    public class Startup
    {
        private readonly MapperConfiguration _mapperConfiguration;

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true);

            if (env.IsEnvironment("Development"))
            {
                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(true);
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();

            _mapperConfiguration = new MapperConfiguration(x => x.AddProfile(new AutoMapperConfiguration()));
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddApplicationInsightsTelemetry(Configuration);

            services.AddMvc();

            #region Dependency Injection

            #region Request Handlers

            services.AddScoped<IMessageRequestHandler, MessageRequestHandler>();
            services.AddScoped<IOrderRequestHandler, OrderRequestHandler>();
            services.AddScoped<ITourRequestHandler, TourRequestHandler>();
            services.AddScoped<IUserRequestHandler, UserRequestHandler>();

            #endregion

            //RequestAuthenticator
            services.AddTransient<IRequestAuthenticator, RequestAuthenticator>();

            #region Connectors

            services.AddScoped<IAddressConnector, AddressConnector>();
            services.AddScoped<ICheckPointConnector, CheckPointConnector>();
            services.AddScoped<IMessageConnector, MessageConnector>();
            services.AddScoped<IMovementConnector, MovementConnector>();
            services.AddScoped<IOrderConnector, OrderConnector>();
            services.AddScoped<IPaymentInfoConnector, PaymentInfoConnector>();
            services.AddScoped<IRatingConnector, RatingConnector>();
            services.AddScoped<ISessionConnector, SessionConnector>();
            services.AddScoped<ITourConnector, TourConnector>();
            services.AddScoped<IUserConnector, UserConnector>();

            #endregion

            #region Repositories

            services.AddScoped<IAddressRepository, AddressRepository>();
            services.AddScoped<ICheckPointRepository, CheckPointRepository>();
            services.AddScoped<IMessageRepository, MessageRepository>();
            services.AddScoped<IMovementRepository, MovementRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IPaymentInfoRepository, PaymentInfoRepository>();
            services.AddScoped<IPromotionRepository, PromotionRepository>();
            services.AddScoped<IReservationRepository, ReservationRepository>();
            services.AddScoped<IRatingRepository, RatingRepository>();
            services.AddScoped<ISessionRepository, SessionRepository>();
            services.AddScoped<ITourRepository, TourRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            #endregion

            //AutoMapper
            services.AddSingleton(x => _mapperConfiguration.CreateMapper());

            #endregion

            //DbContext
            services.AddDbContext<TourStopContext>(
                options => options.UseMySql(Configuration.GetConnectionString("DefaultConnection")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseApplicationInsightsRequestTelemetry();

            app.UseApplicationInsightsExceptionTelemetry();

            app.UseMvc();
        }
    }
}