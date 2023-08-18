using Entities.Concrete;
using FluentValidation;

namespace Business.ValidationRules.FluentValidation.JobValidator;

public class JobValidator : AbstractValidator<Job>
{
    public JobValidator()
    {
        RuleFor(x=>x.JobName).NotEmpty().WithMessage("Meslek alanı boş geçilemez");
        RuleFor(x=>x.JobName).MinimumLength(2).WithMessage("Meslek alanı en az 2 karakterden oluşmalıdır");
        RuleFor(x=>x.JobName).MaximumLength(100).WithMessage("Meslek alanı 100 karakterden oluşmalıdır");
    }
}