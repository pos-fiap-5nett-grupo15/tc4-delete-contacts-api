using Dapper;
using TechChallenge.Infrastructure.UnitOfWork;
using TechChallenge3.Domain.Entities.Contact;

namespace DeleteContact.Infrastructure.Repositories.Contact
{
    public class ContactRepository : IContactRepository
    {
        private readonly ITechDatabase _database;
        private const string TABLE_NAME = "Contact";
        private const string SCHEMA = "ContactsManagement";

        public ContactRepository(ITechDatabase database) =>
            _database = database ?? throw new ArgumentNullException(nameof(database));

        public async Task<int> CreateAsync(ContactEntity model)
        {
            var result = await _database.Connection.ExecuteScalarAsync<int>(
                $@"INSERT INTO
                   [{SCHEMA}].[{TABLE_NAME}]
                         ({nameof(ContactEntity.Nome)},
                          {nameof(ContactEntity.Email)},
                          {nameof(ContactEntity.Ddd)},
                          {nameof(ContactEntity.Telefone)},
                          {nameof(ContactEntity.SituacaoAnterior)},
                          {nameof(ContactEntity.SituacaoAtual)})
                    VALUES
                          ('{model.Nome}',
                           '{model.Email}',
                           {model.Ddd},
                           {model.Telefone},
                           {(model.SituacaoAnterior is null ? "NULL" : (int)model.SituacaoAnterior.Value)},
                           {(model.SituacaoAtual is null ? "NULL" : (int)model.SituacaoAtual.Value)});
                    SELECT SCOPE_IDENTITY();
            ");
            return result;
        }

        public async Task<ContactEntity?> GetByIdAsync(int id) =>
            await _database.Connection.QueryFirstOrDefaultAsync<ContactEntity>(
                $"SELECT * FROM [{SCHEMA}].[{TABLE_NAME}] WHERE {nameof(ContactEntity.Id)} = {id};");
        public async Task DeleteByIdAsync(int id) =>
            await _database.Connection.QueryFirstOrDefaultAsync<ContactEntity>(
                $"DELETE FROM [{SCHEMA}].[{TABLE_NAME}] WHERE {nameof(ContactEntity.Id)} = {id};");

        public async Task UpdateByIdAsync(int id, string? nome, string? email, int? ddd, int? telefone) =>
            await _database.Connection.QueryFirstOrDefaultAsync<ContactEntity>(
                $@"UPDATE [{SCHEMA}].[{TABLE_NAME}]
                   SET
                    {(string.IsNullOrWhiteSpace(nome)
                        ? string.Empty
                        : $"{nameof(ContactEntity.Nome)} = '{nome}'")}
                    {(string.IsNullOrWhiteSpace(email)
                        ? string.Empty
                        : $",{nameof(ContactEntity.Email)} = '{email}'")}
                    {(!ddd.HasValue
                        ? string.Empty
                        : $",{nameof(ContactEntity.Ddd)} = {ddd}")}
                    {(!telefone.HasValue
                        ? string.Empty
                        : $",{nameof(ContactEntity.Telefone)} = {telefone}")}
                   WHERE {nameof(ContactEntity.Id)} = {id};");

        public async Task<ContactEntity?> UpdateStatusByIdAsync(int id, int? situacaoAnterior, int novaSituacao) =>
            await _database.Connection.QueryFirstOrDefaultAsync<ContactEntity>(
                $@"UPDATE [{SCHEMA}].[{TABLE_NAME}]
                   SET
                    {nameof(ContactEntity.SituacaoAnterior)} = {(situacaoAnterior.HasValue ? situacaoAnterior.Value : "NULL")},
                    {nameof(ContactEntity.SituacaoAtual)} = {novaSituacao}
                   WHERE {nameof(ContactEntity.Id)} = {id};");

        public async Task<IEnumerable<ContactEntity>> GetListPaginatedByFiltersAsync(int? ddd, int currentIndex, int pageSize) =>
            await _database.Connection.QueryAsync<ContactEntity>(
                $@"SELECT * FROM [{SCHEMA}].[{TABLE_NAME}]
                   {(!ddd.HasValue
                        ? string.Empty
                        : $"WHERE {nameof(ContactEntity.Ddd)} = {ddd}")}
                   ORDER BY {nameof(ContactEntity.Id)} ASC
                   OFFSET {currentIndex} ROWS FETCH FIRST {pageSize} ROWS ONLY;");
    }
}
