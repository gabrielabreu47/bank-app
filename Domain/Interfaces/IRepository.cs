using System.Linq.Expressions;
using ClientDirectory.Domain.Common;

namespace ClientDirectory.Domain.Interfaces;

public interface IRepository
{
    IQueryable<TEntity> AsQueryable<TEntity>() where TEntity : BaseEntity;
    Task<List<TEntity>> ExecuteQuery<TEntity>(Expression<Func<TEntity, bool>> filter) where TEntity : BaseEntity;
    Task<List<TEntity>> ExecuteQuery<TEntity>(IQueryable<TEntity> query) where TEntity : BaseEntity;
    Task<TEntity?> FirstOrDefault<TEntity>(Expression<Func<TEntity, bool>> filter) where TEntity : BaseEntity;
    Task<int> CountAsync<TEntity>(IQueryable<TEntity> query) where TEntity : BaseEntity;
    Task AddAsync<TEntity>(TEntity entity) where TEntity : BaseEntity;
    Task AddRangeAsync<TEntity>(IEnumerable<TEntity> entities) where TEntity : BaseEntity;
    Task UpdateAsync<TEntity>(TEntity entity) where TEntity : BaseEntity;
    Task DeleteAsync<TEntity>(string id) where TEntity : BaseEntity;
}
