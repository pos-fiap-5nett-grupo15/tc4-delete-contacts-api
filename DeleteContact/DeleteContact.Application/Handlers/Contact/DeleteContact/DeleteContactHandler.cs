using DeleteContact.Application.DTOs.Contact.DeleteContact;
using DeleteContact.Application.Messages;
using DeleteContact.Infrastructure.Services.Contact;
using DeleteContact.Infrastructure.Settings;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using TechChallenge3.Common.RabbitMQ;
using TechChallenge3.Domain.Enums;


namespace DeleteContact.Application.Handlers.Contact.DeleteContact
{
    public class DeleteContactHandler : IRequestHandler<DeleteContactRequest, DeleteContactResponse>
    {
        private readonly IContactService _contactService;
        private readonly ILogger<DeleteContactHandler> _logger;
        private readonly IValidator<DeleteContactRequest> _validator;
        private readonly IRabbitMQProducerSettings _rabbitMQProducerSettings;

        public DeleteContactHandler(
            IContactService contactService,
            ILogger<DeleteContactHandler> logger,
            IValidator<DeleteContactRequest> validator,
            IRabbitMQProducerSettings rabbitMQProducerSettings)
        {
            _logger = logger;
            _validator = validator;
            _contactService = contactService;
            _rabbitMQProducerSettings = rabbitMQProducerSettings;
        }

        public async Task<DeleteContactResponse> Handle(DeleteContactRequest request, CancellationToken ct)
        {
            try
            {
                if (Validate(request) is var validacao && !string.IsNullOrWhiteSpace(validacao.ErrorDescription))
                    return validacao;

                if (await _contactService.GetByIdAsync(request.Id) is var contact && contact is not null)
                {
                    await _contactService.UpdateStatusByIdAsync(contact, ContactSituationEnum.PENDENTE_DELECAO);

                    await PublishByRoutingKey(
                        request.Id,
                        _rabbitMQProducerSettings,
                        _rabbitMQProducerSettings.RoutingKey,
                        ct);

                    return new DeleteContactResponse();
                }
                else
                {
                    _logger.LogError($"An error occurr while deleting contact ID '{request.Id}'.");

                    await PublishByRoutingKey(
                        request.Id,
                        _rabbitMQProducerSettings,
                        _rabbitMQProducerSettings.RoutingKeyInvalid,
                        ct);

                    return new DeleteContactResponse();
                }
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, $"An error occurr while deleting contact ID '{request.Id}': {e.Message}.");

                await PublishByRoutingKey(
                    request.Id, 
                    _rabbitMQProducerSettings, 
                    _rabbitMQProducerSettings.RoutingKeyInvalid,
                    ct);

                throw;
            }
        }

        public DeleteContactResponse Validate(DeleteContactRequest requisicao)
        {
            var retorno = new DeleteContactResponse();
            var result = _validator.Validate(requisicao);
            if (!result.IsValid)
            {
                var erroMensagem = "";
                foreach (var error in result.Errors)
                    erroMensagem += error.ErrorMessage + " ";

                retorno.ErrorDescription = erroMensagem;
            }

            return retorno;
        }

        internal static async Task PublishByRoutingKey(
            int contactId,
            IRabbitMQProducerSettings rabbitMQProducerSettings,
            string routingKey,
            CancellationToken ct)
        {
            await RabbitMQManager.PublishAsync(
                message: new DeleteContactMessage { Id = contactId },
                hostName: rabbitMQProducerSettings.Host,
                port: rabbitMQProducerSettings.Port,
                userName: rabbitMQProducerSettings.Username,
                password: rabbitMQProducerSettings.Password,
                exchangeName: rabbitMQProducerSettings.Exchange,
                routingKeyName: routingKey,
                ct);
        }
    }
}
