using AutoMapper;
using CryptoZAPI.Models;
using Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Mappers {
    public class HistoryProfile : Profile {
        public HistoryProfile() {
            // Origen, Destino
            CreateMap<HistoryForCreationDto, History>()
                .ForMember(dest => dest.Id,
                            opt => opt.Ignore())
                .ReverseMap();

            CreateMap<History, HistoryForViewDto>()
                .ForMember(dest => dest.OriginCode,
                            opt => opt.MapFrom(src => src.Origin.Name))
                .ForMember(dest => dest.DestinationCode,
                            opt => opt.MapFrom(src => src.Destination.Name))
                .ReverseMap();



        }
    }
}
