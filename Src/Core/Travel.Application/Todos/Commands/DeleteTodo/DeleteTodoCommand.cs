using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using Travel.Application.Common.Interfaces;
using Travel.Application.Common.Exceptions;
using Travel.Domain.Entities;
using System.Threading.Tasks;
using System.Threading;

namespace Travel.Application.Todos.Commands.DeleteTodo
{
    public class DeleteTodoCommand : IRequest
    {
        public long Id { get; set; }
    }

    public class DeleteTodoCommandHandler : IRequestHandler<DeleteTodoCommand>
    {
        private readonly IApplicationDbContext _applicationDbContext;

        public DeleteTodoCommandHandler(IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<Unit> Handle(DeleteTodoCommand request, CancellationToken cancellationToken)
        {
            var todo = await _applicationDbContext.Todos.FindAsync(request.Id);

            if (todo == null)
            {
                throw new NotFoundException(nameof(Todo), request.Id);
            }

            _applicationDbContext.Todos.Remove(todo);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
