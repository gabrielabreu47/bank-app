using System.Linq.Expressions;
using ClientDirectory.Domain.Common;
using ClientDirectory.Domain.Interfaces;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class Repository(IClientDirectoryDbContext context) : IRepository
{
    public IQueryable<TEntity> AsQueryable<TEntity>() where TEntity : BaseEntity
    {
        return context.Set<TEntity>().AsQueryable();
    }

    public async Task<List<TEntity>> ExecuteQuery<TEntity>(Expression<Func<TEntity, bool>> filter) where TEntity : BaseEntity
    {
        return await context.Set<TEntity>().Where(filter).ToListAsync();
    }

    public async Task<List<TEntity>> ExecuteQuery<TEntity>(IQueryable<TEntity> query) where TEntity : BaseEntity
    {
        return await query.ToListAsync();
    }

    public async Task<TEntity?> FirstOrDefault<TEntity>(Expression<Func<TEntity, bool>> filter) where TEntity : BaseEntity
    {
        return await context.Set<TEntity>().FirstOrDefaultAsync(filter);
    }

    public async Task<int> CountAsync<TEntity>(IQueryable<TEntity> query) where TEntity : BaseEntity
    {
        return await query.CountAsync();
    }

    public async Task AddAsync<TEntity>(TEntity entity) where TEntity : BaseEntity
    {
        await context.Set<TEntity>().AddAsync(entity);
        await context.SaveChangesAsync();
    }

    public async Task AddRangeAsync<TEntity>(IEnumerable<TEntity> entities) where TEntity : BaseEntity
    {
        await context.Set<TEntity>().AddRangeAsync(entities);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync<TEntity>(TEntity entity) where TEntity : BaseEntity
    {
        context.Set<TEntity>().Update(entity);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync<TEntity>(string id) where TEntity : BaseEntity
    {
        var entity = await context.Set<TEntity>().FindAsync(id);
        if (entity != null)
        {
            context.Set<TEntity>().Remove(entity);
            await context.SaveChangesAsync();
        }
    }

    /// <summary>
    /// Projects entities to DTOs using implicit operators.
    /// </summary>
    /// <typeparam name="TEntity">Entity type.</typeparam>
    /// <typeparam name="TResult">DTO type with implicit operator from TEntity.</typeparam>
    /// <param name="query">Entity query.</param>
    /// <returns>List of projected DTOs.</returns>
    public async Task<List<TResult>> ProjectToAsync<TEntity, TResult>(IQueryable<TEntity> query)
        where TEntity : BaseEntity
    {
        // Uses implicit operator for projection
        return await query.Select(e => (TResult)(object)e).ToListAsync();
    }

    /// <summary>
    /// Projects entities to DTOs with explicit includes and implicit operator mapping.
    /// </summary>
    /// <typeparam name="TEntity">Entity type.</typeparam>
    /// <typeparam name="TResult">DTO type with implicit operator from TEntity.</typeparam>
    /// <param name="includes">List of navigation properties to include.</param>
    /// <param name="filter">Optional filter expression.</param>
    /// <returns>List of projected DTOs.</returns>
    public async Task<List<TResult>> ProjectToWithIncludesAsync<TEntity, TResult>(
        List<Expression<Func<TEntity, object>>> includes,
        Expression<Func<TEntity, bool>>? filter = null)
        where TEntity : BaseEntity
    {
        IQueryable<TEntity> query = context.Set<TEntity>();
        if (filter != null)
            query = query.Where(filter);
        foreach (var include in includes)
            query = query.Include(include);
        return await query.Select(e => (TResult)(object)e).ToListAsync();
    }

    /// <summary>
    /// Executes a projection query and returns the result list.
    /// </summary>
    /// <typeparam name="TResult">Projection result type.</typeparam>
    /// <param name="query">Projection query.</param>
    /// <returns>List of results.</returns>
    public async Task<List<TResult>> ExecuteProjection<TResult>(IQueryable<TResult> query)
    {
        return await query.ToListAsync();
    }
}