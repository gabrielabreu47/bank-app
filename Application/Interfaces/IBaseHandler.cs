using Application.Helpers;
using ClientDirectory.Domain.Common;

namespace Application.Interfaces;

public interface IBaseHandler<in T, TDto> where T : IBase where TDto : class
{
    Task<Paged<TDto>> Get(Filter filter);
    Task<string> Create(TDto dto);
    Task Update(string id, TDto dto);
    Task Delete(string id);
}