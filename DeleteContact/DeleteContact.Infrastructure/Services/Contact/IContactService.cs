using TechChallenge3.Domain.Entities.Contact;
using TechChallenge3.Domain.Enums;

namespace DeleteContact.Infrastructure.Services.Contact
{
    public interface IContactService
    {
        Task<int> CreateAsync(ContactEntity model);
        Task<ContactEntity?> GetByIdAsync(int id);
        Task<ContactEntity?> UpdateStatusByIdAsync(ContactEntity contactEntity, ContactSituationEnum novoStatus);
        Task DeleteByIdAsync(int id);
        Task UpdateByIdAsync(int id, string? nome, string? email, int? ddd, int? telefone);
        Task<IEnumerable<ContactEntity>> GetListPaginatedByFiltersAsync(int? ddd, int currentIndex, int pageSize);
    }
}
