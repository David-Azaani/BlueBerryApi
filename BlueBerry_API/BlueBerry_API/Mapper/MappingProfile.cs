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

            //CreateMap<HotelRoomImage, HotelRoomImageDTO>().ReverseMap();

            //CreateMap<HotelAmenity, HotelAmenityDTO>().ReverseMap();

            //// CreateMap<RoomOrderDetails, RoomOrderDetailsDTO>().ReverseMap();


            //CreateMap<RoomOrderDetails, RoomOrderDetailsDTO>().ForMember(x => x.HotelRoomDTO, opt => opt.MapFrom(c => c.HotelRoom));
            //CreateMap<RoomOrderDetailsDTO, RoomOrderDetails>();
        }
    }
}
