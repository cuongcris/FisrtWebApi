using MyFirstWebApi.Models;

namespace MyFirstWebApi.Services
{
    public interface ITypeRepository
    {
        List<TypeVm> GetAll();
        TypeVm GetById(int id);
        TypeVm AddNew(TypeVm vm);
        void Update(TypeVm vm);
        void Delete(int id);
    }
}
