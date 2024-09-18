using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAppDia2.Data;
using WebAppDia2.Entities;

namespace WebAppDia2.Controllers
{

    [ApiController]
    //[Authorize]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {

        protected readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;


        }

        [HttpGet("GetProducts")]
        public IQueryable<Product> GetAll()
        {
            var lista = _context.Products.AsQueryable();
            return lista;
        }

    }
}
