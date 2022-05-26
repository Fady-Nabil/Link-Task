using API.Dtos.Category;
using API.Dtos.Product;
using AutoMapper;
using Core.Entities;

namespace API.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            /* Mapping Object */
            {
                CreateMap<Product, ProductToReturnDto>()
                    .ForMember(d => d.ProductCategory, o => o.MapFrom(s => s.ProductCategory.Name));
            }
        }
    }
}
