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
                            opt => opt.Ignore()) // TODO: En lugar  de Ignorar, calcular/buscar el Id que le corresponde (o ignorarlo como ahora)
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

            // Quizá no haga falta ¿?
            CreateMap<CurrencyForViewDto, Currency>()
               .ForMember(dest => dest.Id,
                           opt => opt.Ignore()) // TODO: En lugar  de Ignorar, calcular/buscar el Id que le corresponde (o ignorarlo como ahora)              
               .ReverseMap();
        }
    }
}
