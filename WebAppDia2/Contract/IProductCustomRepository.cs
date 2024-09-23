using WebAppDia2.Contract.Dtos;
using WebAppDia2.Contract;
using WebAppDia2.Entities;

namespace WebAppDia3.Contract
{
    public interface IProductCustomRepository : IRepository<Product>
    {

        // Métodos adicionales si es necesario
        Task<IEnumerable<ProductDTO>> GetProductsPagedAsyncSp(string searchTerm, int pageNumber, int pageSize);


    }
}
