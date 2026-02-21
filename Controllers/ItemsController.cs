using Microsoft.AspNetCore.Mvc;
using Tasty_Treat_be.DTOs;
using Tasty_Treat_be.Interfaces.Service;

namespace Tasty_Treat_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly IItemService _itemService;
        private readonly IBlobStorageService _blobStorageService;
        private readonly IConfiguration _configuration;
        private readonly string _adminUploadsContainer;

        public ItemsController(IItemService itemService, IBlobStorageService blobStorageService, IConfiguration configuration)
        {
            _itemService = itemService;
            _blobStorageService = blobStorageService;
            _configuration = configuration;
            _adminUploadsContainer = _configuration["AzureBlobStorage:AdminUploadsContainer"] ?? "adminuploads";
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemDto>>> GetAll()
        {
            var items = await _itemService.GetAllAsync();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDto>> GetById(int id)
        {
            var item = await _itemService.GetByIdAsync(id);
            if (item == null)
                return NotFound($"Item with id {id} not found");

            return Ok(item);
        }

        [HttpGet("category/{category}")]
        public async Task<ActionResult<IEnumerable<ItemDto>>> GetByCategory(string category)
        {
            var items = await _itemService.GetByCategoryAsync(category);
            return Ok(items);
        }

        /// <summary>
        /// Creates a new item with optional image upload
        /// </summary>
        /// <param name="createItemDto">Item details (name, category, price, flavour, etc.)</param>
        /// <param name="image">Optional image file (jpg, png, gif, webp - max 5MB)</param>
        /// <returns>The created item with ImageUrl if image was uploaded</returns>
        [HttpPost]
        public async Task<ActionResult<ItemDto>> Create([FromForm] CreateItemDto createItemDto, IFormFile? image)
        {
            try
            {
                // Create the item first
                var item = await _itemService.CreateAsync(createItemDto);

                // Upload image if provided
                if (image != null && image.Length > 0)
                {
                    try
                    {
                        var imageUrl = await _blobStorageService.UploadImageAsync(image, _adminUploadsContainer);
                        item = await _itemService.UpdateImageAsync(item.ItemId, imageUrl);
                    }
                    catch (ArgumentException ex)
                    {
                        // If image upload fails, delete the created item and return error
                        await _itemService.DeleteAsync(item.ItemId);
                        return BadRequest($"Item created but image upload failed: {ex.Message}");
                    }
                }

                return CreatedAtAction(nameof(GetById), new { id = item.ItemId }, item);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Updates an existing item with optional image upload
        /// </summary>
        /// <param name="id">Item ID to update</param>
        /// <param name="updateItemDto">Fields to update (only non-null fields will be updated)</param>
        /// <param name="image">Optional new image file (replaces existing image)</param>
        /// <returns>The updated item</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<ItemDto>> Update(int id, [FromForm] UpdateItemDto updateItemDto, IFormFile? image)
        {
            try
            {
                // Update item details
                var item = await _itemService.UpdateAsync(id, updateItemDto);

                // Upload new image if provided
                if (image != null && image.Length > 0)
                {
                    try
                    {
                        // Delete old image if exists
                        if (!string.IsNullOrEmpty(item.ImageUrl))
                        {
                            await _blobStorageService.DeleteImageAsync(item.ImageUrl, _adminUploadsContainer);
                        }

                        // Upload new image
                        var imageUrl = await _blobStorageService.UploadImageAsync(image, _adminUploadsContainer);
                        item = await _itemService.UpdateImageAsync(id, imageUrl);
                    }
                    catch (ArgumentException ex)
                    {
                        return BadRequest($"Item updated but image upload failed: {ex.Message}");
                    }
                }

                return Ok(item);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _itemService.DeleteAsync(id);
            if (!result)
                return NotFound($"Item with id {id} not found");

            return NoContent();
        }

        [HttpPost("{id}/upload-image")]
        public async Task<ActionResult<ItemDto>> UploadImage(int id, IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return BadRequest("No file uploaded");
                }

                // Get existing item
                var item = await _itemService.GetByIdAsync(id);
                if (item == null)
                {
                    return NotFound($"Item with id {id} not found");
                }

                // Delete old image if exists
                if (!string.IsNullOrEmpty(item.ImageUrl))
                {
                    await _blobStorageService.DeleteImageAsync(item.ImageUrl, _adminUploadsContainer);
                }

                // Upload new image
                var imageUrl = await _blobStorageService.UploadImageAsync(file, _adminUploadsContainer);

                // Update item with new image URL
                var updateDto = new UpdateItemDto { };
                var updatedItem = await _itemService.UpdateImageAsync(id, imageUrl);

                return Ok(updatedItem);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error uploading image: {ex.Message}");
            }
        }

        [HttpDelete("{id}/delete-image")]
        public async Task<ActionResult<ItemDto>> DeleteImage(int id)
        {
            try
            {
                var item = await _itemService.GetByIdAsync(id);
                if (item == null)
                {
                    return NotFound($"Item with id {id} not found");
                }

                if (string.IsNullOrEmpty(item.ImageUrl))
                {
                    return BadRequest("Item has no image to delete");
                }

                // Delete image from blob storage
                await _blobStorageService.DeleteImageAsync(item.ImageUrl, _adminUploadsContainer);

                // Update item to remove image URL
                var updatedItem = await _itemService.UpdateImageAsync(id, null);

                return Ok(updatedItem);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error deleting image: {ex.Message}");
            }
        }
    }
}
