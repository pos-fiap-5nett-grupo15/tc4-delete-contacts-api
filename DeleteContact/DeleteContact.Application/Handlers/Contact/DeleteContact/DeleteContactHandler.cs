using DeleteContact.Application.DTOs.Contact.DeleteContact;
using DeleteContact.Application.Messages;
using DeleteContact.Infrastructure.Services.Contact;
using DeleteContact.Infrastructure.Settings;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using TechChallenge.Common.RabbitMQ;


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

        public async Task<DeleteContactResponse> Handle(DeleteContactRequest requisicao, CancellationToken ct)
        {
            if (Validate(requisicao) is var validacao && !string.IsNullOrWhiteSpace(validacao.ErrorDescription))
                return validacao;

            await _contactService.DeleteByIdAsync(requisicao.Id);

            await RabbitMQManager.Publish(
                new DeleteContactMessage { Id = requisicao.Id },
                _rabbitMQProducerSettings.Host,
                _rabbitMQProducerSettings.Exchange,
                _rabbitMQProducerSettings.RoutingKey,
                ct);

            return new DeleteContactResponse();
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
    }
}
