using Application.Dtos.Client;
using Application.Helpers;
using ClientDirectory.Domain.Common;
using ClientDirectory.Domain.Entities;

namespace Application.Interfaces;

public interface IClientHandler : IBaseHandler<Client, CreateClientDto>
{
    Task<Paged<ClientDto>> Get(Filter filter);
    Task<ClientDto> Get(string id);
}