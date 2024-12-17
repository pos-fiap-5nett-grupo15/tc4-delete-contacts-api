using DeleteContact.Application.DTOs.Contact.DeleteContact;
using DeleteContact.Application.DTOs.Validations;
using DeleteContact.Application.Handlers.Contact.DeleteContact;
using DeleteContact.Infrastructure.Services.Contact;
using DeleteContact.Infrastructure.Settings;
using DeleteContact.Infrastructure.UnitOfWork;
using FluentValidation;
using TechChallenge.Infrastructure.DefaultStartup;

namespace DeleteContact.Api
{
    internal class Startup : BaseStartup
    {
        public IConfiguration Configuration;

        public Startup(IConfiguration configuration)
            : base(configuration)
        {
            this.Configuration = configuration;
        }

        internal void ConfigureImpl(IApplicationBuilder app, IWebHostEnvironment environment)
        {
            this.Configure(app, environment);
        }

        internal void ConfigureServiceImpl(IServiceCollection services)
        {
            this.ConfigureService(services);
            services.AddLogging();

            ConfigureUnitOfWork(services);
            ConfigureHandleServices(services);
            ConfigureContactServices(services);
        }

        private void ConfigureContactServices(IServiceCollection services)
        {
            services.AddScoped<IContactService, ContactService>();
        }
        
        private void ConfigureHandleServices(IServiceCollection services)
        {
            services.AddSingleton<IRabbitMQProducerSettings>(Configuration.GetSection(nameof(RabbitMQProducerSettings))?.Get<RabbitMQProducerSettings>() ?? throw new ArgumentNullException(nameof(RabbitMQProducerSettings)));

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DeleteContactHandler).Assembly));
            services.AddTransient<IValidator<DeleteContactRequest>, ContactValidation>();
        }

        private void ConfigureUnitOfWork(IServiceCollection services)
        {
            services.AddScoped<IDeleteContactUnitOfWork, DeleteContactUnitOfWork>();
        }
    }
}
