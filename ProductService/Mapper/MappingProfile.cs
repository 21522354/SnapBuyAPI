﻿using AutoMapper;
using ProductService.Models.Dtos.RequestModels;
using ProductService.Models.Dtos.ResponseModels;
using ProductService.Models.Entities;

namespace ProductService.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Category, MRes_Category>().ReverseMap();
            CreateMap<Category, MReq_Category>().ReverseMap();
            CreateMap<Product, MRes_Product>().ReverseMap();
            CreateMap<ProductImage, MRes_ProductImage>().ReverseMap();
            CreateMap<ProductVariant, MRes_ProductVariant>().ReverseMap();
            CreateMap<Tag, MRes_Tag>().ReverseMap();
            CreateMap<Tag, MReq_Tag>().ReverseMap();
            CreateMap<ProductReview, MReq_ProductReview>().ReverseMap();
            CreateMap<ProductReview, MRes_ProductReview>().ReverseMap();
        }
    }
}
