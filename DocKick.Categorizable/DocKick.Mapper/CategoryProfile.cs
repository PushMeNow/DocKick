using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using AutoMapper.Internal;
using DocKick.Dtos.Categories;
using DocKick.Entities.Categories;

namespace DocKick.Mapper
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryModel>()
                .ForMember(q => q.Children, q => q.MapFrom(w => w.Children.ToList()))
                .ReverseMap()
                .ForMember(q => q.CategoryId, q => q.Ignore());

            CreateMap<Category, CategoryRequest>()
                .ReverseMap()
                .ForMember(q => q.CategoryId, q => q.Ignore());
        }

        public static ICollection<Category> LoadCategoryChildren(ICollection<Category> categories)
        {
            categories.ForAll(q =>
                              {
                                  q.Children.ToList();

                                  if (q.Children.Any())
                                  {
                                      LoadCategoryChildren(q.Children);
                                  }
                              });

            return categories;
        }
    }
}