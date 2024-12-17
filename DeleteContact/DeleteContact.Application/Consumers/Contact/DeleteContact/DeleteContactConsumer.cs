using DeleteContact.Application.Handlers.Contact.DeleteContact;
using DeleteContact.Application.Messages;
using DeleteContact.Infrastructure.Services.Contact;
using DeleteContact.Infrastructure.Settings;
using Microsoft.Extensions.Logging;
using TechChallenge3.Domain.Enums;

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
            try
            {
                await _contactService.UpdateStatusByIdAsync((await _contactService.GetByIdAsync(message.Id))!, ContactSituationEnum.PENDENTE_DELECAO);
                return;
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, $"An error occurr while deleting contact ID '{message.Id}': {e.Message}.");

                await DeleteContactHandler.PublishByRoutingKey(
                    message.Id,
                    _rabbitMQProducerSettings,
                    _rabbitMQProducerSettings.RoutingKeyInvalid,
                    ct);

                throw;
            }
        }
    }
}
