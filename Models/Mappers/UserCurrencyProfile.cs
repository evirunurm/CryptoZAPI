using AutoMapper;
using CryptoZAPI.Models;
using Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Mappers {
    public class UserCurrencyProfile : Profile {
        public UserCurrencyProfile() {

            
            CreateMap<UserCurrency, UserCurrencyForViewDto>()
              .ForMember(dest => dest.Name,
                          opt => opt.MapFrom(src => src.Name))
              .ReverseMap();

      


        }
    }
}
