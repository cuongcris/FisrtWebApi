using Microsoft.EntityFrameworkCore;
using MyFirstWebApi.Data;
using MyFirstWebApi.Models;
using System.Globalization;

namespace MyFirstWebApi.Services
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;
        public static int PAGE_SIZE { get; set; } = 5;
        public ProductRepository(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }

        public ProductModel AddNew(ProductModel vm)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public List<ProductModel> GetAll(string search, double? from, double? to,string? sortBy,int page=1)
        {
            var allProducts = _context.Products.Include(hh=>hh.Type).AsQueryable(); //tạm dừng biên dịch
            
            #region Filtering
            if (!string.IsNullOrEmpty(search))
            {
                allProducts= allProducts.Where(p => p.Name.Contains(search) 
                );

            }
            if (from.HasValue)
            {
                allProducts = allProducts.Where(p =>  p.unitPrice>=from);

            }
            if (to.HasValue)
            {
                allProducts = allProducts.Where(p => p.unitPrice <= to );

            }
            #endregion
            #region Sorting
            //defaul
            allProducts = allProducts.OrderBy(p => p.Name);

            if (!String.IsNullOrEmpty(sortBy))
            {
                switch(sortBy)
                {
                    case "Price_asc": allProducts = allProducts.OrderBy(p => p.unitPrice);break;
                    case "Name_des": allProducts = allProducts.OrderByDescending(p => p.Name);break;
                    case "Price_des": allProducts = allProducts.OrderByDescending(p => p.unitPrice); break;
                }
            }

            #endregion
            #region Paging
            allProducts = allProducts.Skip((page-1)*PAGE_SIZE).Take(PAGE_SIZE);
            #endregion
           /* var products = allProducts.Select(t => new ProductModel
            {
               ProductId = t.Id,
               ProductName = t.Name,
               Discount = t.Discount,
               UnitPrice = t.unitPrice,
               TypeName = t.Type.NameType
            });
            return products.ToList();*/

            var result = PaginatedList<Data.Product>.Create(allProducts,page,PAGE_SIZE);
            return result.Select(t=> new ProductModel
            {
                ProductId = t.Id,
                ProductName = t.Name,
                Discount = t.Discount,
                UnitPrice = t.unitPrice,
                TypeName = t.Type?.NameType
            }).ToList();
        }

        public ProductModel GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(ProductModel vm)
        {
            throw new NotImplementedException();
        }
    }
}
