using MyFirstWebApi.Models;

namespace MyFirstWebApi.Services
{
    public interface IProductRepository
    {
        List<ProductModel> GetAll(string search, double? from, double? to,string? sortBy,int page= 1);
        ProductModel GetById(int id);
        ProductModel AddNew(ProductModel vm);
        void Update(ProductModel vm);
        void Delete(int id);
    }
}
