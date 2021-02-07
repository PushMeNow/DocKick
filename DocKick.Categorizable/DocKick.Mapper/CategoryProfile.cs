using AutoMapper;
using DocKick.Dtos.Categories;
using DocKick.Entities.Categories;

namespace DocKick.Mapper
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryModel>()
                .ReverseMap()
                .ForMember(q => q.CategoryId, q => q.Ignore());
            CreateMap<Category, CategoryRequest>()
                .ReverseMap()
                .ForMember(q => q.CategoryId, q => q.Ignore());
        }
    }
}