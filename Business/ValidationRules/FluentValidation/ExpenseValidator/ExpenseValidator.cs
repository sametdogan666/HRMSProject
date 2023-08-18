using Entities.Concrete;
using FluentValidation;

namespace Business.ValidationRules.FluentValidation.ExpenseValidator;

public class ExpenseValidator : AbstractValidator<Expense>
{
    public ExpenseValidator()
    {
        RuleFor(x=>x.Title).NotEmpty().WithMessage("Başlık alanı boş geçilemez");
        RuleFor(x=>x.Title).MinimumLength(3).WithMessage("Başlık alanı en az 3 karakterden oluşmalıdır");
        RuleFor(x=>x.Title).MaximumLength(100).WithMessage("Başlık alanı en fazla 100 karakterden oluşmalıdır");

        RuleFor(x => x.Description).NotEmpty().WithMessage("Açıklama alanı boş geçilemez");
        RuleFor(x => x.Description).MinimumLength(3).WithMessage("Açıklama alanı en az 3 karakterden oluşmalıdır");
        RuleFor(x => x.Description).MaximumLength(1000).WithMessage("Açıklama alanı en fazla 1000 karakterden oluşmalıdır");

        RuleFor(x => x.Amount).NotEmpty().WithMessage("Miktar alanı boş geçilemez");
        RuleFor(x => x.Amount).GreaterThanOrEqualTo(0).WithMessage("Tutar alanı 0'dan büyük veya eşit olmalıdır");
    }
}