using FluentValidation;

namespace ToDo.Core.Api.Models.ToDoTasksManagement.CreateToDoTask
{
    /// <summary>
    /// Validator for CreateToDoTaskRequest to enforce business rules and validation.
    /// </summary>
    internal class CreateToDoTaskRequestValidator : AbstractValidator<CreateToDoTaskRequest>
    {
        public CreateToDoTaskRequestValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(100).WithMessage("Title cannot exceed 100 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(5000).WithMessage("Description cannot exceed 5000 characters.");

            RuleFor(x => x.ExpiryAt)
                .NotEmpty().WithMessage("Expiry date is required.");
        }
    }
}
