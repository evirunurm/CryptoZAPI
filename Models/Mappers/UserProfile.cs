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
        public UserProfile () {

            // <Origen, Destino>
            CreateMap<UserForCreationDto, User>()
                .ForMember(dest => dest.Id,
                            opt => opt.Ignore())            
                .ReverseMap();

            CreateMap<UserForCreationDto, UserForViewDto>().ReverseMap();

            CreateMap<UserForViewDto, User>()
                .ForMember(dest => dest.Id,
                            opt => opt.Ignore())
                .ReverseMap();
        }
    }
}
