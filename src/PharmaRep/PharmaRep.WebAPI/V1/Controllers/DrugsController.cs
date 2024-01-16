using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PharmaRep.Application.Common;
using PharmaRep.Application.Medicine.Commands.DeactivateDrug;
using PharmaRep.Application.Medicine.Commands.RegisterDrug;
using PharmaRep.Application.Medicine.Queries;
using PharmaRep.Domain.Medicine.Aggregates;
using PharmaRep.Domain.Medicine.Entities;
using System.ComponentModel.DataAnnotations;

namespace _PharmaRep.WebAPI.V1.Controllers
{
    [ApiVersion(2.0)]
    [Route("/api/v1/drugs")]
    [ApiController]
    public class DrugsController(ILogger<AuthenticationController> logger) : ControllerBase
    {
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] DrugQuery query,
            [FromServices] IQueryHandler<DrugQuery, IEnumerable<DrugInformation>> handler)
        {
            var response = await handler.HandleAsync(query ?? new());
            return Ok(response);
        }


        [Authorize]

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id, [FromServices] IQueryHandler<DrugByIdQuery, DrugAggregate> handler)
        {
            var drug = await handler.HandleAsync(new DrugByIdQuery(id));
            if (drug is null)
            {
                return NotFound();
            }
            return Ok(drug);

        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] RegisterDrugCommand command,
            [FromServices] ICommandHandler<RegisterDrugCommand, EntityCreated> handler)
        {
            try
            {
                var response = await handler.HandleAsync(command);
                if (!response.IsSuccess)
                {
                    return Conflict(response.Message);
                }
                return CreatedAtAction(nameof(Get), new { id = response.Data!.Id }, response);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error adding drug {name}", command.Name);
                return StatusCode(500, "Internal server error");
            }
        }
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateDrugCommand command,
            [FromServices] ICommandHandler<UpdateDrugCommand, EntityUpdated> handler)
        {
            try
            {
                command.Id = id;
                var response = await handler.HandleAsync(command);

                if (response == null)
                {
                    return NotFound();
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error updating drug {name}", command.Name);
                return StatusCode(500, "Internal Error");
            }
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, [FromServices] ICommandHandler<DectivateDrugCommand, DeactivatedEntity> handler)
        {
            try
            {
                var response = await handler.HandleAsync(new DectivateDrugCommand(id));
                return response.IsSuccess ? Ok(response) : Problem(response.Message);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error deleting drug {id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [Authorize]
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> Update(int id,
            [FromBody][Required] UpdateDrugStatusCommand command,
            [FromServices] ICommandHandler<UpdateDrugStatusCommand, EntityUpdated> handler)
        {

            try
            {
                command.Id = id;
                var response = await handler.HandleAsync(command);

                if (response.IsSuccess)
                {
                    return Ok(response);
                }
                else
                {
                    return Problem(response.Message);
                }

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error updating status of drug {id}", id);
                return StatusCode(500, "Internal server error");
            }
        }
    }

}
