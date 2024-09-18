using AutoMapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using WebAppDia2.Contract;
using WebAppDia2.Contract.Dtos;
using WebAppDia2.Controllers;
using WebAppDia2.Data;
using WebAppDia2.Entities;

namespace WebAppDia2.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {

        private readonly IMapper _mapper;
        public ProductRepository(ApplicationDbContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;
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


        public async Task<ProductDTO> GetProductDetailsByIdAsync(int id)
        {
            // Obtener el producto y los datos relacionados en una sola consulta Sin Autommapper
            var productDto = await _context.Products
                .Where(p => p.Id == id)
                .Include(p => p.Category)   // Cargar la categoría relacionada
                .Include(p => p.Supplier)   // Cargar el proveedor relacionado
                .Select(p => new ProductDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    CategoryName = string.IsNullOrWhiteSpace(p.Category.Name) ? "No Category" : p.Category.Name, // Validar y proporcionar valor predeterminado
                    SupplierName = string.IsNullOrWhiteSpace(p.Supplier.Name) ? "No Supplier" : p.Supplier.Name  // Validar y proporcionar valor predeterminado
                })
                .FirstOrDefaultAsync();

            //Con autoMapper
            var productFe = await _context.Products
                .Where(p => p.Id == id)
                .Include(p => p.Category)   // Cargar la categoría relacionada
                .Include(p => p.Supplier)   // Cargar el proveedor relacionado
                .FirstOrDefaultAsync();

            var produtDtoFe = _mapper.Map<ProductDTO>(productFe);

            return produtDtoFe;

        }

    }
}
