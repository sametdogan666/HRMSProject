using Business.Abstract;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class ProjectManager:IProjectService
    {
        private readonly IProjectDal _projectDal;

        public ProjectManager(IProjectDal projectDal)
        {
            _projectDal = projectDal;
        }

        public void Delete(Project entity)
        {
            _projectDal.Delete(entity);
        }

        public List<Project> GetAll()
        {
            return _projectDal.GetAll();
        }

        public Project GetById(int id)
        {
            return _projectDal.GetById(id);
        }

        public List<Project> GetProjectsWithAppUser()
        {
            return _projectDal.GetProjectsWithAppUser();
        }

        public void Insert(Project entity)
        {
            _projectDal.Insert(entity);
        }

        public void Update(Project entity)
        {
            _projectDal.Update(entity);
        }
    }
}
