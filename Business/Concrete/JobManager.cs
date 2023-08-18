using Business.Abstract;
using Business.ValidationRules.FluentValidation.JobValidator;
using Core.Aspects.Autofac.Validation;
using DataAccess.Abstract;
using Entities.Concrete;

namespace Business.Concrete;

public class JobManager : IJobService
{
    private readonly IJobDal _jobDal;

    public JobManager(IJobDal jobDal)
    {
        _jobDal = jobDal;
    }

    [ValidationAspect(typeof(JobValidator))]
    public void Insert(Job entity)
    {
        _jobDal.Insert(entity);
    }

    [ValidationAspect(typeof(JobValidator))]
    public void Update(Job entity)
    {
        _jobDal.Update(entity);
    }

    public void Delete(Job entity)
    {
        _jobDal.Delete(entity);
    }

    public List<Job> GetAll()
    {
        return _jobDal.GetAll();
    }

    public Job? GetById(int id)
    {
        return _jobDal.GetById(id);
    }
}