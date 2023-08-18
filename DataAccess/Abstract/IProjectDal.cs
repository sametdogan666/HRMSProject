using DataAccess.Repositories;
using Entities.Concrete;

namespace DataAccess.Abstract;

public interface IProjectDal : IGenericRepository<Project>
{
    List<Project> GetProjectsWithAppUser();
}