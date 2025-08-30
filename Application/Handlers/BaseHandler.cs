using Application.Extensions;
using Application.Helpers;
using Application.Interfaces;
using AutoMapper;
using ClientDirectory.Domain.Common;
using ClientDirectory.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Handlers;

/// <summary>
/// Base handler for CRUD operations on entities.
/// </summary>
public class BaseHandler<T, TDto>(IRepository repository, IMapper mapper, ILogger<BaseHandler<T, TDto>> logger) 
    : IBaseHandler<T, TDto> where T : BaseEntity where TDto : class
{
    
    /// <summary>
    /// Gets a paged list of entities projected to DTOs, applying filters and pagination with mapping at query level.
    /// </summary>
    public async Task<Paged<TDto>> Get(Filter filter)
    {
        var query = repository.AsQueryable<T>();
        
        if (filter.Filters is not null)
        {
            query = query.Filter(filter.Filters);
        }
        
        var count = await repository.CountAsync(query);
        
        var (skip, take) = Paged<TDto>
            .GetPagination(filter.PageNumber, filter.PageSize);
        
        var pagedQuery = query
            .Skip(skip)
            .Take(take);
        
        var result = await repository
            .ExecuteQuery(pagedQuery);

        var mapping = mapper.Map<List<TDto>>(result);
        
        return Paged<TDto>.Create(mapping, count, filter.PageNumber, filter.PageSize);
    }
    
    /// <summary>
    /// Creates a new entity from the given DTO.
    /// </summary>
    public virtual async Task<string> Create(TDto dto)
    {
        var entity = mapper.Map<T>(dto);
        entity.Id = Guid.NewGuid().ToString();
        await repository.AddAsync(entity);
        return entity.Id;
    }

    /// <summary>
    /// Updates an entity by ID using the given DTO.
    /// Throws EntityNotFoundException if not found.
    /// </summary>
    public virtual async Task Update(string id, TDto dto)
    {
        var entity = await repository.FirstOrDefault<T>(e => e.Id == id);
        if (entity is null)
        {
            logger.LogWarning("Entity not found for id: {Id}", id);
            throw new EntityNotFoundException("Entity doesn't exists");
        }
        mapper.Map(dto, entity);
        await repository.UpdateAsync(entity);
    }

    /// <summary>
    /// Deletes an entity by ID.
    /// </summary>
    public virtual async Task Delete(string id)
    {
        await repository.DeleteAsync<T>(id);
    }
}