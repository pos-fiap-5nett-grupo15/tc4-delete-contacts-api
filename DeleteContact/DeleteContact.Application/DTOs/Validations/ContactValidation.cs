using DeleteContact.Application.DTOs.Contact.DeleteContact;
using FluentValidation;

namespace DeleteContact.Application.DTOs.Validations
{
    public class ContactValidation : AbstractValidator<DeleteContactRequest>
    {
        public ContactValidation()
        {
            RuleFor(c => c.Id)
                .NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido");
        }
    }
}
