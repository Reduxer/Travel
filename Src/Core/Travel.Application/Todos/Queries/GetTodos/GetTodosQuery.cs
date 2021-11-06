using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using Travel.Application.Dtos.TodoDtos;
using Travel.Application.Common.Interfaces;
using System.Threading.Tasks;
using System.Threading;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Travel.Application.Todos.Queries.GetTodos
{
    public class GetTodosQuery : IRequest<TodosVM>
    {
        
    }

    public class GetTodosQueryHandler : IRequestHandler<GetTodosQuery, TodosVM>
    {
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly IMapper _mapper;

        public GetTodosQueryHandler(IApplicationDbContext applicationDbContext, IMapper mapper)
        {
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
        }

        public async Task<TodosVM> Handle(GetTodosQuery request, CancellationToken cancellationToken)
        {
            var todos = await _applicationDbContext.Todos
                .ProjectTo<TodoDto>(_mapper.ConfigurationProvider)
                .OrderBy(t => t.Name)
                .ToListAsync(cancellationToken);

            return new TodosVM()
            {
                Todos = todos
            };
        }
    }
}
