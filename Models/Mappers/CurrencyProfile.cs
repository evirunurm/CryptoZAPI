using AutoMapper;
using CryptoZAPI.Models;
using Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Mappers {
    public class CurrencyProfile : Profile {
        public CurrencyProfile() {

            // <Origen, Destino>
            CreateMap<CurrencyForCreationDto, Currency>()
                .ForMember(dest => dest.Id,
                            opt => opt.Ignore())
                .ForMember(dest => dest.Code,
                            opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.PriceDate,
                            opt => opt.MapFrom(src => src.price_date))
                .ForMember(dest => dest.LogoUrl,
                            opt => opt.MapFrom(src => src.logo_url))
                .ReverseMap();
            
            CreateMap<CurrencyForCreationDto, CurrencyForViewDto>()
              .ForMember(dest => dest.Code,
                          opt => opt.MapFrom(src => src.Id))
              .ForMember(dest => dest.PriceDate,
                          opt => opt.MapFrom(src => src.price_date))
              .ForMember(dest => dest.LogoUrl,
                          opt => opt.MapFrom(src => src.logo_url))
              .ReverseMap();

            CreateMap<CurrencyForViewDto, Currency>()
               .ForMember(dest => dest.Id,
                           opt => opt.Ignore())
               .ReverseMap();


        }
    }
}
