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

            // <Origen, Destino>
            CreateMap<HistoryDto, History>()
                .ForMember(dest => dest.Id,
                            opt => opt.Ignore())            
                .ReverseMap();

            CreateMap<HistoryForCreationDto, History>()
                .ForMember(dest => dest.Id,
                            opt => opt.Ignore());


            CreateMap<History, HistoryDto>()
                .ForMember(dest => dest.Origin, 
                            opt => opt.MapFrom(src => src.Origin.Code))
            .ForMember(dest => dest.Destination,
                            opt => opt.MapFrom(src => src.Destination.Code));



        }
    }
}
