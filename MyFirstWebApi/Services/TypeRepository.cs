using MyFirstWebApi.Data;
using MyFirstWebApi.Models;

namespace MyFirstWebApi.Services
{
    public class TypeRepository : ITypeRepository
    {
        private readonly AppDbContext _context;
        public TypeRepository( AppDbContext appDbContext)
        {
            _context = appDbContext;

        }
        public TypeVm AddNew(TypeVm vm)
        {
            var _type = new ProductType
            {
                NameType = vm.NameType,
            };
            _context.Add(_type);
            _context.SaveChanges();
            return new TypeVm { 
                TypeId = _type.TypeId,
                NameType = _type.NameType
            };
        }

        public void Delete(int id)
        {
            var type = _context.types.FirstOrDefault(p => p.TypeId == id);
            if (type != null)
            {
                _context.types.Remove(type);
                _context.SaveChanges();
            }
        }

        public TypeVm GetById(int id)
        {
            var type = _context.types.FirstOrDefault(p => p.TypeId == id);
            if (type != null)
            {
                return new TypeVm { 
                    TypeId = type.TypeId,
                    NameType = type.NameType
                };
            }
            return null;
        }

        public List<TypeVm> GetAll() 
        {
            var types = _context.types.Select(t => new TypeVm
            {
                TypeId = t.TypeId,
                NameType = t.NameType,
            });
            return types.ToList();
        }

        public void Update(TypeVm vm)
        {
            var _type = _context.types.FirstOrDefault(p => p.TypeId == vm.TypeId);
            if (_type != null)
            {
                _type.NameType = vm.NameType;
                _context.SaveChanges();
            }
        }
    }
}
