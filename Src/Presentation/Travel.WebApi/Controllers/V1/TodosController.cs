using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Travel.Application.Todos.Queries.GetTodos;
using Travel.Application.Todos.Commands.CreateTodo;
using Travel.Application.Todos.Commands.UpdateTodo;
using Travel.Application.Todos.Commands.DeleteTodo;

namespace Travel.WebApi.Controllers.V1
{
    public class TodosController : ApiController
    {
        [HttpGet]
        [ProducesResponseType(typeof(TodosVM), StatusCodes.Status200OK)]
        public async Task<ActionResult<TodosVM>> Get()
        {
            return await Mediator.Send(new GetTodosQuery());
        }

        [HttpPost]
        [ProducesResponseType(typeof(long), StatusCodes.Status200OK)]
        public async Task<ActionResult<long>> Create(CreateTodoCommand command)
        {
            return await Mediator.Send(command);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Update([FromRoute] long id, UpdateTodoCommand command)
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
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete(long id, DeleteTodoCommand command)
        {
            if(id != command.Id)
            {
                return BadRequest();
            }

            await Mediator.Send(command);
            return NoContent();
        }
    }
}
