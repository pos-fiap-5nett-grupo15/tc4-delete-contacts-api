using DeleteContact.Application.DTOs.Contact.DeleteContact;
using DeleteContact.Application.Handlers.Contact.DeleteContact;
using DeleteContact.Infrastructure.Services.Contact;
using DeleteContact.Infrastructure.Settings;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Moq;

namespace TestesUnitarios.Handlers
{
    public class DeleteContactByIdTest
    {

        private readonly DeleteContactHandler deleteContactByIdHandler;
        private readonly Mock<IContactService> _contactService;
        private readonly Mock<ILogger<DeleteContactHandler>> _logger;
        private readonly Mock<IValidator<DeleteContactRequest>> _validator;
        private readonly Mock<IRabbitMQProducerSettings> _rabbit;

        public DeleteContactByIdTest()
        {
            _validator = new Mock<IValidator<DeleteContactRequest>>();
            _rabbit = new Mock<IRabbitMQProducerSettings>();
            _logger = new Mock<ILogger<DeleteContactHandler>>();
            _contactService = new Mock<IContactService>();
            deleteContactByIdHandler = new DeleteContactHandler(_contactService.Object, _logger.Object, _validator.Object, _rabbit.Object);
        }

        [Fact]
        public async void DeleteContactByIdSucess()
        {
            //set
            this._contactService.Setup(x => x.DeleteByIdAsync(It.IsAny<int>())
            ).Returns(Task.CompletedTask);

            DeleteContactRequest request = new DeleteContactRequest
            {
                Id = 1
            };

            //act
            var result = await deleteContactByIdHandler.Handle(request, default);

            //assert
            Assert.True(result.ErrorDescription == null);
        }

        [Fact]
        public async void DeleteContactByIdError()
        {
            DeleteContactRequest request = new DeleteContactRequest
            {
                Id = 0
            };

            var result = await deleteContactByIdHandler.Handle(request, default);

            Assert.True(result.ErrorDescription != null);
        }
    }
}
