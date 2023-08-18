using Entities.Concrete;
using FluentValidation;

namespace Business.ValidationRules.FluentValidation.AnnouncementValidator;

public class AnnouncementValidator : AbstractValidator<Announcement>
{
    public AnnouncementValidator()
    {
        RuleFor(x=>x.Title).NotEmpty().WithMessage("Başlık alanı boş geçilemez");
        RuleFor(x=>x.Title).MinimumLength(3).WithMessage("Başlık alanı en a 3 karakterden oluşmalıdır");
        RuleFor(x=>x.Title).MaximumLength(100).WithMessage("Başlık alanı en fazla 100 karakterden oluşmalıdır");

        RuleFor(x => x.Content).NotEmpty().WithMessage("İçerik alanı boş geçilemez");
        RuleFor(x => x.Content).MinimumLength(3).WithMessage("İçerik alanı en a 3 karakterden oluşmalıdır");
        RuleFor(x => x.Content).MaximumLength(1000).WithMessage("İçerik alanı en fazla 1000 karakterden oluşmalıdır");

    }
}