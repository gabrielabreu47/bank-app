using ClientDirectory.Domain.Common;

namespace Application.Interfaces;

public interface IBaseHandler<in T, in TDto> where T : IBase where TDto : class
{
    Task<string> Create(TDto dto);
    Task Update(string id, TDto dto);
    Task Delete(string id);
}