using CreateContract.Worker.Consumers;
using DeleteContact.Application.Consumers.Contact.DeleteContact;
using DeleteContact.Infrastructure.RabbitMQ;
using DeleteContact.Infrastructure.Services.Contact;
using DeleteContact.Infrastructure.Settings;
using DeleteContact.Infrastructure.UnitOfWork;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using TechChallenge3.Infrastructure.Crypto;
using TechChallenge3.Infrastructure.Settings;
using TechChallenge3.Infrastructure.UnitOfWork;

namespace CreateContract.Worker
{
    internal class Startup
    {
        public IConfiguration Configuration;

        public Startup(IConfiguration configuration) =>
            this.Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

        internal void Configure(IApplicationBuilder app, IWebHostEnvironment environment) { }

        internal void ConfigureService(IServiceCollection services)
        {
            // Logging
            services.AddLogging();

            this.ConfigureRabitMQ(services);
            this.ConfigureDatabase(services);
            this.ConfigureCustomServices(services);
            this.ConfigureConsumers(services);
        }

        private void ConfigureRabitMQ(IServiceCollection services)
        {
            services.AddSingleton<IRabbitMQProducerSettings>(Configuration.GetSection(nameof(RabbitMQProducerSettings))?.Get<RabbitMQProducerSettings>() ?? throw new ArgumentNullException(nameof(RabbitMQProducerSettings)));
            services.AddSingleton(new RabbitMQConnector(Configuration.GetSection(nameof(RabbitMQConsumerSettings))?.Get<RabbitMQConsumerSettings>()));
            services.AddHostedService<RabbitMQConsumer>();
        }

        private void ConfigureDatabase(IServiceCollection services)
        {
            var cryptoService = new CryptoService(Configuration.GetSection(nameof(CryptoSettings)).Get<CryptoSettings>());
            services.AddSingleton((ICryptoService)cryptoService);
            services.AddScoped<ITechDatabase, TechDatabase>();
            services.AddScoped<IDeleteContactUnitOfWork, DeleteContactUnitOfWork>();
        }

        private void ConfigureCustomServices(IServiceCollection services) =>
            services.AddScoped<IContactService, ContactService>();

        private void ConfigureConsumers(IServiceCollection services) =>
            services.AddScoped<IDeleteContactConsumer, DeleteContactConsumer>();

    }
}
