using System;
using System.Collections.Generic;
using System.Text;
using Travel.Application.Common.Mappings;
using Travel.Domain.Entities;

namespace Travel.Application.Dtos.TodoDtos
{
    public class TodoDto : IMapFrom<Todo>
    {
        public long Id { get; set; }

        public string Name { get; set; }
    }
}
