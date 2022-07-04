using Microsoft.AspNetCore.Mvc;
using MyWebApi.Dtos;
using MyWebApi.Entities;
using MyWebApi.Repositories;

namespace MyWebApi.Controllers
{
    [ApiController]
    [Route("items")]
    public class ItemsController : ControllerBase
    {
        private readonly IItemsRepository repository;
        private readonly ILogger<ItemsController> logger;

        public ItemsController(IItemsRepository repository, ILogger<ItemsController> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<ItemDto>> GetItemsAsync(string? name = null)
        {
            var items = (await repository.GetItemsAsync()).
                        Select( item => item.AsDto());

            if(!string.IsNullOrEmpty(name))
            {
                items = items.Where(item => item.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
            }

            logger.LogInformation($"{DateTime.UtcNow:hh:mm:ss}: Retrieved {items.Count()}");

            return items;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDto>> GetItemAsync(Guid id)
        {
            var item = await repository.GetItemAsync(id);

            if (item is null)
                return NotFound();

            return item.AsDto();
        }

        [HttpPost]
        public async Task<ActionResult<ItemDto>> CreateItemAsync(CreateItemDto itemDto)
        {
            Item item = new()
            {
                Id = Guid.NewGuid(),
                Name = itemDto.Name,
                Description = itemDto.Description,
                Price = itemDto.Price,
                CreatedDate = DateTime.UtcNow
            };

            if (!await repository.CreateItemAsync(item))
                return BadRequest(new { Error = "Object already contains." });

            return CreatedAtAction(nameof(GetItemAsync), new { id = item.Id }, item.AsDto());
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateItemAsync(Guid id, UpdateItemDto itemDto)
        {
            var existingItem = await repository.GetItemAsync(id);

            if (existingItem is null)
                return NotFound();

            existingItem.Name = itemDto.Name;
            existingItem.Price = itemDto.Price;

            await repository.UpdateItemAsync(existingItem);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteItemAsync(Guid id)
        {
            var item = await repository.GetItemAsync(id);

            if (item is null)
                return NotFound();

            await repository.DeleteItemAsync(id);

            return NoContent();
        }
    }
}
