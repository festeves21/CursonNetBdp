using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAppDia2.Contract;
using WebAppDia2.Contract.Dtos;
using WebAppDia2.Entities;
using WebAppDia2.Repositories;
using WebAppDia3.Entities;

namespace WebAppDia2.Controllers
{

    [ApiController]
    //[Authorize]
    [Route("api/[controller]")]
    public class Producto2Controller: ControllerBase
    {
        private readonly IRepository<Product> _productsRepository;
        private readonly IRepository<Supplier> _supplierRepository;
        private readonly IRepository<Category> _categoryRepository;

        private readonly IProductRepository _productRepository;

        private readonly IMapper _mapper;

        private readonly ILogger<Producto2Controller> _logger;

        public Producto2Controller(IRepository<Product> productsRepository,
                                   IRepository<Supplier> supplierRepository,
                                   IRepository<Category> categoryRepository,
                                    IProductRepository productRepository, IMapper mapper,
                                    ILogger<Producto2Controller> logger) 
        {
            _productsRepository = productsRepository;
            _productRepository = productRepository;
            _mapper = mapper;

            _supplierRepository = supplierRepository;
            _categoryRepository = categoryRepository;

            _logger = logger;

        }


        [HttpGet("GetProducts")]
        public IQueryable<Product> GetAll()
        {
            var lista = _productsRepository.GetAll();
            return lista;
        }



        [HttpGet("GetProductsEf")]
        public async Task<IActionResult> GetProductsEf(
    [FromQuery] string searchTerm,
    [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var lista = await _productRepository.GetProductsPagedAsyncEf(searchTerm, pageNumber, pageSize);


            return Ok(lista);
        }

        [HttpGet("GetProductsSp")]
        public async Task<IActionResult> GetProductsSp(
           [FromQuery] string searchTerm,
           [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var lista = await _productRepository.GetProductsPagedAsyncSp(searchTerm, pageNumber, pageSize);


            return Ok(lista);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if(product==null)return NotFound();

            var productDto = _mapper.Map<ProductDTO>(product);

            return Ok(productDto);
        }

        [HttpGet("GetNamesArtesanal({id})")]
        public async Task<IActionResult> GetNames(int id)
        {
            var product = await _productsRepository
    .GetAll()
    .Where(i => i.Id == id).FirstOrDefaultAsync();
            ProductDTO resultado = new ProductDTO();

            Category category = new Category();
            Supplier supplier = new Supplier();

            try
            {
                if (product != null)
                {
                    category = await _categoryRepository
                         .GetAll()
                         .Where(i => i.Id == product.CategoryId).FirstOrDefaultAsync();


                    supplier = await _supplierRepository
                   .GetAll()
                   .Where(i => i.Id == product.SupplierId).FirstOrDefaultAsync();
                }

                resultado.Id = product.Id;
                resultado.Name = product.Name;
                resultado.Price = product.Price;
                resultado.CategoryName = category.Name;
                resultado.SupplierName = supplier.Name;

            }
            catch (Exception eex)
            {

                throw;
            }




            //if (product == null) return NotFound();

            //var productDto = _mapper.Map<ProductDTO>(product);

            return Ok(resultado);
        }


        [HttpGet("error")]
        public IActionResult Error()
        {
            _logger.LogInformation("Este es un mensaje de prueba desde TestController, antes del error.");
            _logger.LogDebug("Mensaje de depuración");
            _logger.LogWarning("Mensaje de advertencia");
            _logger.LogError("Mensaje de error");
            try
            {
                // Genera una excepción de prueba
                throw new Exception("Excepción de prueba en el controlador");
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex, "Hello World");
                // Usar el logger para registrar errores
                _logger.LogError(ex, "Ocurrió un error en la API.");
                return StatusCode(500, "Error interno en el servidor");
            }
        }

        [HttpGet("GetNamesEficiente/{id}")]
        public async Task<IActionResult> GetNamesEficiente(int id)
        {

            ProductDTO resultado = new ProductDTO();
            resultado = await _productRepository.GetProductDetailsByIdAsync(id);



            return Ok(resultado);
        }

    }
}
