using Application.Dtos.Client;
using Application.Interfaces;
using AutoMapper;
using Application.Extensions;
using Application.Helpers;
using ClientDirectory.Domain.Common;
using ClientDirectory.Domain.Entities;
using ClientDirectory.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Handlers;

/// <summary>
/// Handles client-related operations and queries.
/// </summary>
public class ClientHandler(IRepository repository, IMapper mapper, ILogger<ClientHandler> logger) 
    : BaseHandler<Client, CreateClientDto>(repository, mapper, logger), IClientHandler
{
    private readonly IRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    /// <summary>
    /// Gets paged clients with optional filters.
    /// </summary>
    public async Task<Paged<ClientDto>> Get(Filter filter)
    {
        var query = _repository.AsQueryable<Client>();
        if (filter.Filters is not null)
        {
            query = query.Filter(filter.Filters);
        }
        var count = await _repository.CountAsync(query);
        var (skip, take) = Paged<ClientDto>.GetPagination(filter.PageNumber, filter.PageSize);
        var pagedQuery = query.Skip(skip).Take(take);
        var result = await _repository.ProjectToAsync<Client, ClientDto>(pagedQuery);
        return Paged<ClientDto>.Create(result, count, filter.PageNumber, filter.PageSize);
    }

    /// <summary>
    /// Gets a client by its ID.
    /// Throws ClientNotFoundException if not found.
    /// </summary>
    public async Task<ClientDto> Get(string id)
    {
        var result = await _repository.FirstOrDefault<Client>(c => c.Id == id);
        if (result is null)
        {
            logger.LogWarning("Client not found for id: {Id}", id);
            throw new ClientNotFoundException("Client doesn't exists");
        }
        logger.LogInformation("Client retrieved for id: {Id}", id);
        return (ClientDto)result;
    }
}