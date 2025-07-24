using Application.Dtos.Client;
using Application.Interfaces;
using AutoMapper;
using Application.Extensions;
using Application.Helpers;
using ClientDirectory.Domain.Common;
using ClientDirectory.Domain.Entities;
using ClientDirectory.Domain.Interfaces;

namespace Application.Handlers;

public class ClientHandler(IRepository repository, IMapper mapper) 
    : BaseHandler<Client, CreateClientDto>(repository, mapper), IClientHandler
{
    private readonly IRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<Paged<ClientDto>> Get(Filter filter)
    {
        var query = _repository.AsQueryable<Client>();
        
        if (filter.Filters is not null)
        {
            query = query.Filter(filter.Filters);
        }

        var count = await _repository.CountAsync(query);

        var (skip, take) = Paged<ClientDto>.GetPagination(filter.PageNumber, filter.PageSize);

        query = query.Skip(skip).Take(take);

        var response = await _repository.ExecuteQuery(query);

        var result = _mapper.Map<List<ClientDto>>(response);

        return Paged<ClientDto>.Create(result, count, filter.PageNumber, filter.PageSize);
    }
}