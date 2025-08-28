using System.Linq.Expressions;

namespace Vneed.Repositories.Interfaces;

public interface IGenericRepository<T> where T : class
{
    Task<T> GetById(int id);
    Task<IEnumerable<T>> GetAll();
    Task<IEnumerable<T>> GetWhere(Expression<Func<T, bool>> predicate);
    Task<T> Add(T entity);
    Task<T> Update(T entity);
    Task<bool> Delete(int id);

    //Paging için kullandık.
    IQueryable<T> GetAllQueryable();
}