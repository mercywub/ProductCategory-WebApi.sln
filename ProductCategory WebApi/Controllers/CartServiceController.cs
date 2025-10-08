using Microsoft.AspNetCore.Mvc;
using ProductCategory_WebApi.Application.Services;

namespace ProductCategory_WebApi.Controllers
{


    [ApiController]
    [Route("api/[controller]")]
    public class CartServiceController : ControllerBase
    {
        private readonly CartService _cartService;

        public CartServiceController(CartService cartService)
        {
            _cartService = cartService;
        }

        // ✅ GET: api/Cart/{userId}
        [HttpGet("{userId:guid}")]
        public async Task<IActionResult> GetCartByUserId(Guid userId)
        {
            var cart = await _cartService.GetUserCartAsync(userId);
            if (cart == null)
                return NotFound("Cart not found for this user.");

            return Ok(cart);
        }

        // ✅ POST: api/Cart/AddItem
        [HttpPost("AddItem")]
        public async Task<IActionResult> AddToCart([FromQuery] Guid userId, [FromQuery] Guid productId, [FromQuery] int quantity)
        {
            if (quantity <= 0)
                return BadRequest("Quantity must be greater than zero.");

            await _cartService.AddToCartAsync(userId, productId, quantity);
            return Ok("Item added to cart successfully.");
        }

        // ✅ DELETE: api/Cart/RemoveItem
        [HttpDelete("RemoveItem")]
        public async Task<IActionResult> RemoveItem([FromQuery] Guid userId, [FromQuery] Guid productId)
        {
            var cart = await _cartService.GetUserCartAsync(userId);
            if (cart == null)
                return NotFound("Cart not found.");

            var item = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);
            if (item == null)
                return NotFound("Item not found in cart.");

            cart.CartItems.Remove(item);
            await _cartService.UpdateCartAsync(cart);

            return NoContent();
        }

        // ✅ PUT: api/Cart/UpdateItem
        [HttpPut("UpdateItem")]
        public async Task<IActionResult> UpdateItemQuantity([FromQuery] Guid userId, [FromQuery] Guid productId, [FromQuery] int quantity)
        {
            if (quantity <= 0)
                return BadRequest("Quantity must be greater than zero.");

            await _cartService.UpdateItemQuantityAsync(userId, productId, quantity);
            return Ok("Cart item updated successfully.");
        }

        // ✅ DELETE: api/Cart/Clear/{userId}
        [HttpDelete("Clear/{userId:guid}")]
        public async Task<IActionResult> ClearCart(Guid userId)
        {
            await _cartService.ClearCartAsync(userId);
            return NoContent();
        }
    }
}
