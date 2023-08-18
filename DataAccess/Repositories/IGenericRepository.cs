using System.Linq.Expressions;

namespace DataAccess.Repositories;

public interface IGenericRepository<T> where T : class
{
    void Insert(T entity);
    void Update(T entity);
    void Delete(T entity);
    List<T> GetAll();
    //List<T> FindByCondition(Expression<Func<T, bool>> expression);
    T? GetById(int id);
}