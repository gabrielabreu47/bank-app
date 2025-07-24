using Application.Interfaces;
using AutoMapper;
using ClientDirectory.Domain.Common;
using ClientDirectory.Domain.Interfaces;

namespace Application.Handlers;

public class BaseHandler<T, TDto>(IRepository repository, IMapper mapper) 
    : IBaseHandler<T, TDto> where T : BaseEntity where TDto : class
{
    public virtual async Task<string> Create(TDto dto)
    {
        var entity = mapper.Map<T>(dto);
        entity.Id = Guid.NewGuid().ToString();
        await repository.AddAsync(entity);
        return entity.Id;
    }

    public virtual async Task Update(string id, TDto dto)
    {
        var entity = await repository.FirstOrDefault<T>(e => e.Id == id);
        if (entity is null) throw new Exception("Entity doesn't exists");
        mapper.Map(dto, entity);
        await repository.UpdateAsync(entity);
    }

    public virtual async Task Delete(string id)
    {
        await repository.DeleteAsync<T>(id);
    }
}