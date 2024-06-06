using AutoMapper;
using EkonLayer.Core.DbModels;
using EkonLayer.Core.LogModels;
using EkonLayer.Helpers.Models.Dtos.DbModelDtos;
using EkonLayer.Helpers.Models.Dtos.LogModelDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EkonLayer.Service.Mapping
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<Application, ApplicationDto>().ReverseMap();
            CreateMap<UserLog, UserLogDto>().ReverseMap();
            CreateMap<ErrorLog, ErrorLogDto>().ReverseMap();
            CreateMap<ApplicationLog, ApplicationLogDto>().ReverseMap();
            CreateMap<Stock, StockDto>().ReverseMap();


        }
    }
}
