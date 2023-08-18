using DataAccess.Repositories;
using Entities.Concrete;

namespace DataAccess.Abstract;

public interface IOffDayDal : IGenericRepository<OffDay>
{
    List<OffDay> GetOffDaysWithUserName();
}