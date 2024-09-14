using Implementation_Pagination_and_Filtering.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Implementation_Pagination_and_Filtering.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<IActionResult> GetProducts([FromQuery] ProductQueryParameters parameters)
        {
            var queryable = _context.Products.AsQueryable();

            // Filtering
            if (!string.IsNullOrEmpty(parameters.Category))
            {
                queryable = queryable.Where(p => p.Category == parameters.Category);
            }

            if (!string.IsNullOrEmpty(parameters.SearchTerm))
            {
                queryable = queryable.Where(p => p.Name.Contains(parameters.SearchTerm));
            }

            // Sorting
            if (!string.IsNullOrEmpty(parameters.SortBy))
            {
                if (parameters.SortBy == "price" && parameters.SortDescending)
                {
                    queryable = queryable.OrderByDescending(p => p.Price);
                }
                else if (parameters.SortBy == "price")
                {
                    queryable = queryable.OrderBy(p => p.Price);
                }
                else if (parameters.SortBy == "name" && parameters.SortDescending)
                {
                    queryable = queryable.OrderByDescending(p => p.Name);
                }
                else
                {
                    queryable = queryable.OrderBy(p => p.Name);
                }
            }

            // Pagination
            var totalItems = await queryable.CountAsync();
            var products = await queryable
                                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                                .Take(parameters.PageSize)
                                .ToListAsync();

            var response = new
            {
                TotalItems = totalItems,
                PageNumber = parameters.PageNumber,
                PageSize = parameters.PageSize,
                Data = products
            };

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] CreateProductDto createProductDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var product = new Product
            {
                Name = createProductDto.Name,
                Price = createProductDto.Price,
                Category = createProductDto.Category
            };

            _context.Products.Add(product);

            await _context.SaveChangesAsync();

            return Ok(product);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _context.Products.ToListAsync();
            return Ok(products);
        }
    }
}
