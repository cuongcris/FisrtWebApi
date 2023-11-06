using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyFirstWebApi.Data;
using MyFirstWebApi.Models;

namespace MyFirstWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductTypesController : ControllerBase
    {
        public readonly AppDbContext _context;
        public ProductTypesController(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_context.types.ToList()) ;
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var type = _context.types.FirstOrDefault(p => p.TypeId == id);
            if(type == null)
            {
                return NotFound();
            }
            return Ok(type);
        }

        [HttpPost]
        [Authorize]  //only request already authorize to do
        public IActionResult Create(TypeModel type)
        {
            try
            {
                var pType = new ProductType()
                {
                    NameType = type.NameType,
                };

                _context.types.Add(pType);
                _context.SaveChanges();
                return Ok(new
                {
                    Success = true,
                    Data = pType
                });
            }
            catch
            {
                return BadRequest();
            }
           
        }

        [HttpPut("{id}")]
        public IActionResult UpdateTypeById(int id, TypeModel typeMv)
        {
            try
            {
                var type = _context.types.SingleOrDefault(p => p.TypeId == id);
                if (type == null)
                {
                    return NotFound();
                }
                if (id != type.TypeId)
                {
                    return BadRequest();
                }
                type.NameType = typeMv.NameType;
                _context.SaveChanges();
                return Ok(type);
            }
            catch
            {
                return BadRequest();
            }

        }
        [HttpDelete("{id}")]
        public IActionResult DeleteType(int id)
        {
            try
            {
                var type = _context.types.SingleOrDefault(p => p.TypeId == id);
                if (type == null)
                {
                    return NotFound();
                }
                _context.types.Remove(type);
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
