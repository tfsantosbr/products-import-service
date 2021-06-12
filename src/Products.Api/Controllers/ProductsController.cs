using Microsoft.AspNetCore.Mvc;
using Products.Api.Repositories;
using System;
using System.Threading.Tasks;

namespace Products.Api.Controllers
{
    [ApiController]
    [Route("stores/{storeId}/products")]
    public class ProductsController : ControllerBase
    {
        private readonly ProductRepository _productRepository;
        public ProductsController(ProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int storeId)
        {
            return Ok(await _productRepository.ListProducts(storeId));
        }

        [HttpGet("{productId}")]
        public async Task<IActionResult> Get(int storeId, Guid productId)
        {
            var productDetails = await _productRepository.GetDetails(storeId, productId);

            if (productDetails is null)
            {
                return NotFound(new { Message = "Product not found" });
            }

            return Ok(productDetails);
        }
    }
}
