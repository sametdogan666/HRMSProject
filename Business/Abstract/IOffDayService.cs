using Entities.Concrete;

namespace Business.Abstract;

public interface IOffDayService : IGenericService<OffDay>
{
    List<OffDay> GetOffDaysWithUserName();
}