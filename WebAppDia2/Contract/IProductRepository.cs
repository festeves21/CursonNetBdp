using WebAppDia2.Contract.Dtos;
using WebAppDia2.Entities;
using WebAppDia3.Contract.Dtos;

namespace WebAppDia2.Contract
{
    public interface IProductRepository : IRepository<Product>
    {

        //Metodos adicionales si es necesario


        Task<IEnumerable<Product>> GetProductsPagedAsyncSp(string searchTerm, int pageNumber, int pageSize);

        Task<IEnumerable<Product>> GetProductsPagedAsyncEf(string searchTerm, int pageNumber, int pageSize);

        Task<ProductDTO> GetProductDetailsByIdAsync(int id);

        Task<Boolean> UpdateInventAsync(int productId, int typeId, decimal ammount, int userId);

        Task<List<UserKardexSummaryDto>> GetKardexSummaryByUserAsync(DateTime startDate, DateTime endDate);

        Task<List<ProductDTO>> GetFullProductsAsync(string? searchTerm, int pageNumber, int pageSize);

    }
}
