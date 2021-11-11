using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Travel.Application.Common.Interfaces;
using Travel.Application.Common.Exceptions;
using Travel.Domain.Entities;

namespace Travel.Application.Todos.Commands.UpdateTodo
{
    public class UpdateTodoCommand : IRequest
    {
        public long Id { get; set; }

        public string Name { get; set; }
    }

    public class UpdateTodoRequestHandler : IRequestHandler<UpdateTodoCommand>
    {
        private readonly IApplicationDbContext _applicationDbContext;

        public UpdateTodoRequestHandler(IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<Unit> Handle(UpdateTodoCommand request, CancellationToken cancellationToken)
        {
            var todo = await _applicationDbContext.Todos.FindAsync(request.Id);

            if (todo == null) 
            {
                throw new NotFoundException(nameof(Todo), request.Id);
            }

            todo.Name = request.Name;
            await _applicationDbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
