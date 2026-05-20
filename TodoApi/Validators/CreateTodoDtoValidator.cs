using FluentValidation;
using TodoApi.DTOs.TodoDTOs;

namespace TodoApi.Validators;

public class CreateTodoDtoValidator : AbstractValidator<CreateTodoDto>
{
    public CreateTodoDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(100);

        RuleFor(x => x.Description)
            .MaximumLength(500);

        RuleFor(x => x.Priority)
            .IsInEnum();

        RuleFor(x => x.Category)
            .IsInEnum();
    }
}