using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyFirstWebApi.Services;

namespace MyFirstWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        [HttpGet]
        public IActionResult GetAll(string? searchValue,double? from , double? to,string? sortBy, int page = 1)
        {
            try
            {
                var result = _productRepository.GetAll(searchValue,from,to,sortBy,page);
                return Ok(result);
                
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
