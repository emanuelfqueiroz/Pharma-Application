using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PharmaRep.Application.Common;
using PharmaRep.Application.Medicine.Queries;
using PharmaRep.Domain.Medicine.Entities;

namespace _PharmaRep.WebAPI.V1.Controllers
{
    [ApiVersion(1.0)]
    [Route("api/v{version:apiVersion}/medicalreactions")]
    [ApiController]
    public class MedicalReactionsController(IQueryHandler<MedicalReactionQuery, IEnumerable<MedicalReaction>> handler) : ControllerBase
    {
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] MedicalReactionQuery query)
        {
            var response = await handler.HandleAsync(query ?? new());
            return Ok(response);
        }
    }
}
