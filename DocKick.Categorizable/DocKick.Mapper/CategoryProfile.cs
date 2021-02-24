using System.Linq;
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
                .ForMember(q => q.Children, q => q.MapFrom(w => w.Children.ToArray()))
                .ForMember(q => q.ParentName, q => q.MapFrom(w => w.ParentId.HasValue ? w.Parent.Name : null))
                .ReverseMap()
                .ForMember(q => q.CategoryId, q => q.Ignore())
                .ForMember(q => q.Children, q => q.Ignore())
                .ForMember(q => q.Parent, q => q.Ignore());

            CreateMap<CategoryRequest, CategoryModel>();
        }
    }
}