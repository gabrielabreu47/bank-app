using Application.Dtos.Movement;
using AutoMapper;
using Application.Extensions;
using ClientDirectory.Domain.Entities;
using ClientDirectory.Domain.Enums;

namespace Application.Mappings;

public class MovementProfile : Profile
{
    public MovementProfile()
    {
        CreateMap<Movement, MovementDto>()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToEnum<MovementTypes>()))
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        
        CreateMap<CreateMovementDto, Movement>()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => (int?)src.Type ?? 0))
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}