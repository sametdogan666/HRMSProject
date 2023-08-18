using Business.Abstract;
using Business.ValidationRules.FluentValidation.OffDayValidator;
using Core.Aspects.Autofac.Validation;
using DataAccess.Abstract;
using Entities.Concrete;

namespace Business.Concrete;

public class OffDayManager : IOffDayService
{
    private readonly IOffDayDal _offDayDal;

    public OffDayManager(IOffDayDal offDayDal)
    {
        _offDayDal = offDayDal;
    }

    [ValidationAspect(typeof(OffDayValidator))]
    public void Insert(OffDay entity)
    {
        _offDayDal.Insert(entity);
    }

    [ValidationAspect(typeof(OffDayValidator))]
    public void Update(OffDay entity)
    {
        _offDayDal.Update(entity);
    }

    public void Delete(OffDay entity)
    {
        _offDayDal.Delete(entity);
    }

    public List<OffDay> GetAll()
    {
        return _offDayDal.GetAll();
    }

    public OffDay? GetById(int id)
    {
        return _offDayDal.GetById(id);
    }

    public List<OffDay> GetOffDaysWithUserName()
    {
        return _offDayDal.GetOffDaysWithUserName();
    }
}