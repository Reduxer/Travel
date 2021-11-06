using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;

namespace Travel.Application.Todos.Commands.CreateTodo
{
    public class CreateTodoCommandValidator : AbstractValidator<CreateTodoCommand>
    {
        public CreateTodoCommandValidator()
        {
            RuleFor(v => v.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(60).WithMessage("Name must not exceed 60 characters");
        }
    }
}
