using AutoMapper;
using OrderService.Models.Dtos.RequestModels;
using OrderService.Models.Dtos.ResponseModels;
using OrderService.Models.Entities;

namespace OrderService.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Order, MReq_Order>().ReverseMap();
            CreateMap<Order, MRes_Order>().ReverseMap();
            CreateMap<OrderItem, MRes_OrderItem>().ReverseMap();
            CreateMap<OrderItem, MReq_OrderItem>().ReverseMap();
        }
    }
}
