using DataAccess.Abstract;
using DataAccess.Concrete.Context;
using DataAccess.Repositories;
using Entities.Concrete;

namespace DataAccess.Concrete.EntityFramework;

public class EfJobDal : GenericRepository<Job>, IJobDal
{
    public EfJobDal(HRMSContext context) : base(context)
    {
    }
}