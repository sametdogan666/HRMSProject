using Business.Abstract;
using Core.Aspects.Autofac.Validation;
using DataAccess.Abstract;
using Entities.Concrete;

namespace Business.Concrete;

public class PaySlipManager : IPaySlipService
{
    private readonly IPaySlipDal _paySlipDal;

    public PaySlipManager(IPaySlipDal paySlipDal)
    {
        _paySlipDal = paySlipDal;
    }

    public void Insert(PaySlip entity)
    {
        _paySlipDal.Insert(entity);
    }

    public void Update(PaySlip entity)
    {
        _paySlipDal.Update(entity);
    }

    public void Delete(PaySlip entity)
    {
        _paySlipDal.Delete(entity);
    }

    public List<PaySlip> GetAll()
    {
        return _paySlipDal.GetAll();
    }

    public PaySlip? GetById(int id)
    {
        return _paySlipDal.GetById(id);
    }
}