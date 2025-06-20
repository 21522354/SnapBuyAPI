﻿using AutoMapper;
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
            CreateMap<Voucher, MReq_Voucher>().ReverseMap();
            CreateMap<Voucher, MRes_Voucher>().ReverseMap();

            CreateMap<VoucherUsage, MReq_VoucherUsage>().ReverseMap();
            CreateMap<VoucherUsage, MRes_VoucherUsage>().ReverseMap();
        }
    }
}
