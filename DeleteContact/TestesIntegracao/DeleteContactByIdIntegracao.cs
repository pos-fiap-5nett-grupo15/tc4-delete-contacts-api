using DeleteContact.Infrastructure.Repositories.Contact;
using Microsoft.Extensions.Configuration;
using TechChallenge.Infrastructure.Crypto;
using TechChallenge.Infrastructure.UnitOfWork;
using TechChallenge3.Domain.Entities.Contact;

namespace TestesIntegracao
{
    public class DeleteContactByIdIntegracao
    {
        private readonly TechDatabase _database;
        private readonly ContactRepository _repository;


        public DeleteContactByIdIntegracao()
        {
            var configuration = new ConfigurationBuilder()
             .SetBasePath(System.AppContext.BaseDirectory)
             .AddJsonFile("appsettings.test.json")
             .Build();

            Environment.SetEnvironmentVariable("TECH1_API_SEC_KEY", configuration["DataBaseSec:TECH1_API_SEC_KEY"]);
            Environment.SetEnvironmentVariable("TECH1_API_SEC_IV", configuration["DataBaseSec:TECH1_API_SEC_IV"]);

            _database = new TechDatabase(configuration, new CryptoService(null));

            _repository = new ContactRepository(_database);
        }

        [Fact]
        public async Task DeleteById()
        {
            var contact = new ContactEntity
            {
                Nome = "Miguel Angelo",
                Email = "miguel@exemplo.com",
                Ddd = 11,
                Telefone = 55667788
            };
            await _repository.CreateAsync(contact);

            await _repository.DeleteByIdAsync(1);

            var result = await _repository.GetByIdAsync(1);
            Assert.Null(result);
        }
    }
}
