using System.Linq.Expressions;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IGenericRepository<T> where T: BaseEntity
    {
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        IEnumerable<T> Find(Expression<Func<T, bool>> expression);
        Task<(int totalRegistros, IEnumerable<T> registros)> GetAllAsync(int PageIndex, int PageSize, string search);
        void Add(T entity);
        void AddRange (IEnumerable<T> entities);
        void Remove (T entity);
        void RemoveRange (IEnumerable<T> entities);
        void Update (T entity);
    }
}