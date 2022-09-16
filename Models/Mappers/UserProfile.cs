using AutoMapper;
using CryptoZAPI.Models;
using Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Mappers {
    public class UserProfile : Profile {
        public UserProfile() {

            // <Origen, Destino>

            /* CreationDTO */
            CreateMap<UserForCreationDto, User>()
                .ForMember(dest => dest.Id,
                            opt => opt.Ignore())
                .ForMember(dest => dest.FullName,
                            opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.UserName,
                            opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.FullName,
                            opt => opt.MapFrom(src => src.Name))
                .ReverseMap();

            /* ViewDTO */
            CreateMap<UserForViewDto, User>()
                .ForMember(dest => dest.Id,
                            opt => opt.Ignore());

            CreateMap<User, UserForViewDto>()
                .ForMember(dest => dest.CountryCode,
                            opt => opt.MapFrom(src => src.Country.CountryCode))
                .ForMember(dest => dest.Name,
                            opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.CountryName,
                            opt => opt.MapFrom(src => src.Country.Name));

            /* UpdateDTO */
            CreateMap<UserForUpdateDto, User>()
                .ForMember(dest => dest.Id,
                            opt => opt.Ignore())
                  .ForMember(dest => dest.FullName,
                            opt => opt.MapFrom(src => src.Name))
                .ReverseMap();
        }
    }
}
