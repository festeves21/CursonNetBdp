using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using WebAppDia2.Contract;
using WebAppDia2.Data;
using WebAppDia2.Entities;

namespace WebAppDia2.Repositories
{
    public class ProductRepository: Repository<Product>, IProductRepository
    {

        public ProductRepository(ApplicationDbContext context) : base(context)
        {
        }


        // Método específico para ejecutar un procedimiento almacenado en el contexto de Product
        public async Task<IEnumerable<Product>> GetProductsPagedAsyncSp(string searchTerm, int pageNumber, int pageSize)
        {
            // Definir los parámetros para el procedimiento almacenado
            var parameters = new[]
            {
        new SqlParameter("@SearchTerm", SqlDbType.NVarChar,128) { Value = searchTerm == null ? "":searchTerm } ,
        new SqlParameter("@PageNumber", SqlDbType.Int) { Value = pageNumber } ,
        new SqlParameter("@PageSize", SqlDbType.Int) { Value = pageSize }
        };


            // Ejecutar un procedimiento almacenado que devuelve resultados
            var products = await ExecuteStoredProcedureWithResultsAsync("GetProductsPaged", parameters);

            return products;
            // Trabaja con los resultados
        }




        public async Task<IEnumerable<Product>> GetProductsPagedAsyncEf(string searchTerm, int pageNumber, int pageSize)
        {
            var query = _context.Products.AsQueryable();

            // Aplicar filtrado
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(p => p.Name.Contains(searchTerm));
            }

            // Aplicar paginación
            var products = await query
                .OrderBy(p => p.Name) // Ordenar por algún criterio
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return products;
        }

    }
}
