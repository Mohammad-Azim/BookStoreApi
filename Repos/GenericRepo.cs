using BookStoreApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using BookStoreApi.Interfaces;
using System.Linq.Expressions;

namespace BookStoreApi.Repos;
public class GenericRepo<T> : IGenericRepo<T> where T : class
{

    private readonly IMongoCollection<T> _Collection;

    public GenericRepo(
        IOptions<BookStoreDatabaseSettings> bookStoreDatabaseSettings)
    {
        var mongoClient = new MongoClient(
                bookStoreDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            bookStoreDatabaseSettings.Value.DatabaseName);

        _Collection = mongoDatabase.GetCollection<T>(
            bookStoreDatabaseSettings.Value.BooksCollectionName);
    }

    public async Task<List<T>> GetAsync() =>
        await _Collection.Find(_ => true).ToListAsync();

    public async Task<T?> GetAsync(string id, Expression<Func<T, bool>> filterExpression) =>
        await _Collection.Find(filterExpression).FirstOrDefaultAsync();

    public async Task CreateAsync(T newBook) =>
        await _Collection.InsertOneAsync(newBook);

    public async Task UpdateAsync(string id, T updatedBook, Expression<Func<T, bool>> filterExpression) =>
        await _Collection.ReplaceOneAsync(filterExpression, updatedBook);

    public async Task RemoveAsync(string id, Expression<Func<T, bool>> filterExpression) =>
        await _Collection.DeleteOneAsync(filterExpression);
}
