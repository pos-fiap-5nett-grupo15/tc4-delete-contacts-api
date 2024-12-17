using DeleteContact.Application.Messages;
using DeleteContact.Infrastructure.Services.Contact;
using DeleteContact.Infrastructure.Settings;
using Microsoft.Extensions.Logging;
using TechChallenge.Common.RabbitMQ;

namespace DeleteContact.Application.Consumers.Contact.DeleteContact
{
    public class DeleteContactConsumer : IDeleteContactConsumer
    {
        private readonly IContactService _contactService;
        private readonly ILogger<DeleteContactConsumer> _logger;
        private readonly IRabbitMQProducerSettings _rabbitMQProducerSettings;

        public DeleteContactConsumer(
            IContactService contactService,
            ILogger<DeleteContactConsumer> logger,
            IRabbitMQProducerSettings rabbitMQProducerSettings)
        {
            _logger = logger;
            _contactService = contactService;
            _rabbitMQProducerSettings = rabbitMQProducerSettings;
        }

        public async Task HandleAsync(DeleteContactMessage message, CancellationToken ct)
        {
            await RabbitMQManager.Publish(
                new DeleteContactMessage { Id = message.Id },
                _rabbitMQProducerSettings.Host,
                _rabbitMQProducerSettings.Exchange,
                _rabbitMQProducerSettings.RoutingKey,
                ct);
            return;
        }
    }
}
