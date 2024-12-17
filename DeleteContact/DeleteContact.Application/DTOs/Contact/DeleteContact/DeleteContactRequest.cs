using MediatR;

namespace DeleteContact.Application.DTOs.Contact.DeleteContact
{
    public class DeleteContactRequest : IRequest<DeleteContactResponse>
    {
        public int Id { get; set; }
    }
}
