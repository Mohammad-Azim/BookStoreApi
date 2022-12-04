using System.Linq.Expressions;

namespace BookStoreApi.Interfaces;

public interface IGenericRepo<T> where T : class
{
    Task<List<T>> GetAsync();
    Task<T?> GetAsync(string id, Expression<Func<T, bool>> filterExpression);

    Task CreateAsync(T newBook);

    Task UpdateAsync(string id, T updatedItem, Expression<Func<T, bool>> filterExpression);

    Task RemoveAsync(string id, Expression<Func<T, bool>> filterExpression);

}