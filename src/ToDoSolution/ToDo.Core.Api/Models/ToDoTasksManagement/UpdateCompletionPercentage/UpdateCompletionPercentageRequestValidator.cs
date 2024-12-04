using FluentValidation;

namespace ToDo.Core.Api.Models.ToDoTasksManagement.UpdateCompletionPercentage
{
    /// <summary>
    /// Validator for UpdateCompletionPercentageRequest to ensure valid completion percentage.
    /// </summary>
    internal class UpdateCompletionPercentageRequestValidator : AbstractValidator<UpdateCompletionPercentageRequest>
    {
        public UpdateCompletionPercentageRequestValidator()
        {
            RuleFor(x => x.CompletionPercentage)
                .GreaterThanOrEqualTo(0).WithMessage("Completion percentage must be between 0 and 100.")
                .LessThanOrEqualTo(100).WithMessage("Completion percentage must be between 0 and 100.");
        }
    }
}
