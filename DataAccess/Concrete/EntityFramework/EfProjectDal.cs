using DataAccess.Abstract;
using DataAccess.Concrete.Context;
using DataAccess.Repositories;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Concrete.EntityFramework;

public class EfProjectDal : GenericRepository<Project>, IProjectDal
{
    private readonly HRMSContext _hrmsContext;
    public EfProjectDal(HRMSContext context, HRMSContext hrmsContext) : base(context)
    {
        _hrmsContext = hrmsContext;
    }

    public List<Project> GetProjectsWithAppUser()
    {
        return _hrmsContext.Projects.Include(x=>x.AppUsers).ToList();
    }
}