using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyFirstWebApi.Data;
using MyFirstWebApi.Models;
using MyFirstWebApi.Services;

namespace MyFirstWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductTypeController : Controller
    {
        private readonly ITypeRepository _typeRepository;
        public ProductTypeController(ITypeRepository typeRepository)
        {
            _typeRepository = typeRepository; 
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                return Ok(_typeRepository.GetAll());
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("{id}")]
        public IActionResult GetTypeById(int id)
        {
            try
            {
                var data = _typeRepository.GetById(id);
                if(data == null)
                {
                    return NotFound();
                }
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Create(TypeVm vm)
        {
            try
            {
                _typeRepository.AddNew(vm);
                return Ok(vm);
            }
            catch(Exception ex) 
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPut("{id}")]
        public IActionResult UpdateTypeById(int id, TypeVm typeVm)
        {
            try
            {
                if (id !=  typeVm.TypeId)
                {
                    return BadRequest();
                }
                _typeRepository.Update(typeVm);
                return NoContent();
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }

        }

        [HttpDelete("{id}")]
        public IActionResult DeleteType(int id)
        {
            try
            {
                _typeRepository.Delete(id);
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
