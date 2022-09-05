using AutoMapper;
using CryptoZAPI.Models;
using Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Mappers {
    public class CountryProfile : Profile {
        public CountryProfile() {

            // <Origen, Destino>
            CreateMap<CountryForCreationDto, Country>()
                .ForMember(dest => dest.Id,
                            opt => opt.Ignore())
                .ForMember(dest => dest.CountryCode,
                            opt => opt.MapFrom(src => src.alpha2Code))
                .ForMember(dest => dest.Name,
                            opt => opt.MapFrom(src => src.name))
                .ReverseMap();

            CreateMap<Country, CountryForViewDto>()
               .ForMember(dest => dest.Id,
                           opt => opt.Ignore())
               .ReverseMap();


        }
    }
}
