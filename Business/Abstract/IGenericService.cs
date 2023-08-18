namespace Business.Abstract;

public interface IGenericService<T> where T : class
{
    void Insert(T entity);
    void Update(T entity);
    void Delete(T entity);
    List<T> GetAll();
    T GetById(int id);
}