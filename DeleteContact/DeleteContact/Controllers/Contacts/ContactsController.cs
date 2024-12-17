using DeleteContact.Application.DTOs.Contact.DeleteContact;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TechChallenge3.Common.DTOs;

namespace DeleteContact.Api.Controllers.Contacts
{
    [ApiController]
    [Route("[controller]")]
    // TODO: Implementar autenticação/autorização
    public class ContactsController
        : ControllerBase
    {
        private readonly IMediator _mediator;

        public ContactsController(IMediator mediator) =>
            _mediator = mediator;

        [HttpDelete("{id}")]
        [SwaggerResponse(StatusCodes.Status201Created)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, type: typeof(BaseReponse))]
        public async Task<IActionResult> DeleteByIdAsync(
            [FromRoute] int id)
        {
            return Created(string.Empty, await _mediator.Send(new DeleteContactRequest { Id = id }));
        }
    }
}
