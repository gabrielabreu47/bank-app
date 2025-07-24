using Application.Dtos.Client;
using AutoMapper;
using Application.Extensions;
using ClientDirectory.Domain.Entities;
using ClientDirectory.Domain.Enums;

namespace Application.Mappings;

public class ClientProfile : Profile
{
    public ClientProfile()
    {
        CreateMap<Client, ClientDto>()
            .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.ToEnum<Genders>()))
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        
        CreateMap<ClientDto, Client>()
            .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => (int?)src.Gender ?? 0))
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        
        CreateMap<CreateClientDto, Client>()
            .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => (int?)src.Gender ?? 0))
            .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}