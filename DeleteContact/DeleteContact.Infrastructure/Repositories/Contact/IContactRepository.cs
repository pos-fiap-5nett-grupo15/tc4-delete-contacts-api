using TechChallenge3.Domain.Entities.Contact;

namespace DeleteContact.Infrastructure.Repositories.Contact
{
    public interface IContactRepository
    {
        Task<int> CreateAsync(ContactEntity model);
        Task<ContactEntity?> GetByIdAsync(int id);
        Task DeleteByIdAsync(int id);
        Task UpdateByIdAsync(int id, string? nome, string? email, int? ddd, int? telefone);
        Task<ContactEntity?> UpdateStatusByIdAsync(int id, int? situacaoAnterior, int novaSituacao);
        Task<IEnumerable<ContactEntity>> GetListPaginatedByFiltersAsync(int? ddd, int currentIndex, int pageSize);
    }
}
