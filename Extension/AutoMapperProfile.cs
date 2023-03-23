using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tutor1.Models.DTO;
using tutor1.Models.Entity;

namespace tutor1.Extension
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ClinicOrder, ClinicOrderDTO>()               
                .ReverseMap(); 
        }
    }
}
