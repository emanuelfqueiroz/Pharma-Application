using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PharmaRep.Application.Brand.Queries;
using PharmaRep.Application.Common;
using PharmaRep.Domain.Brand.Entities;

namespace _PharmaRep.WebAPI.V1.Controllers
{
    [ApiVersion(1.0)]
    [Route("api/v1/brands")]
    [ApiController]
    public class BrandsController(IQueryHandler<BrandQuery, IEnumerable<Brand>> handler) : ControllerBase
    {
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] BrandQuery query)
        {
            var response = await handler.HandleAsync(query ?? new());
            return Ok(response);
        }
    }
}
