using Business.Abstract;
using Business.ValidationRules.FluentValidation.AnnouncementValidator;
using Core.Aspects.Autofac.Validation;
using DataAccess.Abstract;
using Entities.Concrete;

namespace Business.Concrete;

public class AnnouncementManager : IAnnouncementService
{
    private readonly IAnnouncementDal _announcementDal;

    public AnnouncementManager(IAnnouncementDal announcementDal)
    {
        _announcementDal = announcementDal;
    }

    [ValidationAspect(typeof(AnnouncementValidator))]
    public void Insert(Announcement entity)
    {
        _announcementDal.Insert(entity);
    }

    [ValidationAspect(typeof(AnnouncementValidator))]
    public void Update(Announcement entity)
    {
        _announcementDal.Update(entity);
    }

    public void Delete(Announcement entity)
    {
        _announcementDal.Delete(entity);
    }

    public List<Announcement> GetAll()
    {
        return _announcementDal.GetAll();
    }

    public Announcement? GetById(int id)
    {
        return _announcementDal.GetById(id);
    }
}