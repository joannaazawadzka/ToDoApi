using FluentValidation;

namespace ToDo.Core.Api.Models.ToDoTasksManagement.UpdateToDoTask
{
    /// <summary>
    /// Validator for UpdateToDoTaskRequest to ensure valid task updates.
    /// </summary>
    internal class UpdateToDoTaskRequestValidator : AbstractValidator<UpdateToDoTaskRequest>
    {
        public UpdateToDoTaskRequestValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(100).WithMessage("Title cannot exceed 100 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(5000).WithMessage("Description cannot exceed 5000 characters.");

            RuleFor(x => x.ExpiryAt)
                .NotEmpty().WithMessage("Expiry date is required.");

            RuleFor(x => x.CompletionPercentage)
                .GreaterThanOrEqualTo(0).WithMessage("Completion percentage must be between 0 and 100.")
                .LessThanOrEqualTo(100).WithMessage("Completion percentage must be between 0 and 100.");
        }
    }
}
