using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Travel.Application.Todos.Queries.GetTodos;
using Travel.Application.Todos.Commands.CreateTodo;

namespace Travel.WebApi.Controllers
{
    public class TodoController : ApiController
    {
        [HttpGet]
        [ProducesResponseType(typeof(TodosVM), StatusCodes.Status200OK)]
        public async Task<ActionResult<TodosVM>> Get()
        {
            return await Mediator.Send(new GetTodosQuery());
        }

        [HttpPost]
        [ProducesResponseType(typeof(long), StatusCodes.Status200OK)]
        public async Task<ActionResult<long>> Create(CreateTodoCommand createTodoCommand)
        {
            return await Mediator.Send(createTodoCommand);
        }
    }
}
