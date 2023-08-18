using DataAccess.Abstract;
using DataAccess.Concrete.Context;
using DataAccess.Repositories;
using Entities.Concrete;

namespace DataAccess.Concrete.EntityFramework;

public class EfPaySlipDal : GenericRepository<PaySlip>, IPaySlipDal
{
    public EfPaySlipDal(HRMSContext context) : base(context)
    {
    }
    
}