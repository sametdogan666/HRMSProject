using Entities.Concrete;
using FluentValidation;

namespace Business.ValidationRules.FluentValidation.DepartmentValidator;

public class DepartmentValidator : AbstractValidator<Department>
{
    public DepartmentValidator()
    {
        RuleFor(x=>x.DepartmentName).NotEmpty().WithMessage("Departman adı boş geçilemez");
        RuleFor(x=>x.DepartmentName).MinimumLength(2).WithMessage("Departman 2 karakterden az olamaz");
        RuleFor(x=>x.DepartmentName).MaximumLength(100).WithMessage("Departman 100 karakterden fazla olamaz");
    }
}