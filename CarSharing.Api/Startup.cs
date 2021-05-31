using Amazon.SQS;
using CarSharing.Api.Events;
using CarSharing.Application.UseCases.DeviceState;
using CarSharing.Application.UseCases.FinishBooking;
using CarSharing.Application.UseCases.GetCars;
using CarSharing.Application.UseCases.StartBooking;
using CarSharing.Core.Repositories;
using CarSharing.Core.Services;
using CarSharing.Infrastructure.DataAccess;
using CarSharing.Infrastructure.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text.Json.Serialization;

namespace CarSharing.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            RegisterUseCases(services);

            services
                .AddControllers()
                .AddJsonOptions(opts =>
                {
                    opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

            services.AddSwaggerGen();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "CarSharing Api");
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void RegisterUseCases(IServiceCollection services)
        {
            var awsCredentials = new Amazon.Runtime.BasicAWSCredentials("accesskey", "secret");
            services.AddSingleton<IAmazonSQS>(new AmazonSQSClient(awsCredentials, Amazon.RegionEndpoint.EUWest1));

            services.AddScoped<IStartBookingUseCase, StartBookingUseCase>();
            services.AddScoped<IFinishBookingUseCase, FinishBookingUseCase>();
            services.AddScoped<IGetCarsUseCase, GetCarsUseCase>();
            services.AddSingleton<IUpdateDeviceStateUseCase, UpdateDeviceStateUseCase>();

            services.AddScoped<ICarsRepository, CarsRepository>();
            services.AddScoped<IBookingRepository, BookingRepository>();
            services.AddSingleton<IDeviceStateRepository, DeviceStateRepository>();

            services.AddScoped<ICarLockingService, CarLockingService>();

            services.AddHostedService<EventSubscriber>();
        }

    }
}
