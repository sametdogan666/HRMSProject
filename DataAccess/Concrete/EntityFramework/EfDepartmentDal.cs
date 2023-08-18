using DataAccess.Abstract;
using DataAccess.Concrete.Context;
using DataAccess.Repositories;
using Entities.Concrete;

namespace DataAccess.Concrete.EntityFramework;

public class EfDepartmentDal : GenericRepository<Department>, IDepartmentDal
{
    public EfDepartmentDal(HRMSContext context) : base(context)
    {
    }
}