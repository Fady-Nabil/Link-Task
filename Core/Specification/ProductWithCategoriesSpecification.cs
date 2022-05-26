using System;
using System.Collections.Generic;
using System.Text;
using Core.Entities;

namespace Core.Specification
{
    public class ProductWithCategoriesSpecification : BaseSpecipication<Product>
    {
        public ProductWithCategoriesSpecification(ProductSpecParams productParams) : base(x=>
            //(String.IsNullOrEmpty(productParams.Search) || x.Name.ToLower().Contains(productParams.Search)) &&
            (!productParams.CategoryId.HasValue || x.ProductCategoryId == productParams.CategoryId)
            )
        {
            AddInclude(x => x.ProductCategory);
            AddOrderBy(x => x.Name);
            ApplyPaging(productParams.PageSize * (productParams.PageIndex -1), productParams.PageSize);

            if (!string.IsNullOrEmpty(productParams.Sort))
            {
                switch (productParams.Sort)
                {
                    case "priceAsc":
                        AddOrderBy(p=>p.Price);
                        break;
                    case "priceDesc":
                        AddOrderByDescending(p=>p.Price);
                        break;
                    default:
                        AddOrderBy(n=>n.Name);
                        break;
                }
            }
        }

        public ProductWithCategoriesSpecification(int id) : base(x => x.Id == id)
        {
            AddInclude(x => x.ProductCategory);

        }
    }
}
