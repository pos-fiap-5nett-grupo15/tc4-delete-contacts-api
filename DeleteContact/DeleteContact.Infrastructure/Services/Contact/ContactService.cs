using DeleteContact.Infrastructure.UnitOfWork;
using TechChallenge3.Domain.Entities.Contact;
using TechChallenge3.Domain.Enums;

namespace DeleteContact.Infrastructure.Services.Contact
{
    public class ContactService : IContactService
    {
        private readonly IDeleteContactUnitOfWork _unitOfWork;

        public ContactService(IDeleteContactUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> CreateAsync(ContactEntity model)
        {
            return await _unitOfWork.ContactRepository.CreateAsync(model);
        }

        public async Task DeleteByIdAsync(int id)
        {
            await _unitOfWork.ContactRepository.DeleteByIdAsync(id);
        }

        public async Task<ContactEntity?> GetByIdAsync(int id)
        {
            return await _unitOfWork.ContactRepository.GetByIdAsync(id);
        }

        public async Task<ContactEntity?> UpdateStatusByIdAsync(ContactEntity contactEntity, ContactSituationEnum novoStatus)
        {
            return await _unitOfWork.ContactRepository.UpdateStatusByIdAsync(contactEntity.Id, (int?)contactEntity.SituacaoAtual, (int)novoStatus);
        }

        public async Task<IEnumerable<ContactEntity>> GetListPaginatedByFiltersAsync(int? ddd, int currentIndex, int pageSize)
        {
            return await _unitOfWork.ContactRepository.GetListPaginatedByFiltersAsync(ddd, currentIndex, pageSize);
        }

        public async Task UpdateByIdAsync(int id, string? nome, string? email, int? ddd, int? telefone)
        {
            await _unitOfWork.ContactRepository.UpdateByIdAsync(id, nome, email, ddd, telefone);
        }
    }
}
