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

            Environment.SetEnvironmentVariable("TECH1_API_SEC_KEY", "IGVRaqQssHbDh3adxV22rw73SmOusqrKHomyJe33TrM=");
            Environment.SetEnvironmentVariable("TECH1_API_SEC_IV", "iqs1k4XU6wADBBOP3cpg5A==");
            
            _database = new TechDatabase(configuration, new CryptoService(null));

            _repository = new ContactRepository(_database);
        }

        [Fact]
        public async Task DeleteById()
        {
            var contact = new ContactEntity
            {
                Nome = "Miguel Angelo",
                Email = "miguel@example.com",
                Ddd = 31,
                Telefone = 55667788
            };
            await _repository.CreateAsync(contact);

            await _repository.DeleteByIdAsync(1);

            var result = await _repository.GetByIdAsync(1);
            Assert.Null(result);
        }
    }
}
