using Entities.Concrete;
using FluentValidation;

namespace Business.ValidationRules.FluentValidation.OffDayValidator;

public class OffDayValidator : AbstractValidator<OffDay>
{
    public OffDayValidator()
    {
        RuleFor(x=>x.Description).NotEmpty().WithMessage("Açıklama alanı boş geçilemez");
        RuleFor(x=>x.Description).MinimumLength(3).WithMessage("Açıklama alanı en az 3 karakterden oluşmalıdır");
        RuleFor(x=>x.Description).MaximumLength(250).WithMessage("Açıklama alanı en fazla 250 karakterden oluşmalıdır");
    }
}