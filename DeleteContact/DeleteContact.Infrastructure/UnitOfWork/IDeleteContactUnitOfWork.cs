using DeleteContact.Infrastructure.Repositories.Contact;
using TechChallenge.Infrastructure.UnitOfWork;

namespace DeleteContact.Infrastructure.UnitOfWork
{
    public interface IDeleteContactUnitOfWork : IBaseUnitOfWork
    {
        IContactRepository ContactRepository { get; }
    }
}

