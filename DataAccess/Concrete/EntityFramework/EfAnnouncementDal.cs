using DataAccess.Abstract;
using DataAccess.Concrete.Context;
using DataAccess.Repositories;
using Entities.Concrete;

namespace DataAccess.Concrete.EntityFramework;

public class EfAnnouncementDal : GenericRepository<Announcement>, IAnnouncementDal
{
    public EfAnnouncementDal(HRMSContext context) : base(context)
    {
    }
}