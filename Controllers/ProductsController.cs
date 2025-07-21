using Microsoft.AspNetCore.Mvc;
using PortfolioBackend.Models;
using PortfolioBackend.Services;

namespace PortfolioBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public IActionResult GetProducts()
        {
            Products[] products = _productService.GetProducts()?.ToArray() ?? [];
            if (products.Length == 0) return NotFound("No products found.");
            return Ok(products);
        }

        [HttpGet("{id}")]
        public IActionResult GetProductById(int id)
        {
            Products? products = _productService.GetProductById(id);
            if (products == null) return NotFound($"Product with ID {id} not found.");
            return Ok(products);
        }

        [HttpPost]
        public IActionResult CreateProduct([FromBody] Products product)
        {
            if (product == null) return BadRequest("Product cannot be null.");
            _productService.CreateProduct(product);
            return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateProduct(int id, [FromBody] Products product)
        {
            if (product == null) return BadRequest("Product cannot be null.");
            _productService.UpdateProduct(id, product);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            _productService.DeleteProduct(id);
            return NoContent();
        }
    }
}