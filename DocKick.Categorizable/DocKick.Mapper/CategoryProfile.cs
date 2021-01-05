﻿using AutoMapper;
using DocKick.Dtos.Categories;
using DocKick.Entities.Category;

namespace DocKick.Mapper
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryModel>()
                .ReverseMap()
                .ForMember(q => q.CategoryId, q => q.Ignore());
        }
    }
}