using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PharmaRep.Application.Common;
using PharmaRep.Application.Medicine.Queries;
using PharmaRep.Domain.Medicine.Entities;

namespace PharmaRep.WebAPI.Controllers
{
    [ApiVersion(1.0)]
    [Route("api/medicalconditions")]
    [ApiController]
    public class MedicalConditionController(IQueryHandler<MedicalConditionQuery, IEnumerable<MedicalCondition>> handler) : ControllerBase
    {
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Get(MedicalConditionQuery query)
        {
            var response = await handler.HandleAsync(query ?? new());
            return Ok(response);
        }
    }
}
