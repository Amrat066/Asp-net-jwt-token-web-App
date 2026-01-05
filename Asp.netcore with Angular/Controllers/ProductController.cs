using Asp.netcore_with_Angular.Model;
using Asp.netcore_with_Angular.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Asp.netcore_with_Angular.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly ProductRepository pr;

        public ProductController(ProductRepository prep)
        {
            this.pr = prep;
        }


        [HttpGet]
        public async Task<IActionResult> getProduct()
        {
            var product = await pr.getAll();
            if (product == null || product.Count == 0)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> saveProduct(Product pv)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await pr.saveAllProduct(pv);
            return Ok(new { message = "Product inserted successfully" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> updateproduct(int id, [FromBody] Product pro)
        {
            try
            {
                await pr.updateProduct(id, pro);
                return Ok(new { message = "Product updated successfully." });
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> deleteProduct(int id)
        {
            try
            {
                await pr.deleteProduct(id);
                return Ok(new { message = "Product deleted successfully." });
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

    }
}
