using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAppDia2.Contract;
using WebAppDia2.Entities;
using WebAppDia2.Repositories;

namespace WebAppDia2.Controllers
{

    [ApiController]
    //[Authorize]
    [Route("api/[controller]")]
    public class Producto2Controller: ControllerBase
    {
        private readonly IRepository<Product> _productsRepository;
        private readonly IProductRepository _productRepository;

        public Producto2Controller(IRepository<Product> productsRepository, IProductRepository productRepository) 
        {
            _productsRepository = productsRepository;
            _productRepository = productRepository;
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



    }
}
