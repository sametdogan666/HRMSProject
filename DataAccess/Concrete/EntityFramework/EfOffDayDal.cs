using DataAccess.Abstract;
using DataAccess.Concrete.Context;
using DataAccess.Repositories;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Concrete.EntityFramework;

public class EfOffDayDal : GenericRepository<OffDay>, IOffDayDal
{
    private readonly HRMSContext _context;
    public EfOffDayDal(HRMSContext context) : base(context)
    {
        _context = context;
    }

    public List<OffDay> GetOffDaysWithUserName()
    {
        return _context.OffDays.Include(x=>x.AppUser).ToList();
    }
}