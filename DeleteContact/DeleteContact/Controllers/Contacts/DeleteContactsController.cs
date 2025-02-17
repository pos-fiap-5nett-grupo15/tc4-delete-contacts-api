using System.Dynamic;
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
    public class DeleteContactsController
        : ControllerBase
    {
        private readonly IMediator _mediator;

        public DeleteContactsController(IMediator mediator) =>
            _mediator = mediator;

        [HttpDelete("{id}")]
        [SwaggerResponse(StatusCodes.Status201Created)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, type: typeof(BaseReponse))]
        public async Task<IActionResult> DeleteByIdAsync(
            [FromRoute] int id)
        {
            try
            {
                if (await _mediator.Send(new DeleteContactRequest { Id = id }) is var response && !string.IsNullOrWhiteSpace(response.ErrorDescription))
                    return BadRequest(response);
            
                return NoContent();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                dynamic response = new ExpandoObject();
                response.Message = e.Message;
                response.StackTrace = e.StackTrace;
                response.InnerException = e.InnerException;

                return BadRequest(response);
            }
        }
    }
}
