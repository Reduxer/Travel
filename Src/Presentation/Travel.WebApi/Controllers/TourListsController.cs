using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Travel.Application.TourLists.Commands.CreateTourList;
using Travel.Application.TourLists.Commands.DeleteTourList;
using Travel.Application.TourLists.Commands.UpdateTourList;
using Travel.Application.TourLists.Queries.ExportTours;
using Travel.Application.TourLists.Queries.GetTours;

namespace Travel.WebApi.Controllers
{
    public class TourListsController : ApiController
    {
        [HttpGet]
        [ProducesResponseType(typeof(ToursVm), StatusCodes.Status200OK)]
        public async Task<ActionResult<ToursVm>> Get()
        {
            return await Mediator.Send(new GetToursQuery());
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<FileResult> GetById([FromRoute] int id)
        {
            var vm = await Mediator.Send(new ExportToursQuery() { ListId = id });

            return File(vm.Content, vm.ContentType, vm.FileName);
        }

        [HttpPost]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<int>> Create(CreateTourListCommand command)
        {
            return await Mediator.Send(command);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update([FromRoute] int id, UpdateTourListCommand command)
        {
            if(id != command.Id)
            {
                return BadRequest();
            }

            await Mediator.Send(command);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            await Mediator.Send(new DeleteTourListCommand() { Id = id });

            return NoContent();
        }
    }
}
