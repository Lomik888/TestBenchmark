using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using TestBenchmark.Entities;

namespace TestBenchmark
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<PeopleClass, PeopleEntity>();
            CreateMap<OrganizationClass, OrganizationEntity>();
            CreateMap<ComplimentClass, ComplimentEntity>();
        }
    }
}