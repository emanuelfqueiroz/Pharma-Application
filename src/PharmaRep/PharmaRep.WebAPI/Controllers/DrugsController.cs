﻿using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PharmaRep.Application.Common;
using PharmaRep.Application.Medicine.Commands.DeactivateDrug;
using PharmaRep.Application.Medicine.Commands.RegisterDrug;
using PharmaRep.Application.Medicine.Queries;
using PharmaRep.Domain.Medicine.Aggregates;
using PharmaRep.Domain.Medicine.Entities;
using PharmaRep.Domain.Medicine.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace PharmaRep.WebAPI.Controllers
{
    [ApiVersion(1.0)]
    [Route("api/drugs")]
    [ApiController]
    public class DrugsController(ILogger<AuthenticationController> logger) : ControllerBase
    {
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetDrugs([FromQuery] DrugQuery query,
            [FromServices] IQueryHandler<DrugQuery, IEnumerable<DrugInformation>> handler)
        {
            var response = await handler.HandleAsync(query ?? new());
            return Ok(response);
        }


        [Authorize]

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDrug(int id, [FromServices] IQueryHandler<DrugByIdQuery, DrugAggregate> handler)
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
        public async Task<IActionResult> PostAdd([FromBody] RegisterDrugCommand command,
            [FromServices] ICommandHandler<RegisterDrugCommand, EntityCreated> handler)
        {
            try
            {
                var response = await handler.HandleAsync(command);
                if (response.IsSuccess)
                {
                    logger.LogInformation("Drug {id}:{name} added successfully ", response?.Data?.Id, command.Name);
                    return CreatedAtAction(nameof(GetDrug), new { id = response.Data!.Id }, response);
                }
                return Conflict(response.Message);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error adding drug {name}", command.Name);
                return StatusCode(500, "Internal server error");
            }
        }
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDrugAsync(int id, [FromBody] UpdateDrugCommand command,
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
        public async Task<IActionResult> DeleteDrugAsync(int id, [FromServices] ICommandHandler<DectivateDrugCommand, DeactivatedEntity> handler)
        {
            try
            {
                var response = await handler.HandleAsync(new DectivateDrugCommand(id));

                if (response.IsSuccess)
                {
                    logger.LogInformation("Drug {id} deactivated successfully ", id);
                    return Ok(response);
                }
                else
                {
                    logger.LogInformation("Drug {id} not deleted ", id);
                    return BadRequest(response.Message);
                }

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error deleting drug {id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [Authorize]
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateDrugStatus(int id,
            [FromBody][Required] UpdateDrugStatusCommand command,
            [FromServices] ICommandHandler<UpdateDrugStatusCommand, EntityUpdated> handler)
        {

            try
            {
                command.Id = id;
                var response = await handler.HandleAsync(command);

                if (response.IsSuccess)
                {
                    logger.LogInformation("Status of drug {id} was updated successfully", id);
                    return Ok(response);
                }
                else
                {
                    logger.LogInformation("Drug {id} not deleted ", id);
                    return BadRequest(response.Message);
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
