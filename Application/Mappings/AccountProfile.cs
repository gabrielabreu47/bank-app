using Application.Dtos.Account;
using AutoMapper;
using Application.Extensions;
using ClientDirectory.Domain.Entities;
using ClientDirectory.Domain.Enums;

namespace Application.Mappings;

public class AccountProfile : Profile
{
    public AccountProfile()
    {
        CreateMap<Account, AccountDto>()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToEnum<AccountTypes>()))
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        
        CreateMap<AccountDto, Account>()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => (int?)src.Type ?? 0))
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}