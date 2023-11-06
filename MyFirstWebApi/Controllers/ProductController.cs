using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyFirstWebApi.Data;
using MyFirstWebApi.Models;
using Product = MyFirstWebApi.Data.Product;

namespace MyFirstWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        public readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;    
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_context.Products.ToList());
        }
        [HttpGet("{id}")]
        public IActionResult GetById(string id)
        {
            try
            {
                var product = _context.Products.SingleOrDefault(p => p.Id == Guid.Parse(id));
                if (product == null)
                {
                    return NotFound();
                }
                return Ok(product);
            }catch 
            {
                return BadRequest();
            }
          
        }
        [HttpPost]
        public IActionResult Create(ProductVM productVM)
        {
            var product = new Product()
            {
                Id = Guid.NewGuid(),
                Name = productVM.ProductName,
                unitPrice = productVM.UnitPrice,
                Discount = productVM.Discount,
                TypeId = productVM.TypeId
            };

            _context.Add(product);
            _context.SaveChanges();
            return Ok(new
            {
                Success = true,
                Data = product
            });
        }

        [HttpPut("{id}")]
        public IActionResult UpdateProductById(string id ,Product Uproduct)
        {
            try
            {
                var product = _context.Products.SingleOrDefault(p => p.Id == Guid.Parse(id));
                if (product == null)
                {
                    return NotFound();
                }
                if(id != product.Id.ToString())
                {
                    return BadRequest();
                }
                product.Name = Uproduct.Name;
                product.unitPrice = Uproduct.unitPrice;
                _context.SaveChanges();
                return Ok(product);
            }
            catch
            {
                return BadRequest();
            }

        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            try
            {
                var product = _context.Products.SingleOrDefault(p => p.Id == Guid.Parse(id));
                if (product == null)
                {
                    return NotFound();
                }
                _context.Products.Remove(product);
                _context.SaveChanges();
                return Ok(new
                {
                    Success = true,
                });
            }
            catch
            {
                return BadRequest();  
            }

        }
    }
}
