using DeleteContact.Infrastructure.Repositories.Contact;
using TechChallenge3.Infrastructure.UnitOfWork;

namespace DeleteContact.Infrastructure.UnitOfWork
{
    public class DeleteContactUnitOfWork : BaseUnitOfWork, IDeleteContactUnitOfWork
    {
        private readonly ITechDatabase _techDabase;

        public IContactRepository ContactRepository { get; }

        public DeleteContactUnitOfWork(ITechDatabase database)
            : base(database)
        {
            this._techDabase = database ?? throw new ArgumentNullException(nameof(database));

            this.ContactRepository = new ContactRepository(this._techDabase);
        }
    }
}
