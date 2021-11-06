using System;
using System.Collections.Generic;
using System.Text;
using Travel.Application.Dtos.TodoDtos;

namespace Travel.Application.Todos.Queries.GetTodos
{
    public class TodosVM
    {
        public IEnumerable<TodoDto> Todos { get; set; }
    }
}
