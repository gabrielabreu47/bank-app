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
    Task UpdateAsync<TEntity>(TEntity entity) where TEntity : BaseEntity;
    Task DeleteAsync<TEntity>(string id) where TEntity : BaseEntity;
    Task<List<TResult>> ProjectToAsync<TEntity, TResult>(IQueryable<TEntity> query) where TEntity : BaseEntity;
    Task<List<TResult>> ProjectToWithIncludesAsync<TEntity, TResult>(List<Expression<Func<TEntity, object>>> includes, Expression<Func<TEntity, bool>>? filter = null) where TEntity : BaseEntity;
}
