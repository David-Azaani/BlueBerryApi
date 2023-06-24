using AutoMapper;
using BlueBerry_API.Model;
using BlueBerry_API.Model.Dto;

namespace BlueBerry_API.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<MenuItem, MenuItemCreateDTO>().ReverseMap();
            CreateMap<MenuItem, MenuItemUpdateDTO>().ReverseMap();

            CreateMap<OrderHeader, OrderHeaderCreateDTO>().ReverseMap();
            CreateMap<OrderDetails, OrderDetailsCreateDTO>().ReverseMap();

            CreateMap<OrderHeader, OrderHeaderUpdateDTO>().ReverseMap();
          

           
        }
    }
}
