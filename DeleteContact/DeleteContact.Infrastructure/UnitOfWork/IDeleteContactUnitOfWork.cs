using DeleteContact.Infrastructure.Repositories.Contact;
using TechChallenge3.Infrastructure.UnitOfWork;

namespace DeleteContact.Infrastructure.UnitOfWork
{
    public interface IDeleteContactUnitOfWork : IBaseUnitOfWork
    {
        IContactRepository ContactRepository { get; }
    }
}

