using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Travel.Application.Common.Interfaces;
using Travel.Domain.Entities;

namespace Travel.Application.Todos.Commands.CreateTodo
{
    public class CreateTodoCommand : IRequest<long>
    {
        public string Name { get; set; }
    }

    public class CreateTodoCommandHandler : IRequestHandler<CreateTodoCommand, long>
    {
        private readonly IApplicationDbContext _applicationDbContext;

        public CreateTodoCommandHandler(IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<long> Handle(CreateTodoCommand request, CancellationToken cancellationToken)
        {
            var entity = new Todo()
            {
                Name = request.Name
            };

            _applicationDbContext.Todos.Add(entity);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
    }

}
