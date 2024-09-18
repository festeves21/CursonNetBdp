using WebAppDia2.Contract.Dtos;
using WebAppDia2.Entities;

namespace WebAppDia2.Contract
{
    public interface IProductRepository : IRepository<Product>
    {

        //Metodos adicionales si es necesario


        Task<IEnumerable<Product>> GetProductsPagedAsyncSp(string searchTerm, int pageNumber, int pageSize);

        Task<IEnumerable<Product>> GetProductsPagedAsyncEf(string searchTerm, int pageNumber, int pageSize);

        Task<ProductDTO> GetProductDetailsByIdAsync(int id);

    }
}
