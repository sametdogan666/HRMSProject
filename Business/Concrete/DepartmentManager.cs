using Business.Abstract;
using Business.ValidationRules.FluentValidation.DepartmentValidator;
using Core.Aspects.Autofac.Validation;
using DataAccess.Abstract;
using Entities.Concrete;

namespace Business.Concrete;

public class DepartmentManager : IDepartmentService
{
    private readonly IDepartmentDal _departmentDal;

    public DepartmentManager(IDepartmentDal departmentDal)
    {
        _departmentDal = departmentDal;
    }

    [ValidationAspect(typeof(DepartmentValidator))]
    public void Insert(Department entity)
    {
        _departmentDal.Insert(entity);
    }

    [ValidationAspect(typeof(DepartmentValidator))]
    public void Update(Department entity)
    {
        _departmentDal.Update(entity);
    }

    public void Delete(Department entity)
    {
        _departmentDal.Delete(entity);
    }

    public List<Department> GetAll()
    {
        return _departmentDal.GetAll();
    }

    public Department? GetById(int id)
    {
        return _departmentDal.GetById(id);
    }
}