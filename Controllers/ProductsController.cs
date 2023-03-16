using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tutor1.Models.Context;
using tutor1.Models.DTO;
using tutor1.Models.Entity;

namespace tutor1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ClinicContext _context;

        public ProductsController(ClinicContext context, IServiceProvider serviceProvider)
        {
            _context = context;
        }

        // Project products to product DTOs.
        private IQueryable<ProductDTO> MapProducts()
        {
            return from p in _context.products
                   select new ProductDTO()
                   { Id = p.ProductID, Name = p.Name, Price = p.Price };
        }


        // GET: api/Products
        [HttpGet]      
        public async Task<ActionResult<IEnumerable<Product>>> GetAll()
        {
            return await _context.products.ToListAsync();
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public ProductDTO GetProduct(int id)
        {
            var product = (from p in MapProducts()
                           where p.Id == 1
                           select p).FirstOrDefault();
            if (product == null)
            {
                return null;
            }
            return product;
        }

       

    }
}
