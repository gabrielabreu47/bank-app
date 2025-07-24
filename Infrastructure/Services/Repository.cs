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
}