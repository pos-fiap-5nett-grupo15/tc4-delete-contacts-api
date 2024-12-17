using DeleteContact.Application.Messages;

namespace DeleteContact.Application.Consumers.Contact.DeleteContact
{
    public interface IDeleteContactConsumer
    {
        Task HandleAsync(DeleteContactMessage message, CancellationToken ct);
    }
}
